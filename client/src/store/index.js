import { createStore } from "vuex";
import router from "@/router/index";

export default createStore({
	state: {
		keycloak: null,
		isAuth: false,
		gardenMeta: null,
		gardenList: null,
		deviceData: null,
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
				state.gardenMeta = payload.garden_data[0];
			} else {
				router.push("/");
				localStorage.clear();
			}
		},
		setDeviceData(state, payload) {
			state.deviceData = payload;
		},
		setKeycloak(state, payload) {
			state.keycloak = payload;
		},
	},
	actions: {},
	modules: {},
});
