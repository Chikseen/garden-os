<template>
  <div class="grid_item device_wrapper" @click="this.$router.push(`device/${device.device_id}`)">
    <h1> {{ device.name }} </h1>
    <h2> {{ device.corrected_value }} %
      <h6> {{ device.value }} %</h6>
    </h2>
    <h5> {{ formatTime(device.date) }} </h5>
  </div>
</template>

<script>
import { formatToDateTime } from "@/dates.js";

export default {
  props: {
    device: {
      type: Object,
      default: () => { }
    }
  },
  methods: {
    formatTime(d) {
      return formatToDateTime(d)
    }
  },
}
</script>

<style lang="scss">
.device {
  &_wrapper {
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    background-color: #ffffff;
    width: auto;
    padding: 10px 0;

    h1,
    h2,
    h5,
    p {
      text-align: center;
    }
  }
}
</style>