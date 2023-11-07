import type { YTUrl } from '$lib/model/YTUrl';

export function defaultThumbnail(thumbnails: any) {
	return thumbnails[thumbnails.length - 1 || 0].url;
}

export function measurePerformance(t0: number): string {
	const perf = performance.now() - t0;
	if (perf > 1000) {
		return `${Number(perf / 1000).toFixed(2)}s`;
	} else {
		return `${perf}ms`;
	}
}

export function parseUrl(url: string) {
	const parsed = new URL(url);
	if (!parsed) {
		// invalid url
	}

	const { hostname, pathname, searchParams } = parsed;
	const res: YTUrl = { url };

	if (hostname.endsWith('youtu.be')) {
		// youtu.be/video_id => video_id
		res.videoId = pathname.slice(1);
	} else if (hostname.split('.').includes('youtube')) {
		res.videoId = searchParams.get('v') || undefined;
	}

	res.playlistId = searchParams.get('list') || undefined;
	res.index = Number.parseInt(searchParams.get('index')!) || undefined;
	res.t = Number.parseInt(searchParams.get('t')!) || undefined;

	return res;
}
