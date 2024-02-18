<script lang="ts">
  import { toastStore, ProgressBar } from "@skeletonlabs/skeleton";
	import Playlist from "$lib/Playlist.svelte";
  import Video from "$lib/Video.svelte";
	import type { PageData } from "./$types";
	import { PUBLIC_POCKETBASE_URL } from "$env/static/public";
	import { invalidateAll } from "$app/navigation";
	import UrlInput from "$lib/URLInput.svelte";
  import type { YTUrl } from "$lib/model/YTUrl";
	import VideoListItem from "$lib/VideoListItem.svelte";
	import { BottomSheet, Card, CircularProgress, CircularProgressIndeterminate } from "m3-svelte";
	import type { YTVideo } from "$lib/model/YTVideo";
  import type { Video as VideoType } from "$lib/model/Video";
	import Button from "$lib/components/Button.svelte";
	import { save } from "$lib/backend";

  export let data: PageData;
  let { videos } = data;
  $: ({ videos } = data);

  let playlists: string[] = [];
  let ytUrl: YTUrl;
  let loading = false;
  let json: any = undefined;
  let error: any = undefined;
  let showSheet = false;
  let videoData: YTVideo;

  function toVideo(videoData: YTVideo): VideoType {
    const res: VideoType = {
      id: videoData.id,
      channelId: videoData.channelID,
      channelHandle: videoData.uploaderID,
      channelName: videoData.uploader,
      thumbnail: videoData.thumbnail,
      title: videoData.title,
      description: videoData.description,
      createdAt: new Date(videoData.upload_date)
    }

    console.log("loaded video", res);

    return res;
  }

  /* async function handleAdd() {
    loading = true;
    const pa = new URLSearchParams()
    const resp = await fetch(`/api/playlist?url=${encodeURIComponent(url)}`);
    const respJson = await resp.json();

    if (resp.status !== 200) {
      error = respJson;
      toastStore.trigger({ 
        message: error.message,
        background: 'variant-filled-warning',
      });
    } else {
      json = respJson;
      playlists = [...playlists, url]
      url = "";
    }
    loading = false;
  } */

  async function handleVideo() {
    

    invalidateAll();
  }

  async function handleUrl(e: CustomEvent) {
    loading = true;
    ytUrl = e.detail;

    console.log("loading data for", ytUrl);

    const res = await fetch('/api/load', {
      method: 'POST',
      body: JSON.stringify({ url: ytUrl.url, id: ytUrl.videoId }),
    })

    videoData = await res.json();
    console.log(videoData);
    showSheet = true;
    loading = false;
  }

  async function handleSave() {
    const res = await save(ytUrl);
    invalidateAll();
  }

  async function handleDownload() {

  }
</script>

<!-- {#if showSheet}
  <BottomSheet on:close={() => showSheet = false}>
    <div class="flex flex-col pb-4 w-screen h-screen">
      {videoData.title}
    </div>
  </BottomSheet>
{/if} -->

<!-- YouTube URL input -->
<div class="m-4 flex gap-2 relative">
  <UrlInput on:submit={handleUrl} disabled={loading} />
  {#if loading}
    <div class="absolute top-0 right-4 h-full items-center flex w-8">
      <CircularProgressIndeterminate />
    </div>
  {/if}
</div>

<!-- Data -->
<!-- <div class="p-4">
  {#if loading}
    <div
      class="flex flex-col w-48 mx-auto my-6 gap-4 items-center">
      Loading playlist
      <ProgressBar type="" />
    </div>
  {/if}
  {#if json}
    <Playlist {json} />
  {/if}
</div> -->

<div class="px-4">
  {#if showSheet}
    <Card type="filled">
      <Video video={toVideo(videoData)} />
      <Button type="elevated" on:click={handleDownload}>Download</Button>
      <Button type="elevated" on:click={handleSave}>Save</Button>
    </Card>
  {/if}
</div>

<h2 class="text-3xl font-semibold px-4 mb-2">Your videos</h2>
<div class="grid grid-cols-3 gap-4 p-4">
  {#each videos as v}
    <Video video={v} />
  {/each}
</div>