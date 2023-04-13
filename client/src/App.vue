<template>
  <div id="app">
    <MapComponent/>
    <!--
      <div>
        <h3>MY LIVE DATA</h3>
        <div v-if="values != null" style="width: 300px; height: 300px; resize: horizontal">
          <FuelMeter :value="values.potiOne"></FuelMeter>
          <FuelMeter :value="values.potiTwo"></FuelMeter>
        </div>
      </div>
    -->
  </div>
</template>

<script>
import FuelMeter from "@/components/FuelMeter.vue";
import MapComponent from "@/components/MapComponent.vue";

export default {
  name: "App",
  components: {
    FuelMeter,
    MapComponent,
  },
  props: {
    question: {
      type: Object,
      required: true,
    },
  },
  data: function () {
    return {
      values: null,
    };
  },
  created() {
    this.emitter.on("Event", (e) => {
      this.values = e;
    });
  },
};
</script>

<style lang="scss">
body,
html {
  height: 100vh;
  width: 100vw;
  margin: 0;
  padding: 0;
}

#app {
  height: 100vh;
  width: 100vw;
}
</style>
