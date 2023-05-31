<script lang="ts">
	import { defaultThumbnail } from '$lib/utils';
	import dayjs from 'dayjs';
	import dayjsDuration from 'dayjs/plugin/duration';
	import dayjsRelativeTime from 'dayjs/plugin/relativeTime';
	import {
		DownloadIcon,
		ExternalLinkIcon,
		EyeIcon,
		FileIcon,
		HeadphonesIcon,
		VideoIcon
	} from 'svelte-feather-icons';
	dayjs.extend(dayjsDuration);
	dayjs.extend(dayjsRelativeTime);

	export let json: any;
	let { title, description, thumbnails, view_count, duration, channel, channel_url, formats } =
		json;

	let audioFormats: any[] = [];
	let videoFormats: any[] = [];
	let muxedFormats: any[] = [];
	$: if (formats) {
		let af = [];
    let vf = [];
    let mf = [];
    for (let f of formats) {
      const { acodec, vcodec } = f;
      if (acodec !== 'none' && vcodec !== 'none') {
        mf.push(f);
      } else if (acodec === 'none' && vcodec !== 'none') {
        vf.push(f);
      } else if (acodec !== 'none' && vcodec === 'none') {
        af.push(f);
      }
    }
    audioFormats = af;
    videoFormats = vf;
    muxedFormats = mf;
	}

	function durationText(seconds: number) {
		const dur = dayjs.duration(seconds, 's');
		if (seconds < 60) {
			return `${seconds}s`;
		} else if (seconds < 60 * 60) {
			return dur.format('mm:ss');
		} else {
			return dur.format('HH:mm:ss');
		}
	}

	function filesizeText(bytes: number) {
		const sizes = ['B', 'KiB', 'MiB', 'GiB', 'TiB'];
		let i = 0;
		while (bytes > 1024) {
			bytes = bytes / 1024;
			i += 1;
		}

		bytes = Math.round(bytes);

		return `${bytes}${sizes[i]}`;
	}
</script>

<div class="card m-4 overflow-hidden">
	<div class="flex">
		<div
			class="thumbnail rounded-none relative w-80 h-fit"
			style="background-image: url('{defaultThumbnail(thumbnails)}')"
		>
			<span class="badge variant-filled absolute bottom-1 right-1">{durationText(duration)}</span>
		</div>
		<div class="flex flex-col w-fit p-4">
			<h2 class="h2 text-xl font-medium">{title}</h2>
			<div class="flex mt-2.5 mb-4 gap-2">
				<a
					href={channel_url}
					target="_blank"
					class="badge variant-soft transition-colors duration-100 hover:variant-filled-primary"
				>
					<ExternalLinkIcon size="1.25x" class="mr-1.5" />
					{channel}
				</a>
				<span class="badge variant-soft">
					<EyeIcon size="1.25x" class="mr-1.5" />
					{Intl.NumberFormat().format(view_count)} views
				</span>
			</div>
		</div>
	</div>
</div>

<!-- Video only -->
<div class="grid grid-cols-2 gap-8">
	<div class="m-4 flex flex-col gap-2 w-full">
		<h2 class="h2 text-xl font-medium my-2">Video Only</h2>
		{#each videoFormats as f}
			<div class="card px-4 py-2.5 flex gap-2 items-center">
				{f.format_note}{f.fps}
				<span class="badge variant-soft">
					{f.ext}
				</span>
				<span class="badge variant-soft">
					<HeadphonesIcon size="1.25x" class="mr-2" />
					{f.acodec}
				</span>
				<!-- <span class="badge variant-soft">
        </span> -->
				<div class="badge variant-soft">
					<VideoIcon size="1x" class="mr-2" />
					{f.vcodec}
				</div>

				<div class="grow w-8" />

				<a href={f.url} target="_blank" class="btn btn-sm variant-filled-primary">
					<DownloadIcon size="1.25x" class="mr-2" />
					Download
				</a>
			</div>
		{/each}
	</div>

	<!-- Quick download -->
	<div class="m-4 flex flex-col gap-2">
		<h2 class="h2 text-xl font-medium my-2">Quick Download</h2>
		{#each muxedFormats as f}
			<div class="card px-4 py-2.5 flex gap-2 items-center">
				{f.format_note}{f.fps}
				<span class="badge variant-soft">
					{f.ext}
				</span>
				<span class="badge variant-soft">
					<HeadphonesIcon size="1.25x" class="mr-2" />
					{f.acodec}
				</span>
				<div class="badge variant-soft">
					<VideoIcon size="1x" class="mr-2" />
					{f.vcodec}
				</div>

				<div class="grow-[2]" />

				<a
					href="/api/download?url={encodeURIComponent(f.url)}"
					target="_blank"
					download={title}
					class="btn btn-sm variant-filled-primary grow"
				>
					<DownloadIcon size="1.25x" class="mr-2" />
					{filesizeText(f.filesize || f.filesize_approx)}
				</a>
			</div>
		{/each}
	</div>
</div>
