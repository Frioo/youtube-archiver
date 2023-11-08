export interface YTUrl {
	url: string;
	videoId?: string;
	playlistId?: string;
	index?: number;
	t?: number;
}

export function parseUrl(url: string) {
	const parsed = new URL(url);
	if (!parsed) {
		// invalid url
		throw new Error('Could not parse URL: invalid format\n' + url);
	}

	const { hostname, pathname, searchParams } = parsed;
	const res: YTUrl = { url };

	if (hostname.endsWith('youtu.be')) {
		// youtu.be/video_id => video_id
		res.videoId = pathname.slice(1);
	} else if (hostname.split('.').includes('youtube')) {
		res.videoId = searchParams.get('v') || undefined;
	}

	if (!res.videoId && !res.playlistId) {
		return undefined;
	}

	res.playlistId = searchParams.get('list') || undefined;
	res.index = Number.parseInt(searchParams.get('index')!) || undefined;
	res.t = Number.parseInt(searchParams.get('t')!) || undefined;

	return res;
}
