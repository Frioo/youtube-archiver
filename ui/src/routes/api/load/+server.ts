import { env } from '$env/dynamic/private';
import { parseUrl } from '$lib/model/YTUrl.js';
import { loadURL } from '$lib/youtube.js';

const tag = '[api/load]';

/* export async function POST({ fetch, params, request }) {
	const body = await request.json();
	const ytUrl = parseUrl(<string>body.url);

	if (!ytUrl) {
		return new Response('Invalid URL', { status: 400 });
	}

	try {
		const json = await loadURL(ytUrl.url);
		return new Response(JSON.stringify(json), { status: 200 });
	} catch (err) {
		console.error(tag, 'failed to load json for url:', ytUrl);
		return new Response(undefined, { status: 500 });
	}
} */

export async function POST({ fetch, params, request }) {
	const body = await request.json();
	const ytUrl = parseUrl(<string>body.url);

	if (!ytUrl || !ytUrl.videoId) {
		return new Response('Invalid URL', { status: 400 });
	}

	try {
		const url = new URL(env.API_URL + '/video/json');
		url.searchParams.append('id', ytUrl.videoId);
		const res = await fetch(url);
		//const data = await res.json();
		return new Response(res.body, { status: res.status });
	} catch (err) {
		console.error(tag, 'failed to load json for url:', ytUrl, err);
		return new Response(undefined, { status: 500 });
	}
}
