<template>
  <DeviceGroupBox v-for="device in multiDeviceData" :key="device.deviceId" :deviceData="device"
    style="grid-column-start: span 2" />
  <div v-for="device in singleDeviceData" :key="device.deviceId">
    <DeviceBarlabel :sensorData="device.sensor[0]" class="grid_item" />
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
    console.log("a", this.devices)
    await this.devices.forEach(async device => {
      let deviceData = await fetchDeviceSensorLatestValues(localStorage.getItem("selectedGarden"), device.id)
      
      device.sensor.forEach(sensor => {
        const foundSensor = deviceData.sensor.find(s => s.sensorId == sensor.sensorId)
        if (!foundSensor)
          deviceData.sensor.push(sensor)
      })

      if (device.sensor?.length > 1)
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