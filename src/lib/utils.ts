export function defaultThumbnail(thumbnails: any) {
	return thumbnails[thumbnails.length - 1 || 0].url;
}
