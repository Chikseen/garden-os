<template>
  <div class="overlay_wrapper" :style="data == null ? 'height: 0%' : 'height: 75%'">
    <div class="overlay_header">
      <h1>{{ data?.name }}</h1>
      <h1 @click="$emit('close')">X</h1>
    </div>
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
    left: calc(10% - 15px);
    width: calc(80% - 20px);
    max-height: 500px;
    background-color: #e0e0e0e0;
    border-radius: 10px 10px 0 0;
    padding: 5px 25px 0 25px;
    padding-bottom: 0;
    box-shadow: 0 0 10px 5px #ffffff65;
    backdrop-filter: blur(20px);
    transition: 0.5s all;
  }

  &_header {
    display: flex;
    justify-content: space-between;
  }
}
</style>
