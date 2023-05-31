import { exec, spawn } from 'node:child_process';
import YTDLP from 'yt-dlp-wrap';

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
	try {
		const output = await ytdlp.execPromise(['--flat-playlist', '-J', url]);
		return JSON.parse(output);
	} catch (err) {
		console.log(err);
	}
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
