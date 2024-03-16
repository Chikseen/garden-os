<template>
  <!--<DeviceGroupBox v-for="group in groupList" :key="group" :devices="devices.filter(d => d.groupId == group)"
    :group=group style="grid-column-start: span 2" />-->
  <div v-for="sensor in sensors" :key="sensor.deviceId">
    <DeviceBox :sensor="sensor" />
  </div>
</template>

<script>
import DeviceBox from "@/components/Devices/DeviceBox.vue"
import DeviceGroupBox from "@/components/Devices/DeviceGroupBox.vue"

import { fetchDeviceSensorLatestValues } from "@/services/apiService.js"

export default {
  components: {
    DeviceBox,
    DeviceGroupBox
  },
  data() {
    return {
      sensors: []
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
      deviceData.sensor.forEach(sensor => {
        this.sensors.push(sensor)
      });
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