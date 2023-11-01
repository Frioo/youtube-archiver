import type { YTVideo } from '$lib/model/YTVideo.js';
import pocketbase from '$lib/pocketbase.js';
import { measurePerformance } from '$lib/utils.js';
import { loadChannelById, loadURL } from '$lib/youtube';
import { ClientResponseError, type RecordModel } from 'pocketbase';

const TAG = `[api/save]`;

export async function POST({ fetch, request, params }) {
	const t0 = performance.now();

	const { url } = await request.json();
	console.log(TAG, url);

	const json: YTVideo = await loadURL(url);

	const thumbRes = await fetch(json.thumbnail);
	const thumb = await thumbRes.blob();

	/* Get channel record id or create a new channel record */
	let channelId = await getChannelRecordId(json.channel_id);
	if (!channelId) {
		console.log(TAG, 'channel is not saved, creating record...');
		try {
			channelId = await createChannel(json.channel_id);
		} catch (err) {
			if (err instanceof ClientResponseError) {
				return new Response(JSON.stringify(err.toJSON()), { status: err.status });
			}
			return new Response(undefined, { status: 400 });
		}
		console.log(TAG, 'channel', channelId);
	}

	/* Payload for video record */
	const payload = new FormData();
	payload.set('video_id', json.id);
	payload.set('title', json.title);
	payload.set('description', json.description);
	payload.set('thumbnail', thumb);
	payload.set('channel', channelId);

	let record: RecordModel;
	try {
		record = await pocketbase.collection('videos').create(payload);
	} catch (err) {
		if (err instanceof ClientResponseError) {
			console.log(
				`[api/save] ${err.status} ${err.response.message}`,
				JSON.stringify(err.response.data)
			);
		}
		throw err;
	}

	console.log(TAG, 'video saved', record);
	console.log(TAG, 'done in', measurePerformance(t0));

	return new Response(JSON.stringify(record), { status: 200 });
}

async function getChannelRecordId(channelId: string): Promise<string | undefined> {
	try {
		const channel = await pocketbase
			.collection('channels')
			.getFirstListItem(`channel_id = '${channelId}'`, {
				fields: 'id,channel_id'
			});
		console.log('found existing channel ', channel);
		return channel.id;
	} catch (err) {
		console.log('channel lookup failed', err);
		return undefined;
	}
}

async function createChannel(channel_id: string): Promise<string> {
	const channel = await loadChannelById(channel_id);

	if (!channel) throw new Error('Failed to load channel');

	const { channel_url, uploader_id, uploader_url, title, thumbnails } = channel;
	console.log('[createChannel] ', uploader_id);

	const avatarUrl = thumbnails[thumbnails.length - 2].url;
	console.log('[createChannel] downloading channel avatar', avatarUrl);
	const avatarRes = await fetch(avatarUrl);
	const avatar = await avatarRes.blob();

	const payload = {
		channel_id,
		channel_url,
		handle: uploader_id,
		handle_url: uploader_url,
		name: title,
		avatar
	};

	try {
		const { id } = await pocketbase.collection('channels').create(payload);
		return id;
	} catch (err) {
		console.log('failed to create channel record', err);
		if (err instanceof ClientResponseError) {
			console.log(err.message, err.data);
		}

		throw err;
	}
}
