<script lang="ts">
	import { defaultThumbnail } from '$lib/utils';
	import dayjs from 'dayjs';
	import dayjsDuration from 'dayjs/plugin/duration';
	import dayjsRelativeTime from 'dayjs/plugin/relativeTime';
	import {
		BarChart2Icon,
		BarChartIcon,
		DownloadIcon,
		ExternalLinkIcon,
		EyeIcon,
		FileIcon,
		HeadphonesIcon,
		VideoIcon
	} from 'svelte-feather-icons';
	import type { YTVideo } from '$lib/model/YTVideo';
	import pocketbase from '$lib/pocketbase';
	import { ProgressRadial } from '@skeletonlabs/skeleton';

	dayjs.extend(dayjsDuration);
	dayjs.extend(dayjsRelativeTime);

	export let json: YTVideo;
	let { title, description, thumbnails, view_count, duration, channel, channel_url, formats } =
		json;

	let isSaved = false;
	let isSaving = false;

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

	async function handleSave() {
		isSaving = true;
		const res = await fetch('/api/save', {
			method: 'POST',
			body: JSON.stringify({ url: json.original_url })
		});
		isSaved = true;
		isSaving = false;
	}
</script>

<div class="card m-4 overflow-hidden">
	<div class="flex">
		<!-- Thumbnail -->
		<div
			class="thumbnail rounded-none relative w-80 h-fit"
			style="background-image: url('{defaultThumbnail(thumbnails)}')"
		>
			<span class="badge variant-filled absolute bottom-1 right-1">{durationText(duration)}</span>
		</div>
		<!-- Video info header -->
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

		<!-- Archive actions -->
		<div class="flex flex-col grow p-4 items-end justify-end">
			<button class="btn variant-filled px-6" class:variant-success={isSaved} on:click={handleSave} disabled={isSaving || isSaved}>
				{#if isSaving}
					<ProgressRadial width="w-6" />
				{:else}
					Save
				{/if}
			</button>
		</div>
	</div>
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

<div class="grid grid-cols-2 gap-2">
	<!-- Video only -->
	<div class="m-4 flex flex-col gap-2">
		<h2 class="h2 text-xl font-medium my-2">Video Only</h2>
		<div class="table-container card">
			<table class="table table-compact !bg-transparent table-interactive">
				<tbody>
					{#each videoFormats as f}
						<tr class="row">
							<td class="">
								<span class="text-base">{f.format_note}{f.fps}</span>
							</td>
							<td>
								<span class="badge variant-soft">
									<VideoIcon size="1.25x" class="mr-2" />
									{f.ext} / {f.vcodec.split('.')[0]}
								</span>
							</td>
							<td class="w-full !p-0 !m-0" />
							<!-- Audio codec -->
							<!-- <td>
                <span class="badge variant-soft">
                  <HeadphonesIcon size="1.25x" class="mr-2" />
                  {f.acodec}
                </span>
              </td> -->
							<!-- Video codec -->
							<!-- <td>
                <div class="variant-soft bg-transparent">
                  {f.vcodec.split('.')[0]}
                </div>
              </td> -->
							<!-- Video bitrate -->
							<td>
								<div class="variant-soft bg-transparent w-max ml-auto mr-0">
									{Math.round(f.vbr)} Kbps
								</div>
							</td>
						</tr>
					{/each}
				</tbody>
			</table>
		</div>
	</div>

	<!-- Audio only -->
	<div class="m-4 flex flex-col gap-2">
		<h2 class="h2 text-xl font-medium my-2">Audio Only</h2>
		<div class="table-container card">
			<table class="table table-compact !bg-transparent table-interactive">
				<tbody>
					{#each audioFormats as f}
						<tr class="row">
							<td class="">
								<span class="text-base">{f.format_note}</span>
							</td>
							<td>
								<span class="badge variant-soft">
									<HeadphonesIcon size="1.25x" class="mr-2" />
									{f.ext} / {f.acodec}
								</span>
							</td>
							<td class="w-full !p-0 !m-0" />
							<!-- Audio codec -->
							<!-- <td>
                <span class="badge variant-soft">
                  <HeadphonesIcon size="1.25x" class="mr-2" />
                  {f.acodec}
                </span>
              </td> -->
							<!-- Video bitrate -->
							<td>
								<div class="variant-soft bg-transparent w-max mr-0 ml-auto">
									{Math.round(f.abr)} Kbps
								</div>
							</td>
						</tr>
					{/each}
				</tbody>
			</table>
		</div>
	</div>
</div>

<style lang="postcss">
	tr.row:nth-child(even) {
		@apply bg-transparent;
	}
</style>
