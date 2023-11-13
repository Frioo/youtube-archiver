import type { Video } from '$lib/model/Video.js';
import { env } from '$env/dynamic/private';

const tag = '[server: /]';

export async function load({ fetch }) {
	let videos: Video[] = [];

	try {
		const url = env.API_URL + '/video';
		const res = await fetch(url);
		videos = await res.json();
		console.log(tag, `Fetched ${videos.length} videos`);
	} catch (err) {
		console.error(tag, err);
	}

	return {
		videos
	};
}
