import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  install(Vue) {
    const connection = new HubConnectionBuilder()
      .withUrl('http://93.201.163.148:5082/hub')
      .configureLogging(LogLevel.Information)
      .build()

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