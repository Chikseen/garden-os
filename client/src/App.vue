<template>
  <div id="app">
    <h1>This is a websocket test for now</h1>
    <div>
      <h3>MY LIVE DATA</h3>
      <p>{{ testEvent }}</p>
    </div>
  </div>
</template>

<script>
export default {
  name: "App",
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
      this.testEvent = e;
    });
  },
};
</script>
