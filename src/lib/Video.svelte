<script lang="ts">
	import { defaultThumbnail } from "$lib/utils";

  export let video;
  let { title, channel, thumbnails } = video;
</script>

<div class="entry">
  <div class="thumbnail h-16" style="background-image: url('{defaultThumbnail(thumbnails)}')" />
  <div class="flex flex-col justify-start ml-2">
    <div class="font-medium opacity-90">{title}</div>
    <div class="text-gray-800 text-base">{channel || ''}</div>
  </div>
</div>

<style lang="postcss">
  .entry {
		@apply flex py-1;
	}
</style>