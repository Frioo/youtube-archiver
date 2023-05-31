<script lang="ts">
  import { defaultThumbnail } from "$lib/utils";
  import dayjs from "dayjs";
  import dayjsDuration from "dayjs/plugin/duration";
  import dayjsRelativeTime from "dayjs/plugin/relativeTime";
  dayjs.extend(dayjsDuration);
  dayjs.extend(dayjsRelativeTime);

  export let json: any;
  let { title, description, thumbnails, view_count, duration, channel, channel_url, } = json;

  function durationText(seconds: number) {
    const dur = dayjs.duration(seconds, 's');
    if (seconds < 60) {
      return `${seconds}s`
    } else if (seconds < 60 * 60) {
      return dur.format('mm:ss');
    } else {
      return dur.format('HH:mm:ss');
    }
  }
</script>

<div class="card m-4 w-min overflow-hidden">
  <div class="relative isolate">
    <img class="max-w-sm" src={defaultThumbnail(thumbnails)} alt="">
    <span class="badge variant-filled absolute bottom-1 right-1">{durationText(duration)}</span>
  </div>
  <div class="p-4">
    <h2 class="h2 text-xl font-medium">{title}</h2>
    <div class="flex mt-2 mb-4 gap-2">
      <a href={channel_url} target="_blank" class="badge variant-soft transition-colors duration-100 hover:variant-filled-primary">{channel}</a>
      <span class="badge variant-soft">{Intl.NumberFormat().format(view_count)} views</span>
    </div>
    <p class="p whitespace-pre-line">
      {description}
    </p>
  </div>
</div>