import { createApp } from "vue";
import App from "./App.vue";
import "./registerServiceWorker";
import router from "./router";
import store from "./store";
import Hub from "./hub";
import mitt from "mitt";

const app = createApp(App);

const emitter = mitt();
app.config.globalProperties.emitter = emitter;

app.use(store).use(router).use(Hub.install(app)).mount("#app");

window.addEventListener("beforeunload", () => {
  app.config.globalProperties.emitter.emit("closeConnection", {});
});
