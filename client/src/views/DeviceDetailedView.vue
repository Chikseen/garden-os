<template>
  <div>
    <div v-if="currentDevice">
      <h1>Device: {{ currentDevice.name }}</h1>
      <h2>Current Value: {{ currentDevice.value }}</h2>
      <h3>ID: {{ currentDevice.device_id }}</h3>
      <div class="lineChart_wrapper">
        <Line :data="data" :options="options" />
      </div>
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
  Legend,
  TimeScale
} from 'chart.js'
import { Line } from 'vue-chartjs'

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  TimeScale
)

import { fetchGardenMeta, fetchDevices } from "@/apiService.js"
import { formatToDateTime } from "@/dates.js"
import { mapState } from "vuex";

export default {
  components: {
    Line
  },
  data() {
    return {
      chartRawData: null,
      data: {
        labels: [],
        datasets: []
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          x: {
            time: {
              unit: 'day',
              parser: 'dd.MM.yyyy',
              stepSize: 4
            }
          }
        }
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
        body: JSON.stringify({ start: "2023-05-24T00:00:00", end: "2023-05-24T23:59:59" }),
        headers: {
          'Authorization': `Bearer ${localStorage.getItem("apiToken")}`,
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
      });
      const chartData = await initData.json()
      console.log(chartData)

      // Refine Chart data
      let labels = []
      let datasets = []

      chartData.devices.forEach(device => {
        if (!labels.includes(device.date))
          labels.push(device.date)

        if (!datasets.some(d => d.label == device.name)) {
          const devicesInLabel = chartData.devices.filter(d => d.name == device.name)
          const dataForDevice = devicesInLabel.map(d => d.value)
          // const dataForDevice = devicesInLabel.map(d => { return { y: d.value, t: d.date } })
          datasets.push({
            label: device.name,
            data: dataForDevice,
            backgroundColor: "#" + Math.floor(Math.random() * 16777215).toString(16),
          })
        }
      });


      this.data = { labels: labels, datasets: datasets }
      console.log("data", this.data)

    } catch (error) {

      console.log(error)
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