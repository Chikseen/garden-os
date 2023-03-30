import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  install(Vue) {
    const connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5082/question-hub')
      .configureLogging(LogLevel.Information)
      .build()

    Vue.config.globalProperties.emitter.on("closeConnection", () => {
      connection.stop
    });

    connection.start()
    // Forward server side SignalR events through $questionHub, where components will listen to them
    connection.on('SendMyEvent', payload => {
      Vue.config.globalProperties.emitter.emit("test", payload)
    })

    Vue.provide("$hub", connection)
  }
}