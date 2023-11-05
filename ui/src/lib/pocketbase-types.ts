/**
* This file was @generated using pocketbase-typegen
*/

import type PocketBase from 'pocketbase'
import type { RecordService } from 'pocketbase'

export enum Collections {
	Channels = "channels",
	Formats = "formats",
	Users = "users",
	Videos = "videos",
}

// Alias types for improved usability
export type IsoDateString = string
export type RecordIdString = string
export type HTMLString = string

// System fields
export type BaseSystemFields<T = never> = {
	id: RecordIdString
	created: IsoDateString
	updated: IsoDateString
	collectionId: string
	collectionName: Collections
	expand?: T
}

export type AuthSystemFields<T = never> = {
	email: string
	emailVisibility: boolean
	username: string
	verified: boolean
} & BaseSystemFields<T>

// Record types for each collection

export type ChannelsRecord = {
	avatar?: string
	channel_id?: string
	channel_url?: string
	handle?: string
	handle_url?: string
	name?: string
}

export enum FormatsTypeOptions {
	"video" = "video",
	"audio" = "audio",
	"muxed" = "muxed",
	"storyboard" = "storyboard",
}
export type FormatsRecord = {
	abr?: number
	acodec?: string
	asr?: string
	ext?: string
	filename?: string
	filesize?: number
	fps?: number
	height?: number
	name?: string
	note?: string
	type: FormatsTypeOptions
	vbr?: number
	vcodec?: string
	video: RecordIdString
	width?: number
}

export type UsersRecord = {
	avatar?: string
	name?: string
}

export type VideosRecord = {
	channel?: RecordIdString
	description?: string
	thumbnail?: string
	title?: string
	video_id: string
}

// Response types include system fields and match responses from the PocketBase API
export type ChannelsResponse<Texpand = unknown> = Required<ChannelsRecord> & BaseSystemFields<Texpand>
export type FormatsResponse<Texpand = unknown> = Required<FormatsRecord> & BaseSystemFields<Texpand>
export type UsersResponse<Texpand = unknown> = Required<UsersRecord> & AuthSystemFields<Texpand>
export type VideosResponse<Texpand = unknown> = Required<VideosRecord> & BaseSystemFields<Texpand>

// Types containing all Records and Responses, useful for creating typing helper functions

export type CollectionRecords = {
	channels: ChannelsRecord
	formats: FormatsRecord
	users: UsersRecord
	videos: VideosRecord
}

export type CollectionResponses = {
	channels: ChannelsResponse
	formats: FormatsResponse
	users: UsersResponse
	videos: VideosResponse
}

// Type for usage with type asserted PocketBase instance
// https://github.com/pocketbase/js-sdk#specify-typescript-definitions

export type TypedPocketBase = PocketBase & {
	collection(idOrName: 'channels'): RecordService<ChannelsResponse>
	collection(idOrName: 'formats'): RecordService<FormatsResponse>
	collection(idOrName: 'users'): RecordService<UsersResponse>
	collection(idOrName: 'videos'): RecordService<VideosResponse>
}
