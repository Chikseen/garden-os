<template>
  <div v-if="currentDevice" class="detailed_wrapper">
    <DeviceBox :device="getDeviceById()" />
    <div class="detailed_timeframe_wrapper">
      <div class="detailed_timeframe">
        <div>
          <p>Start</p>
          <input type="datetime-local" @change="setTimeFrameStart" :value="timeframe.start.replace(/Z|\+[0-9:]*/, '')">
        </div>
        <div>
          <p>End</p>
          <input type="datetime-local" @change="setTimeFrameEnd" :value="timeframe.end.replace(/Z|\+[0-9:]*/, '')">
        </div>
      </div>
      <button @click="fetchData()">Fetch Timeframe</button>
    </div>
  </div>

  <LC v-if="isDataLoading" />
  <div v-else class="lineChart_wrapper">
    <Line :data="data" :options="options" />
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
import DeviceBox from "@/components/Devices/DeviceBox.vue"
import LC from "@/components/ui/LoadingComponent.vue"

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
import { toUTCISOString } from "@/dates.js"
import { mapState } from "vuex";

export default {
  components: {
    Line,
    DeviceBox,
    LC,
  },
  data() {
    return {
      isDataLoading: true,
      timeframe: {
        start: null,
        end: null
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
            display: false
          }
        },
        scales: {
          x: {
            type: "time",
            time: {
              minUnit: "hour"
            },
            ticks: {
              autoSkip: true,
              maxTicksLimit: 50
            }
          },
          y: {
            // beginAtZero: true,
            //max: 100
          }
        },
        elements: {
          line: {
            tension: 0.3
          }
        }
      }
    }
  },
  methods: {
    setTimeFrameStart(e) {
      console.log("changed")
      this.timeframe.start = toUTCISOString(new Date(e.target.value))
    },
    setTimeFrameEnd(e) {
      this.timeframe.end = toUTCISOString(new Date(e.target.value))
    },
    getDeviceById() {
      return this.deviceData.devices.find(d => d.device_id == this.$route.params.id)
    },
    async fetchData() {
      this.isDataLoading = true
      const response = await fetch(`${process.env.VUE_APP_PI_HOST}user/detailed/${localStorage.getItem("selectedGarden")}`, {
        method: "POST",
        body: JSON.stringify(this.timeframe),
        headers: {
          'Authorization': `Bearer ${localStorage.getItem("accessToken")}`,
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
      });

      const chartData = await response.json()

      // Refine Chart data
      let labels = []
      let datasets = []
      const routeDevice = this.deviceData.devices.find(d => d.device_id == this.$route.params.id)

      chartData.devices.forEach(device => {
        if (!labels.includes(device.date)) {
          labels.push(device.date)
        }
        if (!datasets.some(d => d.label == device.name)) {
          const ishidden = routeDevice.group_id != "" ? routeDevice.group_id != device.group_id : routeDevice.device_id != device.device_id
          const devicesInLabel = chartData.devices.filter(d => d.name == device.name)
          const dataForDevice = devicesInLabel.map(d => { return { y: d.corrected_value, x: new Date(d.date) } })
          datasets.push({
            label: device.name,
            data: dataForDevice,
            hidden: ishidden,
            backgroundColor: "#" + Math.floor(Math.random() * 16777215).toString(16),
          })
        }
      });
      this.data = { labels: labels, datasets: datasets }
      this.isDataLoading = false
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
  created() {
    let dayStart = new Date().setUTCHours(0, 0, 0, 0)
    let dayEnd = new Date().setUTCHours(23, 59, 0, 0)

    // At this point i hink JS is high
    this.timeframe.start = new Date(dayStart).toISOString()
    this.timeframe.end = new Date(dayEnd).toISOString()
  },
  async mounted() {
    this.$store.commit("setGardenList", await fetchGardenMeta())
    this.$store.commit("setDeviceData", await fetchDevices(localStorage.getItem("selectedGarden")))
    this.fetchData()
  },
}
</script>

<style lang="scss">
.lineChart {
  &_wrapper {
    height: 350px;
  }
}

.detailed {
  &_wrapper {
    max-width: 750px;
    width: calc(100% - 20px);
    margin: 0 auto;
    padding: 10px;
  }

  &_timeframe {
    display: flex;
    justify-content: center;
    gap: 15px;

    &_wrapper {
      display: flex;
      flex-direction: column;
      gap: 15px;
      padding: 10px;
    }
  }
}

.tomorrow {
  max-width: 750px;
}

/*
@media screen and (orientation: landscape) {
  .detailed {
    &_wrapper {
      display: none;
    }
  }

  .lineChart {
    &_wrapper {
      height: 100vh;
      width: 100vw;
    }
  }
}*/
</style>