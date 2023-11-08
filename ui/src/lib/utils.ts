export function defaultThumbnail(thumbnails: any) {
	return thumbnails[thumbnails.length - 1 || 0].url;
}

export function measurePerformance(t0: number): string {
	const perf = performance.now() - t0;
	if (perf > 1000) {
		return `${Number(perf / 1000).toFixed(2)}s`;
	} else {
		return `${perf}ms`;
	}
}
