<script lang="ts">
	import { defaultThumbnail } from "$lib/utils";
  import { InfoIcon } from "svelte-feather-icons";
	import type { Video } from "./model/Video";
	import type { YTVideo } from "./model/YTVideo";
  import { PUBLIC_ASSETS_URL } from "$env/static/public";

  export let video: Video;

  function thumbnailUrl(): string {
    if (!video.thumbnail) return "";

    if (video.thumbnail.startsWith("https://")) {
      return video.thumbnail;
    }
    
    return PUBLIC_ASSETS_URL + video.thumbnail;
  }

  function channelUrl(): string | undefined {
    const yt = "https://youtube.com";
    if (video.channelHandle) return `${yt}/${video.channelHandle}`;
    else if (video.channelId) return `${yt}/channel/${video.channelId}`;
    else return undefined;
  }
</script>

<div class="flex gap-x-2">
  <div class="rounded-sm aspect-[16/9] bg-contain bg-center w-48 h-fit flex-shrink-0 flex-grow-0" style="background-image: url('{thumbnailUrl()}')">
  </div>
  <div class="">
    <span class="inline-block font-medium leading-snug mb-1.5">{video.title}</span>
    <a href={channelUrl()} target="_blank" class="block text-sm hover:text-blue-700">{video.channelName}</a>
  </div>
</div>

<style lang="postcss">

</style>