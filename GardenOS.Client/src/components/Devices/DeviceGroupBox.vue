<template>

  <div class="grid_item group_wrapper" :style="`grid-row-start: span ${(deviceData.sesnor?.length + 1) / 2}`">
    <h1> {{ deviceData.name }} </h1>
    <hr>
    <span class="group_items">
      <div v-for="sensor in deviceData.sensor" :key="sensor.sensorId" class="group">
        <DeviceBarlabel :sensorData="sensor" />
      </div>
    </span>
  </div>
</template>

<script>
import DeviceBarlabel from "@/components/Devices/DeviceBarlabel.vue"

export default {
  components: {
    DeviceBarlabel
  },
  props: {
    deviceData: {
      type: Object,
      default: () => { }
    },
  },
}
</script>

<style lang="scss">
.group {
  cursor: pointer;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  width: auto;

  &_items {
    display: grid;
    grid-template-columns: 50% 50%;
  }

  &_wrapper {
    background-color: #ffffff;

    h1,
    h2,
    h3,
    h5,
    p {
      text-align: center;
    }
  }
}

@media only screen and (max-width: 360px) {
  .group {
    &_items {
      display: grid;
      grid-template-columns: 100%;
    }
  }
}
</style>