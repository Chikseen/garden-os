<template>
  <div v-if="data" class="detailed_wrapper">
    <DeviceBarlabel :sensorData="getDeviceById()" class="grid_item" />
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
  <div class="detailed_table">
    <table>
      <tr>
        <th>Date</th>
        <th v-for="sensors in chartData.sensors" :key="sensors.name">{{ sensors.name }}</th>
        <th v-if="getDeviceById()?.isManual">Remove Entry</th>
      </tr>
      <tr v-for="(yValue, i) in chartData.yAxis" :key="i">
        <td>{{ convertDateTimeToString(chartData.xAxis[i]) }}</td>
        <td v-for="(value, j) in yValue" :key="j + value">{{ value?.toFixed(1) }}</td>
        <td v-if="getDeviceById()?.isManual">
          <ClickAndHoldButton style="width: 50px;" @trigger="removeEntry(i)" />
        </td>
      </tr>
    </table>
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
import DeviceBarlabel from "@/components/Devices/DeviceBarlabel.vue"
import ClickAndHoldButton from "@/components/ClickAndHoldButton.vue"
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

import { formatToDateTime, toUTCISOString } from "@/services/dates.js"
import { getColorByDevice } from "@/services/staticDeviceData.js"
import { mapState, mapActions } from "vuex";

export default {
  components: {
    Line,
    DeviceBarlabel,
    LC,
    ClickAndHoldButton,
  },
  data() {
    return {
      isDataLoading: true,
      chartData: {},
      timeframe: {
        start: null,
        end: null
      },
      data: {
        labels: [],
        datasets: []
      },
      options: {
        backgroundColor: '#ffffff',
        responsive: true,
        pointHitRadius: 99,
        pointRadius: 0,
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
              minUnit: "day"
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
    convertDateTimeToString(date) {
      return formatToDateTime(date)
    },
    async removeEntry(i) {
      const entryId = this.chartData.entryIds[i]
      await fetch(`${process.env.VUE_APP_PI_HOST}devices/${localStorage.getItem("selectedGarden")}/${entryId}`, {
        method: "DELETE",
        body: JSON.stringify(this.timeframe),
        headers: {
          'Authorization': `Bearer ${localStorage.getItem("accessToken")}`,
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
      });
      this.fetchData()
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

      this.chartData = await response.json()

      let dataSets = []
      this.chartData.sensors.forEach((sensor, i) => {

        const color = getColorByDevice(sensor)

        dataSets.push({
          label: sensor.name,
          data: this.chartData.yAxis.map(value => value[i]),
          borderColor: color,
          backgroundColor: color,
        })
      });

      this.data = { labels: this.chartData.xAxis, datasets: dataSets }
      this.isDataLoading = false
    },
    ...mapActions({
      fetchDevices: 'fetchDevices',
      fetchGardenMeta: 'fetchGardenMeta'
    })
  },
  computed: {
    currentDevice() {
      const currentD = this.deviceData?.find(d => d.id == this.$route.params.id)
      return currentD
    },
    ...mapState({
      gardenMeta: (state) => state.gardenMeta,
      deviceData: (state) => state.deviceData,
    }),
  },
  created() {
    const isDeviceManual = this.deviceData.find(d => d.id == this.$route.params.id)?.isManual

    let dayStart;
    if (isDeviceManual)
      dayStart = new Date(new Date().setDate(new Date().getDate() - 30)).setUTCHours(0, 0, 0, 0)
    else
      dayStart = new Date(new Date().setDate(new Date().getDate() - 7)).setUTCHours(0, 0, 0, 0)
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

  &_table {
    max-height: 500px;
    overflow-y: scroll;
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
</style>