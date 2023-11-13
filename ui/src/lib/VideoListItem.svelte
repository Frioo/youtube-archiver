<script lang="ts">
  import InfoIcon from "@iconify-icons/material-symbols/info-rounded";
	import Icon from "@iconify/svelte";
	import type { Video } from "./model/Video";

  export let video: Video;
</script>

<div class="entry">
  <div class="thumbnail video-thumbnail" style="background-image: url('TODO')">
    <a href="/{video.id}" id="btn-video" class="btn-icon variant-filled">
      <Icon icon={InfoIcon} />
    </a>
  </div>
  <div class="flex flex-col justify-start ml-2">
    <div class="font-medium opacity-90">{video.title}</div>
    <div class="text-gray-800 text-base">{video.channelName || ''}</div>
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