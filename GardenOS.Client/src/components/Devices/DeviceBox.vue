<template>
  <DeviceBarlabel :device=device class="grid_item" />
</template>

<script>
import DeviceBarlabel from "@/components/Devices/DeviceBarlabel.vue"

export default {
  components: {
    DeviceBarlabel
  },
  props: {
    device: {
      type: Object,
      default: () => { }
    }
  }
}
</script>