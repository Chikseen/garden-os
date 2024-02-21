import { HubConnectionBuilder } from "@aspnet/signalr";

export default {
	async install(Vue) {
		await this.CheckConnection(Vue, process.env.VUE_APP_PI_HOST);
	},

	async CheckConnection(Vue, ip) {
		const connection = new HubConnectionBuilder().withUrl(`${ip}hub`).build();

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

		connection.on("NewDeviceStatus", (payload) => {
			Vue.config.globalProperties.emitter.emit("NewDeviceStatus", payload);
		});

		Vue.provide("$hub", connection);
		Vue.config.globalProperties.$hub = connection;
		console.log("Connection succed");
	},
};
