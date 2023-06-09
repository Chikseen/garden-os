<template>
  <div id="app">
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: "App",
  created() {
    this.emitter.on("HubDeviceData", (payload) => {
      console.log(payload)
      this.$store.commit('setDeviceData', payload)
    });
  },
};
</script>

<style lang="scss">
body,
html {
  margin: 0;
  padding: 0;
}

h1,
h2,
h3,
h4,
h5,
h6,
p {
  margin: 0;
  padding: 0;
}

#app {
  background-color: #f7f7f7;
  font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;
  min-height: 100vh;
}
</style>
