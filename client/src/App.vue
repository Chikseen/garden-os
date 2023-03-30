<template>
  <div id="app">
    <h1>This is a websocket test for now</h1>
    <div>
      <h3>MY LIVE DATA</h3>
      <div style="width: 300px; height: 300px; resize: horizontal">
        <FuelMeter :value="testEvent"></FuelMeter>
      </div>
    </div>
  </div>
</template>

<script>
import FuelMeter from "@/components/FuelMeter.vue";

export default {
  name: "App",
  components: {
    FuelMeter,
  },
  props: {
    question: {
      type: Object,
      required: true,
    },
  },
  data: function () {
    return {
      testEvent: null,
    };
  },
  methods: {
    onScoreChanged(data) {
      console.log(data);
    },
  },
  created() {
    this.emitter.on("test", (e) => {
      this.testEvent = (e.replace(",", ".") * 1).toFixed(1) * 1;
    });
  },
};
</script>
