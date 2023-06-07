<template>
  <div>
    <div v-if="currentDevice">
      <h1>Device: {{ currentDevice.name }}</h1>
      <h2>Current Value: {{ currentDevice.value }}</h2>
      <h3>ID: {{ currentDevice.device_id }}</h3>
      <div>
        <p>Start</p>
        <input type="datetime-local" @change="setTimeFrameStart"
          :value="`${new Date().toISOString().replace(/T[0-9:.Z]*/, 'T00:00:00')}`">
      </div>
      <div>
        <p>End</p>
        <input type="datetime-local" @change="setTimeFrameEnd"
          :value="`${new Date().toISOString().replace(/T[0-9:.Z]*/, 'T23:59:59')}`">
      </div>
      <button @click="fetchData()">Fetch Timeframe</button>
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
import 'chartjs-adapter-moment';

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
      timeframe: {
        start: new Date().toISOString(),
        end: new Date().toISOString()
      },
      chartRawData: null,
      data: {
        labels: [],
        datasets: []
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          title: {
            text: 'Chart.js Time Scale',
            display: true
          }
        },
        scales: {
          x: {
            type: "time",
            time: {
              minUnit: "minute"
            },
            ticks: {
              autoSkip: true,
              maxTicksLimit: 20
            }
          }
        },
      }
    }
  },
  methods: {
    setTimeFrameStart(e) {
      this.timeframe.start = new Date(e.target.value).toISOString()
    },
    setTimeFrameEnd(e) {
      this.timeframe.end = new Date(e.target.value).toISOString()
    },
    async fetchData() {
      try {
        const initData = await fetch(`${process.env.VUE_APP_PI_HOST}user/${localStorage.getItem("id")}/datalog`, {
          method: "POST",
          body: JSON.stringify(this.timeframe),
          headers: {
            'Authorization': `Bearer ${localStorage.getItem("apiToken")}`,
            'Accept': 'application/json',
            'Content-Type': 'application/json'
          },
        });
        const chartData = await initData.json()

        // Refine Chart data
        let labels = []
        let datasets = []

        chartData.devices.forEach(device => {
          if (!labels.includes(device.date))
            labels.push(device.date)

          if (!datasets.some(d => d.label == device.name)) {
            const devicesInLabel = chartData.devices.filter(d => d.name == device.name)
            const dataForDevice = devicesInLabel.map(d => { return { y: d.value, x: new Date(d.date) } })
            datasets.push({
              label: device.name,
              data: dataForDevice,
              backgroundColor: "#" + Math.floor(Math.random() * 16777215).toString(16),
            })
          }
        });
        this.data = { labels: labels, datasets: datasets }
      } catch (error) {
        console.log(error)
      }
    }
  },
  async mounted() {
    this.$store.commit("setGardenMeta", await fetchGardenMeta())
    this.$store.commit("setDeviceData", await fetchDevices())
    this.fetchData()
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