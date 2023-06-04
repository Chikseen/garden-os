<template>
  <div>
    <h1>Device: {{ currentDevice.name }}</h1>
    <h2>Current Value: {{ currentDevice.value }}</h2>
    <h3>ID: {{ currentDevice.device_id }}</h3>
  </div>
</template>

<script>
import { fetchGardenMeta, fetchDevices } from "@/apiService.js"

import { mapState } from "vuex";

export default {
  async mounted() {
    this.$store.commit("setGardenMeta", await fetchGardenMeta())
    this.$store.commit("setDeviceData", await fetchDevices())
  },
  computed: {
    currentDevice() {
      const currentD = this.deviceData?.devices?.find(d => d.device_id == this.$route.params.id)
      return currentD
    },
    ...mapState({
      gardenMeta: (state) => state.gardenMeta,
      deviceData: (state) => state.deviceData,
    }),
  },
}
</script>