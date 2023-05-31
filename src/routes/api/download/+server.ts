import { error } from '@sveltejs/kit';
import https from 'node:https';
import type { RequestHandler } from './$types';

export async function GET({ url }): Promise<RequestHandler> {
	const val = url.searchParams.get('url');
	if (!val) {
		throw error(400, 'Missing URL');
	}
	const videoUrl = new URL(val);
	const mime = videoUrl.searchParams.get('mime');

	const res = await fetch(videoUrl);
	return new Response(res.body, {
		headers: {
			'Content-Type': mime
		}
	});
}
