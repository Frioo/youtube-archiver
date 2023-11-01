<script lang="ts">
	import Playlist from "$lib/Playlist.svelte";
  import { toastStore, ProgressBar } from "@skeletonlabs/skeleton";
	import type { PageData } from "./$types";
	import { PUBLIC_POCKETBASE_URL } from "$env/static/public";
	import { invalidateAll } from "$app/navigation";

  export let data: PageData;
  let { videos } = data;
  $: ({ videos } = data);

  let playlists: string[] = [];
  let url: string = "";
  let loading = false;
  let json: any = undefined;
  let error: any = undefined;

  async function handleAdd() {
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
  }

  async function handleKeyDown(e: KeyboardEvent) {
    if (e.key === 'Enter') {
      await handleAdd();
    }
  }

  async function handleVideo() {
    const res = await fetch('/api/save', {
      method: 'POST',
      body: JSON.stringify({ url }),
    });

    if (res.status !== 200) {
      console.log(await res.json());
      return;
    }

    invalidateAll();
  }
</script>

<div class="p-4 flex gap-2">
  <div class="input-group input-group-divider grid-cols-[1fr_auto]">
    <input
      bind:value={url}
      on:keydown={handleKeyDown}
      class="input"
      type="url"
      name="url"
      placeholder="Playlist URL">
    <button class="variant-filled" on:click={handleAdd}>
      Fetch
    </button>
  </div>
</div>
<div class="p-4 flex gap-2">
  <div class="input-group input-group-divider grid-cols-[1fr_auto]">
    <input
      bind:value={url}
      on:keydown={(e) => e.key === 'Enter' && handleVideo()}
      class="input"
      type="url"
      name="url"
      placeholder="Video URL">
    <button class="variant-filled" on:click={() => handleVideo()}>
      Fetch
    </button>
  </div>
</div>
<div class="p-4">
  {#if loading}
    <div
      class="flex flex-col w-48 mx-auto my-6 gap-4 items-center">
      Loading playlist
      <ProgressBar />
    </div>
  {/if}
  {#if json}
    <Playlist {json} />
  {/if}
</div>

<h2 class="text-3xl font-semibold px-4 mb-2">Your videos</h2>
<div class="grid grid-cols-3 gap-4 p-4">
  {#each videos as { title, video_id, thumbnail, id, collectionId, expand }}
    <div class="flex gap-x-2">
      <div class="rounded-sm aspect-[16/9] bg-cover bg-center" style="background-image: url('{PUBLIC_POCKETBASE_URL}/api/files/{collectionId}/{id}/{thumbnail}')">
        <div class="w-48"></div>
      </div>
      <div class="">
        <span class="inline-block font-semibold leading-snug mb-1">{title}</span>
        <a href={expand?.channel?.handle_url} target="_blank" class="block hover:text-blue-700">{expand?.channel?.name}</a>
      </div>
    </div>
  {/each}
</div>