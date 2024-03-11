import { createStore } from "vuex";
import router from "@/router/index";
import { fetchDeviceMeta } from "@/services/apiService.js";

export default createStore({
	state: {
		user: null,
		keycloak: null,
		isAuth: false,
		gardenMeta: null,
		gardenList: null,
		deviceData: null,
		deviceStatus: null,
		noDataRecived: true,
	},
	getters: {},
	mutations: {
		setAuthState(state, payload) {
			state.isAuth = payload;
		},
		setUser(state, payload) {
			state.user = payload;
		},
		setGardenMeta(state, payload) {
			state.gardenMeta = payload;
		},
		setGardenList(state, payload) {
			if (payload != null) {
				state.gardenList = payload;
				state.gardenMeta = payload.garden_data.find((g) => g.garden_id == localStorage.getItem("selectedGarden"));
			} else {
				router.push("/");
				localStorage.clear();
			}
		},
		setAllDevicesData(state, payload) {
			state.deviceData = payload;
		},
		setDeviceData(state, payload) {
			const devices = state.deviceData.devices;
			devices.forEach(device => {
				const sensorKeys = Object.keys(payload.sensor)
				sensorKeys.forEach(sensorKey => {
					if (device.sensor_id == sensorKey) {
						device.corrected_values = payload.sensor[sensorKey]
						device.date = new Date()
					}
				})
			});
		},
		setKeycloak(state, payload) {
			state.keycloak = payload;
		},
		setNewDeviceStatus(state, payload) {
			if (state.noDataRecived) {
				payload.message = "No data recived in the last 6 hours";
				payload.status = "error";
			}
			state.deviceStatus = payload;
		},
	},
	actions: {
		async fetchDevices(context) {
			context.commit("setAllDevicesData", this.deviceMeta = await fetchDeviceMeta(localStorage.getItem("selectedGarden")));
		},
		async logout(context) {
			context.state.keycloak.logout({ redirectUri: process.env.VUE_APP_AUTH_LOGOUT });
		},
	},
	modules: {},
});
