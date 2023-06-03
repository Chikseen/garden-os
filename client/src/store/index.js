import { createStore } from "vuex";

export default createStore({
  state: {
    isAuth: false,
    is3dView: false,
    gardenMeta: null,
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
    set3dView(state, payload) {
      state.is3dView = payload;
    },
    setDeviceData(state, payload) {
      state.deviceData = payload;
    },
  },
  actions: {},
  modules: {},
});
