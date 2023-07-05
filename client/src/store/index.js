import { createStore } from "vuex";

export default createStore({
  state: {
    keycloak: null,
    isAuth: false,
    is3dView: false,
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
      state.gardenList = payload;
      state.gardenMeta = payload.garden_data[0];
    },
    set3dView(state, payload) {
      state.is3dView = payload;
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
