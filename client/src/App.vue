<template>
  <div id="app">
    <h1>This is a websocket test for now</h1>
  </div>
</template>

<script>
import { getCurrentInstance } from "vue";

export default {
  name: "App",
  props: {
    question: {
      type: Object,
      required: true,
    },
  },
  methods: {
    // This is called from the server through SignalR
    onScoreChanged({ questionId, score }) {
      console.log(questionId);
      console.log(score);
    },
  },
  created() {
    // Listen to score changes coming from SignalR events
    //this.$questionHub.$on("score-changed", this.onScoreChanged);
    const app = getCurrentInstance();
    app.appContext.provides.$hub.on("QuestionScoreChange", this.onScoreChanged)
  },
  beforeDestroy() {
    // Make sure to cleanup SignalR event handlers when removing the component
    //this.$questionHub.$off("score-changed", this.onScoreChanged);
  },
};
</script>
