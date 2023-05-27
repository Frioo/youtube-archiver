<script lang="ts">
	export let json: any;
	let thumbnail = json?.thumbnails?.[json?.thumbnails.length - 1 || 0];

	function defaultThumbnail(thumbnails: any) {
		return thumbnails[thumbnails.length - 1 || 0].url;
	}
</script>

<div class="playlist">
	<div class="card variant-soft px-4 py-4 flex flex-col">
    <div class="thumbnail w-80 mb-3" style="background-image: url('{defaultThumbnail(json.thumbnails)}')">
    </div>
		<h2 class="h2 mb-3 text-xl font-bold">{json.title}</h2>
    <a class="chip variant-filled w-min" href={json.uploader_url}>{json.uploader}</a>
		<p>
      {json.playlist_count} videos
      &bull;
      {Intl.NumberFormat().format(json.view_count)} views
    </p>
		<p>{json.description || ''}</p>
	</div>
	<div class="entries">
		{#each json.entries as { title, channel, thumbnails }}
			<div class="entry">
				<div class="thumbnail h-16" style="background-image: url('{defaultThumbnail(thumbnails)}')" />
				<div class="flex flex-col justify-center ml-4">
					<div class="font-medium opacity-90">{title}</div>
					<div class="text-gray-800 text-base">{channel || ''}</div>
				</div>
			</div>
		{/each}
	</div>
</div>

<style lang="postcss">
  .playlist {
    @apply grid gap-6 grid-cols-1 lg:grid-cols-[auto_1fr];
  }

	.entries {
		@apply flex flex-col gap-2;
	}

	.entry {
		@apply flex;
	}

	.thumbnail {
		@apply aspect-video bg-center bg-cover rounded-md;
	}
</style>
