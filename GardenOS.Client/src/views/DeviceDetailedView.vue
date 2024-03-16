<template>
  <div v-if="data" class="detailed_wrapper">
    <DeviceBox :sensor="getDeviceById()" />
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

import { toUTCISOString } from "@/dates.js"
import { mapState, mapActions } from "vuex";

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
      this.timeframe.start = toUTCISOString(new Date(e.target.value))
    },
    setTimeFrameEnd(e) {
      this.timeframe.end = toUTCISOString(new Date(e.target.value))
    },
    getDeviceById() {
      return this.deviceData.find(d => d.id == this.$route.params.id)
    },
    async fetchData() {
      this.isDataLoading = true
      const response = await fetch(`${process.env.VUE_APP_PI_HOST}devices/${localStorage.getItem("selectedGarden")}/${this.$route.params.id}`, {
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
      let dataSetsValues = {}

      chartData.forEach(device => {
        labels.push(device.date)
        device.sensor.forEach(sensor => {
          if (!dataSetsValues.hasOwnProperty(sensor.name))
            dataSetsValues[sensor.name] = []
          dataSetsValues[sensor.name].push(sensor.correctedValue)
        })
      });

      console.log(dataSetsValues)

      let dataSets = []

      Object.keys(dataSetsValues).forEach(dataset => {
        dataSets.push({
          label: dataset,
          data: dataSetsValues[dataset],
          //  hidden: ishidden,
          backgroundColor: "#" + Math.floor(Math.random() * 16777215).toString(16),
        })
      });

      this.data = { labels: labels, datasets: dataSets }
      console.log(this.data)
      this.isDataLoading = false
    },
    ...mapActions({
      fetchDevices: 'fetchDevices',
      fetchGardenMeta: 'fetchGardenMeta'
    })
  },
  computed: {
    currentDevice() {
      console.log(this.deviceData)
      const currentD = this.deviceData?.find(d => d.id == this.$route.params.id)
      return currentD
    },
    ...mapState({
      gardenMeta: (state) => state.gardenMeta,
      deviceData: (state) => state.deviceData,
    }),
  },
  created() {
    let dayStart = new Date(new Date().setDate(new Date().getDate() - 7)).setUTCHours(0, 0, 0, 0)
    let dayEnd = new Date().setUTCHours(23, 59, 0, 0)

    // At this point i hink JS is high
    this.timeframe.start = new Date(dayStart).toISOString()
    this.timeframe.end = new Date(dayEnd).toISOString()
  },
  async mounted() {
    this.fetchGardenMeta()
    this.fetchDevices()
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
</style>services/apiService.js