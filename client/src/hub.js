import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr";

export default {
  async install(Vue) {
    // Try connection to remote
    try {
      await this.CheckConnection(Vue, process.env.VUE_APP_PI_HOST);
    } catch (e) {
      console.log(e);
      console.log("BE Service not avaliable"); 
    }
  },

  async CheckConnection(Vue, ip) {
    console.log(`${ip}:${process.env.VUE_APP_PI_PORT}`);
    const connection = new HubConnectionBuilder().withUrl(`http://${ip}:${process.env.VUE_APP_PI_PORT}/hub`).configureLogging(LogLevel.Information).build();
    console.log(connection);
    // get and set Init Values
    const initData = await fetch(`http://${ip}:${process.env.VUE_APP_PI_PORT}`).then((response) => response.json());
    Vue.config.globalProperties.emitter.emit("Event", initData);

    Vue.config.globalProperties.emitter.on("closeConnection", () => {
      connection.stop;
    });

    connection.start();
    connection.on("SendMyEvent", (payload) => {
      Vue.config.globalProperties.emitter.emit("Event", payload);
    });

    connection.disconnected(function () {
      setTimeout(() => {
        console.log("try to reconnect");
        this.CheckConnection();
      }, 1000);
    });

    Vue.provide("$hub", connection);
    console.log("Connection succed");
  },
};
