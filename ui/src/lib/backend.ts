import type { YTUrl } from './model/YTUrl';

export async function save(ytUrl: YTUrl) {
	const res = await fetch('/api/save', {
		method: 'POST',
		body: JSON.stringify(ytUrl)
	});

	if (res.status !== 200) {
		console.log(await res.json());
		return;
	}

	return await res.json();
}
