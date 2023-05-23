<template>
  <div id="app">
    <MapComponent v-if="isAuth" />
    <div v-else>
      <h1>No Auth key</h1>
      <div>
        <h3>User ID</h3>
        <input type="text" @change="insertID" />
      </div>
      <div>
        <h3>Api Key</h3>
        <input type="text" @change="insertApiKey" />
      </div>
      <button @click="checkUser">Login</button>
    </div>
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
      isAuth: true,
      AuthId: "",
      AuthApiKey: "",
    };
  },
  methods: {
    insertID(e) {
      this.AuthId = e.target.value;
    },
    insertApiKey(e) {
      this.AuthApiKey = e.target.value;
    },
    async checkUser() {
      const json = await fetch(`${process.env.VUE_APP_PI_HOST}user/${this.AuthId}/validate`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${this.AuthApiKey}`,
        },
      });
      const res = await json.json();
      console.log(res);
      if (res == true) {
        localStorage.setItem("id", this.AuthId.toString());
        localStorage.setItem("apiToken", this.AuthApiKey.toString());
        this.isAuth = true;
      } else this.isAuth = false;
      console.log(this.isAuth);
    },
  },
  created() {
    this.emitter.on("Event", (e) => {
      this.values = e;
    });
  },
  async mounted() {
    this.AuthId = localStorage.getItem("id");
    this.AuthApiKey = localStorage.getItem("apiToken");
    await this.checkUser();
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
