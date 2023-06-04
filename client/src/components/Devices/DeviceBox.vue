<template>
  <div class="device_wrapper" @click="this.$router.push(`device/${device.device_id}`)">
    <p> {{ device.name }} </p>
    <p> {{ device.value }} </p>
    <p> {{ formatTime(device.date) }} </p>
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
    background-color: whitesmoke;
    margin: 10px;
    max-width: 250px;
    border-radius: 10px;
    box-shadow: 0 0 5px 5px #5f5e5e21;
    padding: 5px;

    p {
      text-align: center;
    }
  }
}
</style>