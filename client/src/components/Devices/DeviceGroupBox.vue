<template>
  <div class="grid_item grid_item_large group_wrapper">
    <h1> {{ group }} </h1>
    <hr>
    <div v-for="device in devices.sort((a, b) => a.sort_order - b.sort_order )" :key="device.id" class="group">
      <DeviceBarlabel :device=device />
    </div>
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
  padding: 10px 0;

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