<template>
  <router-view v-if="!authPending"></router-view>
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
    if (!localStorage.getItem("accessToken")) {
      const keycloak = new Keycloak({
        url: "https://auth.drunc.net",
        realm: process.env.VUE_APP_AUTH_REALM,
        clientId: process.env.VUE_APP_AUTH_CLIENT_ID,

      });
      await keycloak
        .init({
          onLoad: 'login-required',
          redirectUri: process.env.VUE_APP_AUTH_REDIRECT,
        })

      if (keycloak.authenticated) {
        localStorage.setItem("userName", keycloak.idTokenParsed.preferred_username)
        localStorage.setItem("accessToken", keycloak.token)
        this.$store.commit("setKeycloak", keycloak)
      }
    }
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

button {
  position: relative;
  cursor: pointer;
  border-radius: 10px;
  border: none;
  background-color: #eae3d1;
  height: 3rem;
}

input {
  border-radius: 10px;
  border: none;
  background-color: #eae3d280;
  padding: 10px;
}

#app {
  position: relative;
  display: flex;
  flex-direction: column;
  background-color: #f7f7f7;
  font-family: 'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif;
  min-height: 100vh;
  max-width: 100vw;
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
