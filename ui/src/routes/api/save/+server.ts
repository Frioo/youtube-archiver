import type { YTVideo } from '$lib/model/YTVideo.js';
import { measurePerformance } from '$lib/utils.js';
import { env } from '$env/dynamic/private';
import type { YTUrl } from '$lib/model/YTUrl.js';

const TAG = `[api/save]`;

export async function POST({ fetch, request, params }) {
	const t0 = performance.now();
	const ytUrl: YTUrl = await request.json();

	if (!ytUrl.videoId) {
		return new Response('Missing video id', { status: 400 });
	}

	const url = new URL(env.API_URL + '/video/save');
	url.searchParams.append('id', ytUrl.videoId);
	const res = await fetch(url);

	return res;

	//return new Response(JSON.stringify(record), { status: 200 });
}
