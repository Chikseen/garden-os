<template>
  <div class="grid_item group_wrapper" :style="`grid-row-start: span ${(devices.length + 1) / 2}`">
    <h1> {{ group }} </h1>
    <hr>
    <span class="group_items">
      <div v-for="device in devices.sort((a, b) => a.sort_order - b.sort_order)" :key="device.id" class="group">
        <DeviceBarlabel :device=device />
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
    devices: {
      type: Array,
      default: () => []
    },
    group: { type: String }
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
</style>