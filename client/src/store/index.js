import { createStore } from "vuex";
import router from "@/router/index";

export default createStore({
	state: {
		keycloak: null,
		isAuth: false,
		gardenMeta: null,
		gardenList: null,
		deviceData: null,
		deviceStatus: null,
	},
	getters: {},
	mutations: {
		setAuthState(state, payload) {
			state.isAuth = payload;
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
			console.log(state.deviceData);
			console.log(payload);
			const devices = state.deviceData.devices;

			let i = devices.findIndex((d) => d.device_id === payload.device_id);
			state.deviceData.devices[i] = payload;
		},
		setKeycloak(state, payload) {
			state.keycloak = payload;
		},
		setNewDeviceStatus(state, payload) {
			state.deviceStatus = payload;
		},
	},
	actions: {},
	modules: {},
});
