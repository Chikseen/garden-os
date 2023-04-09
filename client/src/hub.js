import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  async install(Vue) {
    const connection = new HubConnectionBuilder()
      .withUrl(`http://${process.env.VUE_APP_PI_HOST}:${process.env.VUE_APP_PI_PORT}/hub`)
      .configureLogging(LogLevel.Information)
      .build()


    // get and set Init Values
    const initData = await fetch(`http://${process.env.VUE_APP_PI_HOST}:${process.env.VUE_APP_PI_PORT}`)
      .then(response => response.json());
    Vue.config.globalProperties.emitter.emit("Event", initData)

    Vue.config.globalProperties.emitter.on("closeConnection", () => {
      connection.stop
    });

    connection.start()
    connection.on('SendMyEvent', payload => {
      Vue.config.globalProperties.emitter.emit("Event", payload)
    })

    Vue.provide("$hub", connection)
  }
}