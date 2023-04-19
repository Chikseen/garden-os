<template>
  <div id="app">
    <MapComponent />
  </div>
</template>

<script>
import MapComponent from "@/components/MapComponent.vue";

export default {
  name: "App",
  components: {
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
  overflow: hidden;
}

#app {
  height: 100vh;
  width: 100vw;
}
</style>
