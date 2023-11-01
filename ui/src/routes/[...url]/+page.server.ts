import { error } from '@sveltejs/kit';
import type { PageServerLoad } from './$types';
import { loadPlaylist, loadURL } from '$lib/youtube';

export const load: PageServerLoad = async ({ params, url }) => {
	const fullUrl = `${params.url}?${url.searchParams.toString()}`;
	const playlistId = url.searchParams.get('list');
	const videoId = url.searchParams.get('v');

	if (!playlistId && !videoId) {
		throw error(400, 'Invalid URL');
	}

	const data = await loadURL(fullUrl);

	return {
		playlistId,
		videoId,
		json: data,
		url: fullUrl
	};
};
