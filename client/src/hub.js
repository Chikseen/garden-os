import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr";

export default {
  async install(Vue) {
    // Try connection to remote
    try {
      await this.CheckConnection(Vue, process.env.VUE_APP_PI_HOST);
    } catch (e) {
      console.log(e);
      // Try connection to last known Ip
      try {
        await this.CheckConnection(Vue, localStorage.getItem("lastWorkingSubIp"));
      } catch (e) {
        console.log(e);
        // Try search new Ip
        let hasConnection = false;
        let subIp = 100;
        while (!hasConnection) {
          try {
            const ip = `192.168.8.${subIp}`;
            await this.CheckConnection(Vue, ip);
            hasConnection = true;
            localStorage.setItem("lastWorkingSubIp", subIp);
          } catch (e) {
            console.log(e);
            subIp++;
            if (subIp > 255) subIp = 100;
            console.error(`Could not connect to any service`);
          }
        }
      }
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
