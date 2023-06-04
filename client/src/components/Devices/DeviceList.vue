<template>
  <div class="device_list" v-if="devices?.devices">
    <DeviceBox v-for="device in devices.devices" :key="device.device_id" :device="device" />
  </div>
</template>

<script>
import DeviceBox from "@/components/Devices/DeviceBox.vue"

export default {
  components: {
    DeviceBox
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