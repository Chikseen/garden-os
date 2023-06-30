<template>
  <div id="app" v-if="!authPending">
    <router-view></router-view>
  </div>
</template>

<script>
import Keycloak from "keycloak-js"

export default {
  name: "App",
  data() {
    return {
      authPending: true
    }
  },
  created() {
    this.emitter.on("HubDeviceData", (payload) => {
      this.$store.commit('setDeviceData', payload)
    });
  },
  async beforeMount() {
    const keycloak = new Keycloak({
      url: "https://auth.drunc.net",
      realm: "GardenOS-DEV",
      clientId: "test-client",

    });
    await keycloak
      .init({
        onLoad: 'login-required',
        redirectUri: "http://localhost:8080/login",
      })

    console.log('Auth', keycloak.authenticated);
    if (keycloak.authenticated) {
      console.log('Auth succed', keycloak);
      localStorage.setItem("accessToken", keycloak.token)
      this.$store.commit("setKeycloak", keycloak)
    } else {
      console.log('User not authenticated!');
    }

    console.log("kc: ", keycloak)
    this.authPending = false
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
  position: relative;
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
      background-color: #03a9f4;
    }
  }
}
</style>
