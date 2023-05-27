<script lang="ts">
  export let json: any;
  let thumbnail = json?.thumbnails?.[json?.thumbnails.length - 1 || 0];

  function defaultThumbnail(thumbnails: any) {
    return thumbnails[thumbnails.length - 1 || 0].url;
  }
</script>

<h2 class="h2 mb-6">{json.title}</h2>
<div class="entries">
  {#each json.entries as { title, channel, thumbnails }}
    <div class="entry">
      <div class="thumbnail" style="background-image: url('{defaultThumbnail(thumbnails)}')"></div>
      <div class="flex flex-col ml-4">  
        <div class="">{title}</div>
        <div class="text-gray-800">{channel || ""}</div>
      </div>
    </div>
  {/each}
</div>

<style>
  .entries {
    @apply flex flex-col gap-2;
  }
  
  .entry {
    @apply flex;
  }

  .entry .thumbnail {
    @apply w-24 aspect-video bg-center bg-cover rounded-md;
  }
  
  .entry img {
    @apply w-full;
  }
</style>