import { HubConnectionBuilder, LogLevel } from "@aspnet/signalr";

export default {
	async install(Vue) {
		await this.CheckConnection(Vue, process.env.VUE_APP_PI_HOST);
	},

	async CheckConnection(Vue, ip) {
		const connection = new HubConnectionBuilder().withUrl(`${ip}hub`).configureLogging(LogLevel.Information).build();

		Vue.config.globalProperties.emitter.on("closeConnection", () => {
			connection.stop;
		});

		connection.start();
		connection.on("SendMyEvent", (payload) => {
			Vue.config.globalProperties.emitter.emit("HubDeviceData", payload);
		});

		connection.on("SendCurrentDeviceData", (payload) => {
			Vue.config.globalProperties.emitter.emit("HubDeviceData", payload);
		});

		/*connection.disconnected(() => {
      setTimeout(async () => {
        console.log("try to reconnect");
        await this.CheckConnection(Vue, ip);
      }, 1000);
    });*/

		Vue.provide("$hub", connection);
		Vue.config.globalProperties.$hub = connection;
		console.log("Connection succed");
	},
};
