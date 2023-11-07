import { Collections } from '$lib/pocketbase-types.js';
import type { ChannelsResponse, VideosResponse } from '$lib/pocketbase-types.js';
import pocketbase from '$lib/pocketbase.js';

export async function load({ fetch }) {
	const videos = await pocketbase.collection(Collections.Videos).getFullList<VideosResponse>({
		expand: 'channel'
	});

	return {
		videos
	};
}
