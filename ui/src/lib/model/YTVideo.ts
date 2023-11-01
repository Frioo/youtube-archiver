export interface YTVideo {
	id: string;
	title: string;
	formats: Format[];
	thumbnails: Thumbnail[];
	thumbnail: string;
	description: string;
	uploader: string;
	uploader_id: string;
	uploader_url: string;
	channel_id: string;
	channel_url: string;
	duration: number;
	view_count: number;
	average_rating: any;
	age_limit: number;
	webpage_url: string;
	categories: string[];
	tags: any[];
	playable_in_embed: boolean;
	live_status: string;
	release_timestamp: number;
	_format_sort_fields: string[];
	automatic_captions: Map<string, Captions[]>;
	subtitles: Map<string, Subtitles[]>;
	comment_count: number;
	chapters: any;
	like_count: number;
	channel: string;
	channel_follower_count: number;
	upload_date: string;
	availability: string;
	original_url: string;
	webpage_url_basename: string;
	webpage_url_domain: string;
	extractor: string;
	extractor_key: string;
	playlist: any;
	playlist_index: any;
	display_id: string;
	fulltitle: string;
	duration_string: string;
	release_date: string;
	is_live: boolean;
	was_live: boolean;
	requested_subtitles: any;
	_has_drm: any;
	requested_downloads: RequestedDownload[];
	requested_formats: RequestedFormat[];
	format: string;
	format_id: string;
	ext: string;
	protocol: string;
	language: any;
	format_note: string;
	filesize_approx: number;
	tbr: number;
	width: number;
	height: number;
	resolution: string;
	fps: number;
	dynamic_range: string;
	vcodec: string;
	vbr: number;
	stretched_ratio: any;
	aspect_ratio: number;
	acodec: string;
	abr: number;
	asr: number;
	audio_channels: number;
	epoch: number;
	_type: string;
}

export interface Format {
	format_id: string;
	format_note: string;
	ext: string;
	protocol: string;
	acodec: string;
	vcodec: string;
	url: string;
	width?: number;
	height?: number;
	fps?: number;
	rows?: number;
	columns?: number;
	fragments?: Fragment[];
	resolution: string;
	aspect_ratio?: number;
	//http_headers: HttpHeaders
	audio_ext: string;
	video_ext: string;
	format: string;
	asr?: number;
	filesize?: number;
	source_preference?: number;
	audio_channels?: number;
	quality?: number;
	has_drm?: boolean;
	tbr?: number;
	language: any;
	language_preference?: number;
	preference?: number;
	dynamic_range?: string;
	abr?: number;
	container?: string;
	vbr?: number;
	filesize_approx?: number;
}

export interface Fragment {
	url: string;
	duration?: number;
}

export interface Thumbnail {
	url: string;
	preference: number;
	id: string;
	height?: number;
	width?: number;
	resolution?: string;
}

export interface Captions {
	ext: string;
	url: string;
	name: string;
}

export interface Subtitles {
	url: string;
	video_id: string;
	ext: string;
	protocol: string;
}

export interface RequestedDownload {
	requested_formats: RequestedDownloadFormat[];
	format: string;
	format_id: string;
	ext: string;
	protocol: string;
	format_note: string;
	filesize_approx: number;
	tbr: number;
	width: number;
	height: number;
	resolution: string;
	fps: number;
	dynamic_range: string;
	vcodec: string;
	vbr: number;
	aspect_ratio: number;
	acodec: string;
	abr: number;
	asr: number;
	audio_channels: number;
	epoch: number;
	_filename: string;
	__write_download_archive: boolean;
}

export interface RequestedDownloadFormat {
	asr?: number;
	filesize: number;
	format_id: string;
	format_note: string;
	source_preference: number;
	fps?: number;
	audio_channels?: number;
	height?: number;
	quality: number;
	has_drm: boolean;
	tbr: number;
	url: string;
	width?: number;
	language: any;
	language_preference: number;
	preference: any;
	ext: string;
	vcodec: string;
	acodec: string;
	dynamic_range?: string;
	vbr?: number;
	protocol: string;
	fragments: RequestedDownloadFormatFragment[];
	container: string;
	resolution: string;
	aspect_ratio?: number;
	video_ext: string;
	audio_ext: string;
	format: string;
	abr?: number;
}

export interface RequestedDownloadFormatFragment {
	url: string;
}

export interface RequestedFormat {
	asr?: number;
	filesize: number;
	format_id: string;
	format_note: string;
	source_preference: number;
	fps?: number;
	audio_channels?: number;
	height?: number;
	quality: number;
	has_drm: boolean;
	tbr: number;
	url: string;
	width?: number;
	language: any;
	language_preference: number;
	preference: any;
	ext: string;
	vcodec: string;
	acodec: string;
	dynamic_range?: string;
	vbr?: number;
	protocol: string;
	fragments: RequestedDownloadFormatFragment[];
	container: string;
	resolution: string;
	aspect_ratio?: number;
	video_ext: string;
	audio_ext: string;
	format: string;
	abr?: number;
}
