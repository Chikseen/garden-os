<template>
  <div class="overlay_wrapper" :style="data == null ? 'height: 0%' : 'height: 75%'">
    <div class="overlay_header">
      <h1>{{ data?.name }}</h1>
      <h1 @click="$emit('close')">X</h1>
    </div>
    <h2 v-if="data?.id == 'rain_collector'" @click="$emit('fake', 'rain_collector', Math.floor(Math.random() * 100))">Fake Data</h2>
    <h2 v-if="data?.id == 'moisture_I'" @click="$emit('fake', 'moisture_I', Math.floor(Math.random() * 100))">Fake Data</h2>
    <h2 v-if="data?.id == 'house_TV'">Play Sound Here</h2>
    <h6 style="margin: 0">{{ data }}</h6>
    <h6 style="margin: 0">{{ addCoordinateTransforms }}</h6>
  </div>
</template>

<script>
const map = require("../../rawMap.json");

export default {
  props: ["data"],
  computed: {
    addCoordinateTransforms() {
      const mainSearch = map.main.find((i) => i.properties.id == this.data?.id);
      const detailedSearch = map.detailed.find((i) => i.properties.id == this.data?.id);

      if (mainSearch) return mainSearch;
      return detailedSearch;
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
    max-height: 250px;
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
