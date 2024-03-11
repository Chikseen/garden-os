<template>
  <DeviceBarlabel :sensor="sensor" class="grid_item" />
</template>

<script>
import DeviceBarlabel from "@/components/Devices/DeviceBarlabel.vue"

export default {
  components: {
    DeviceBarlabel
  },
  props: {
    sensor: {
      type: Object,
      default: () => { }
    }
  }
}
</script>