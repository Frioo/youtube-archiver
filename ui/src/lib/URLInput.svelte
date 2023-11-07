<script context="module" lang="ts">
  export interface YTUrl {
    url: string
    videoId?: string
    playlistId?: string
    index?: number
    t?: number
  }
</script>

<script lang="ts">
	import { createEventDispatcher } from "svelte";

  export let url: YTUrl | undefined = undefined;
  let _url: string;

  const dispatch = createEventDispatcher();

  async function handleUrl() {
    const parsed = new URL(_url);
    if (!parsed) {
      // invalid url
    }

    const { hostname, pathname, searchParams } = parsed;
    const res: YTUrl = { url: _url };

    if (hostname.endsWith('youtu.be')) {
      // youtu.be/video_id => video_id
      res.videoId = pathname.slice(1);
    } else if (hostname.split('.').includes('youtube')) {
      res.videoId = searchParams.get('v') || undefined;
    }
    
    res.playlistId = searchParams.get('list') || undefined;
    res.index = Number.parseInt(searchParams.get('index')!) || undefined;
    res.t = Number.parseInt(searchParams.get('t')!) || undefined;

    url = res;
    dispatch('url', url);
  }
</script>

<div class="input-group input-group-divider grid-cols-[1fr_auto]">
  <input
    bind:value={_url}
    on:keydown={(e) => e.key === 'Enter' && handleUrl()}
    class="input"
    type="url"
    name="url"
    placeholder="YouTube URL">
  <button class="variant-filled" on:click={() => handleUrl()}>
    Load
  </button>
</div>