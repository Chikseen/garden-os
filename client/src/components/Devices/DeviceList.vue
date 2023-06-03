<template>
  <div class="device_list" v-if="devices?.devices">
    <DeviceDetailed v-for="device in devices.devices" :key="device.device_id" :device="device" />
  </div>
</template>

<script>
import DeviceDetailed from "@/components/Devices/DeviceDetailed.vue"

export default {
  components: {
    DeviceDetailed
  },
  props: {
    devices: {
      type: Object,
      default: () => { }
    }
  }
}
</script>

<style lang="scss">
.device {
  &_list {
    display: flex;
    flex-wrap: wrap;
  }
}
</style>