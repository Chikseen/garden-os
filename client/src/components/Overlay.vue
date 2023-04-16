<template>
  <div class="overlay_wrapper" :style="data == null ? 'height: 0%' : 'height: 75%'">
    <p @click="$emit('close')">X</p>
    <p>{{ data }}</p>
    <p>{{ addCoordinateTransforms }}</p>
  </div>
</template>

<script>
const map = require("../../rawMap.json");

export default {
  props: ["data"],
  computed: {
    addCoordinateTransforms() {
      return map.find((i) => (i.properties.id = this.data?.id));
    },
  },
};
</script>

<style lang="scss">
.overlay {
  &_wrapper {
    position: absolute;
    bottom: 0;
    left: 10%;
    width: calc(80% - 20px);
    max-height: 1000px;
    background-color: #f0f8ffaf;
    border-radius: 10px 10px 0 0;
    padding: 10px;
    padding-bottom: 0;
    box-shadow: 0 0 10px 5px #ffffff65;
    backdrop-filter: blur(20px);
    transition: 0.5s all;
  }
}
</style>
