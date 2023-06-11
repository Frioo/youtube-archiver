<script lang="ts">
	import BigVideo from "$lib/BigVideo.svelte";
import Playlist from "$lib/Playlist.svelte";
  import Video from "$lib/Video.svelte";
  import type { PageData } from "./$types";

  export let data: PageData;
  let { videoId, playlistId, json } = data;
  $: ({ videoId, playlistId, json } = data);

  $: console.log(json);
</script>

{#if playlistId}
  <Playlist {json} />
{:else if videoId}
  <div class="max-w-5xl">
    <BigVideo {json} />
  </div>
{/if}