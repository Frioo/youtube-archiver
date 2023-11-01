export interface YTChannel {
	id: string;
	channel_id: string;
	channel_url: string;
	uploader_id: string;
	uploader_url: string;
	title: string;
	description: string;
	thumbnails: Thumbnail[];
}

export interface Thumbnail {
	url: string;
	height: number;
	width: number;
	preference: number;
	id: number;
	resolution: string;
}
