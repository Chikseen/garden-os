<template>
  <DeviceGroupBox v-for="group in groupList" :key="group" :devices="devices.filter(d => d.group_id == group)"
    :group=group style="grid-column-start: span 2"/>
  <DeviceBox v-for="device in devices.filter(d => d.group_id.length < 1).sort((a, b) => a.sort_order - b.sort_order)"
    :key="device.device_id" :device="device" />
</template>

<script>
import DeviceBox from "@/components/Devices/DeviceBox.vue"
import DeviceGroupBox from "@/components/Devices/DeviceGroupBox.vue"

export default {
  components: {
    DeviceBox,
    DeviceGroupBox
  },
  props: {
    devices: {
      type: Object,
      default: () => { }
    }
  },
  computed: {
    groupList() {
      let groupList = []
      this.devices.forEach(device => {
        if (device.group_id && groupList.indexOf(device.group_id) < 0)
          groupList.push(device.group_id)
      });
      return groupList
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