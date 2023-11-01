import { exec, spawn } from 'node:child_process';
import YTDLP from 'yt-dlp-wrap';
import type { YTChannel } from './model/YTChannel';

const ytdlp = new YTDLP();

export async function loadPlaylist(url: string) {
	try {
		const output = await ytdlp.execPromise(['--flat-playlist', '-J', url]);
		return output;
	} catch (err) {
		console.log(err);
	}
}

export async function loadURL(url: string) {
	console.log(`[server] loading url: ${url}`);
	const t0 = performance.now();
	try {
		const output = await ytdlp.execPromise(['--flat-playlist', '-J', url]);
		const res = JSON.parse(output);
		const t1 = performance.now();
		console.log(`[server] done in ${Number((t1 - t0) / 1000).toFixed(2)}s`);
		return res;
	} catch (err) {
		console.log(err);
		return undefined;
	}
}

export async function loadChannel(url: string) {
	try {
		const output = await ytdlp.execPromise(['--flat-playlist', '--playlist-items', '1', '-J', url]);
		return <YTChannel>JSON.parse(output);
	} catch (err) {
		console.log(err);
		return undefined;
	}
}

export async function loadChannelById(id: string) {
	return loadChannel(`https://youtube.com/channel/${id}`);
}

/* export function loadPlaylist(url: string): Promise<string> {
	return new Promise((resolve, reject) => {
		let text = '';
		let err = '';
		const child = spawn(`yt-dlp`, ['--flat-playlist', '-J', url]);

		child.stdout.on('data', (chunk: string) => {
			text += chunk;
		});

		child.stderr.on('data', (chunk: string) => {
			console.log('' + chunk);
			err += chunk;
		});

		child.on('close', (code: number) => {
			console.log(`yt-dlp exit (${code})`);
			if (code === 0) {
				resolve(text);
			} else {
				reject(err);
			}
		});
	});
} */
