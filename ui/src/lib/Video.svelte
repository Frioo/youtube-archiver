<script lang="ts">
	import { defaultThumbnail } from "$lib/utils";
  import { InfoIcon } from "svelte-feather-icons";
	import type { VideosResponse } from "./pocketbase-types";
	import { PUBLIC_POCKETBASE_URL } from "$env/static/public";

  export let video: VideosResponse;
  $: ({ title, thumbnail, video_id, collectionId, id, expand } = video)
</script>

<div class="flex gap-x-2">
  <div class="rounded-sm aspect-[16/9] bg-cover bg-center" style="background-image: url('{PUBLIC_POCKETBASE_URL}/api/files/{collectionId}/{id}/{thumbnail}')">
    <div class="w-48"></div>
  </div>
  <div class="">
    <span class="inline-block font-semibold leading-snug mb-1">{title}</span>
    <a href={expand?.channel?.handle_url} target="_blank" class="block hover:text-blue-700">{expand?.channel?.name}</a>
  </div>
</div>

<!-- <div class="entry">
  <div class="thumbnail video-thumbnail" style="background-image: url('/{thumbnail}')">
    <a href="/{video_id}" id="btn-video" class="btn-icon variant-filled">
      <InfoIcon />
    </a>
  </div>
  <div class="flex flex-col justify-start ml-2">
    <div class="font-medium opacity-90">{title}</div>
    <div class="text-gray-800 text-base">{channel || ''}</div>
  </div>
</div> -->

<style lang="postcss">
  .entry {
		@apply flex py-1;
	}

  .video-thumbnail {
    @apply h-16 grid place-items-center;
  }

  .video-thumbnail #btn-video {
    @apply invisible opacity-0 transition-all duration-150 ease-in-out;
  }

  .video-thumbnail:hover #btn-video {
    @apply visible opacity-100;
  }
</style>