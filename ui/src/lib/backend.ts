export async function save(url: string) {
	const res = await fetch('/api/save', {
		method: 'POST',
		body: JSON.stringify({ url })
	});

	if (res.status !== 200) {
		console.log(await res.json());
		return;
	}
}
