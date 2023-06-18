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
  overflow-x: hidden;
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

.diagram {
  &_liniar {
    width: calc(100% - 30px);
    height: 15px;
    margin: 5px 15px;
    background-color: rgb(190, 190, 190);
    box-shadow: 0 0 2px 2px #69696921;
    border-radius: 5px;
    overflow: hidden;

    &_bar {
      height: 100%;
      background-color: aqua;
    }
  }
}
</style>
