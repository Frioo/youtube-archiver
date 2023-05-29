<script lang="ts">
	import Playlist from "$lib/Playlist.svelte";
  import { toastStore, ProgressBar } from "@skeletonlabs/skeleton";

  let playlists: string[] = [];
  let url: string = "";
  let loading = false;
  let json: any = undefined;
  let error: any = undefined;

  async function handleAdd() {
    loading = true;
    const pa = new URLSearchParams()
    const resp = await fetch(`/api/playlist?url=${encodeURIComponent(url)}`);
    const respJson = await resp.json();

    if (resp.status !== 200) {
      error = respJson;
      toastStore.trigger({ 
        message: error.message,
        background: 'variant-filled-warning',
      });
    } else {
      json = respJson;
      playlists = [...playlists, url]
      url = "";
    }
    loading = false;
  }

  async function handleKeyDown(e: KeyboardEvent) {
    if (e.key === 'Enter') {
      await handleAdd();
    }
  }
</script>

<div class="p-4 flex gap-2">
  <div class="input-group input-group-divider grid-cols-[1fr_auto]">
    <!-- <div class="input-group-shim">(segment)</div> -->
    <input
      bind:value={url}
      on:keydown={handleKeyDown}
      class="input"
      type="url"
      name="url"
      placeholder="Playlist URL">
    <button class="variant-filled" on:click={handleAdd}>
      Fetch
    </button>
  </div>
</div>
<div class="p-4">
  {#if loading}
    <div
      class="flex flex-col w-48 mx-auto my-6 gap-4 items-center">
      Loading playlist
      <ProgressBar />
    </div>
  {/if}
  {#if json}
    <Playlist {json} />
  {/if}
</div>