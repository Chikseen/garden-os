import { createStore } from "vuex";
import router from "@/router/index";

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
			const sixHoursAgo = Date.now() - 60 * 60 * 60 * 6;

			state.noDataRecived = true;
			payload.devices.forEach((device) => {
				const deviceDate = new Date(device.date).getTime();
				if (sixHoursAgo < deviceDate) state.noDataRecived = false;
			});
			state.deviceData = payload;
		},
		setDeviceData(state, payload) {
			const devices = state.deviceData.devices;

			let i = devices.findIndex((d) => d.device_id === payload.device_id);
			state.deviceData.devices[i] = payload;
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
	actions: {},
	modules: {},
});
