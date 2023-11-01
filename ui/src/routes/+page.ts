import pocketbase from '$lib/pocketbase.js';

export async function load({ fetch }) {
	const videos = await pocketbase.collection('videos').getFullList({
		expand: 'channel'
	});

	return {
		videos
	};
}
