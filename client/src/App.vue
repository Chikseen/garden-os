<template>
  <div id="app">
    <Overview v-if="isAuth" />
    <Landing v-else />
  </div>
</template>

<script>
import Overview from "@/views/Overview.vue";
import Landing from "@/views/Landing.vue";

import { mapState } from "vuex";

export default {
  name: "App",
  components: {
    Overview,
    Landing,
  },
  computed: {
    ...mapState({
      isAuth: (state) => state.isAuth,
    }),
  },
  async mounted() {
    // get and set Init Values
    const response = await fetch(`${process.env.VUE_APP_PI_HOST}user/${localStorage.getItem("id")}/datalog`, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("apiToken")}`,
      },
    })
    const res = await response.json()
    this.$store.commit("setDeviceData", res)
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
  background-color: #eae5d2;
  font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;
  min-height: 100vh;
}
</style>
