<script context="module" lang="ts">
  
</script>

<script lang="ts">
	import { createEventDispatcher } from "svelte";
	import type { YTUrl } from "$lib/model/YTUrl";
	import { parseUrl } from "$lib/model/YTUrl";
  //import { TextField } from "m3-svelte";
  import TextField from "$lib/components/TextField.svelte";
  import Button from "$lib/components/Button.svelte";
  import LinkIcon from "@iconify-icons/material-symbols/link-rounded";
	import { fade } from "svelte/transition";
	import { goto } from "$app/navigation";
	
  const dispatch = createEventDispatcher();

  export let url: YTUrl | undefined = undefined;
  let _url: string | undefined = undefined;


  $: validateUrl(_url);

  async function validateUrl(value: string | undefined) {
    if (!value) return;
    const res = parseUrl(value);
    if (!res) return;
    url = res;
    dispatch('url', res);
  }

  function handleSubmit() {
    if (!url) return;
    dispatch('submit', url);
  }
</script>

<!-- on:keydown={(e) => e.key === 'Enter' && handleUrl()} -->
<div class="flex w-full items-center relative">  
  <TextField
    class="w-full"
    bind:value={_url}
    on:keydown={(e) => e.key === 'enter' && handleSubmit()}
    name="YouTube URL"
    leadingIcon={LinkIcon}
    error={!!_url && url === undefined} />

  {#if url}
    <div class="!absolute right-2" in:fade={{ duration: 150 }}>
      <Button type="filled" on:click={handleSubmit}>Load</Button>
    </div>
  {/if}
</div>

<!-- <div class="input-group input-group-divider grid-cols-[1fr_auto]">
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
</div> -->