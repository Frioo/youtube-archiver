<script lang="ts">
	import Video from "$lib/Video.svelte";
	import { defaultThumbnail } from "$lib/utils";
	import VideoListItem from "./VideoListItem.svelte";

	export let json: any;
	let thumbnail = json?.thumbnails?.[json?.thumbnails.length - 1 || 0];
</script>

<div class="playlist">
	<div class="card playlist-head">
    <div class="thumbnail h-64 md:h-48 lg:h-36" style="background-image: url('{defaultThumbnail(json.thumbnails)}')">
    </div>
    <div class="flex flex-col gap-3">
      <h2 class="h2 text-xl font-bold">{json.title}</h2>
      <a class="chip variant-filled w-min" href={json.uploader_url}>{json.uploader}</a>
      <p>
        {json.playlist_count} videos
        &bull;
        {Intl.NumberFormat().format(json.view_count)} views
      </p>
      <p>{json.description || ''}</p>
    </div>
	</div>
	<div class="entries">
		{#each json.entries as entry}
			<VideoListItem video={entry} />
		{/each}
	</div>
</div>

<style lang="postcss">
  .playlist {
    @apply grid gap-6 grid-cols-1 lg:grid-cols-[auto_1fr];
  }

  .playlist-head {
    @apply variant-soft p-4 flex gap-3 flex-col sm:flex-row lg:flex-col;
  }

	.entries {
		@apply flex flex-col;
	}

	:global(.thumbnail) {
		@apply aspect-video bg-center bg-cover rounded-md;
	}
</style>
