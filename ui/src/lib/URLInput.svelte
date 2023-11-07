<script context="module" lang="ts">
  
</script>

<script lang="ts">
	import { createEventDispatcher } from "svelte";
	import type { YTUrl } from "$lib/model/YTUrl";
	import { parseUrl } from "$lib/utils";

  export let url: YTUrl | undefined = undefined;
  let _url: string;

  const dispatch = createEventDispatcher();

  async function handleUrl() {
    url = parseUrl(_url);
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