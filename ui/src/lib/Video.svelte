<script lang="ts">
	import { defaultThumbnail } from "$lib/utils";
  import { InfoIcon } from "svelte-feather-icons";
	import type { VideosResponse } from "./pocketbase-types";
	import { PUBLIC_POCKETBASE_URL } from "$env/static/public";

  export let video: VideosResponse;
  $: ({ title, thumbnail, video_id, collectionId, id, expand } = video)
</script>

<div class="flex gap-x-2">
  <div class="rounded-sm aspect-[16/9] bg-contain bg-center w-48 h-fit flex-shrink-0 flex-grow-0" style="background-image: url('{PUBLIC_POCKETBASE_URL}/api/files/{collectionId}/{id}/{thumbnail}')">
  </div>
  <div class="">
    <span class="inline-block font-medium leading-snug mb-1.5">{title}</span>
    <a href={expand?.channel?.handle_url} target="_blank" class="block text-sm hover:text-blue-700">{expand?.channel?.name}</a>
  </div>
</div>

<style lang="postcss">

</style>