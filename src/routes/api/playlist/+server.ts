import { error } from '@sveltejs/kit';
import type { RequestHandler } from './$types';
import { exec } from 'child_process';
import { loadPlaylist } from '$lib/youtube';

export const GET: RequestHandler = async ({ params, url }) => {
	const playlistURL = url.searchParams.get('url');

	if (!playlistURL) {
		throw error(400, 'Missing playlist URL');
	}

	try {
		const data = await loadPlaylist(playlistURL);
		return new Response(data);
	} catch (err) {
		throw error(500, err);
	}
};
