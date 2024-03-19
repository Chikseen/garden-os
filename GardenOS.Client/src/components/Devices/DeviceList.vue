<template>
  <DeviceGroupBox v-for="device in multiDeviceData" :key="device.deviceId" :deviceData="device" style="grid-column-start: span 2" />
  <div v-for="sensor in singleDeviceData" :key="sensor.deviceId">
    <DeviceBarlabel :sensorData="sensor" class="grid_item" />
  </div>
</template>

<script>
import DeviceBarlabel from "@/components/Devices/DeviceBarlabel.vue"
import DeviceGroupBox from "@/components/Devices/DeviceGroupBox.vue"

import { fetchDeviceSensorLatestValues } from "@/services/apiService.js"

export default {
  components: {
    DeviceBarlabel,
    DeviceGroupBox
  },
  data() {
    return {
      singleDeviceData: [],
      multiDeviceData: [],
    }
  },
  props: {
    devices: {
      type: Object,
      default: () => { }
    }
  },
  async mounted() {
    await this.devices.forEach(async device => {
      let deviceData = await fetchDeviceSensorLatestValues(localStorage.getItem("selectedGarden"), device.id)
      if (deviceData.sensor?.length > 1)
        this.multiDeviceData.push(deviceData)
      else
        this.singleDeviceData.push(deviceData)
    });
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