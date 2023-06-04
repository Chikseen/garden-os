<template>
  <div>
    <h1>Device: {{ currentDevice.name }}</h1>
    <h2>Current Value: {{ currentDevice.value }}</h2>
    <h3>ID: {{ currentDevice.device_id }}</h3>
    <div class="lineChart_wrapper">
      <Line :data="data" :options="options" />
    </div>
  </div>
</template>

<script>
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
} from 'chart.js'
import { Line } from 'vue-chartjs'

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
)

import { fetchGardenMeta, fetchDevices } from "@/apiService.js"

import { mapState } from "vuex";

export default {
  components: {
    Line
  },
  data() {
    return {
      chartRawData: null,
      data: {
        labels: ['February', 'March', 'April', 'May', 'June', 'July'],
        datasets: [
          {
            label: 'Data One',
            backgroundColor: '#f87979',
            data: [40, 39, 80, 40]
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false
      }
    }
  },
  async mounted() {
    this.$store.commit("setGardenMeta", await fetchGardenMeta())
    this.$store.commit("setDeviceData", await fetchDevices())

    try {
      // It is working just a BE error

      const initData = await fetch(`${process.env.VUE_APP_PI_HOST}user/${localStorage.getItem("id")}/datalog`, {
        method: "POST",
        body: JSON.stringify({ start: "2019-01-06T17:16:40", end: "2019-01-06T17:16:40" }),
        headers: {
          'Authorization': `Bearer ${localStorage.getItem("apiToken")}`,
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
      });
      const res = await initData.json()
      console.log(res)

      this.chartRawData = JSON.parse(res.json);
    } catch (error) {

    }
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

<style lang="scss">
.lineChart {
  &_wrapper {
    max-height: 500px;
  }
}
</style>