<script lang="ts">
	import { defaultThumbnail } from "$lib/utils";
  import { InfoIcon } from "svelte-feather-icons";

  export let video;
  let { title, channel, thumbnails, webpage_url } = video;
</script>

<div class="entry">
  <div class="thumbnail video-thumbnail" style="background-image: url('{defaultThumbnail(thumbnails)}')">
    <a href="/{webpage_url}" id="btn-video" class="btn-icon variant-filled">
      <InfoIcon />
    </a>
  </div>
  <div class="flex flex-col justify-start ml-2">
    <div class="font-medium opacity-90">{title}</div>
    <div class="text-gray-800 text-base">{channel || ''}</div>
  </div>
</div>

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