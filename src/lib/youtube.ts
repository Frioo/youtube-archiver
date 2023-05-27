import { exec, spawn } from 'node:child_process';

export function loadPlaylist(url: string): Promise<string> {
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
}
