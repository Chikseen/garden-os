import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  install(Vue) {
    // use new Vue instance as an event bus
    const questionHub = require('vue').default;

    const connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5082/question-hub')
      .configureLogging(LogLevel.Information)
      .build()
    connection.start()

    // Forward server side SignalR events through $questionHub, where components will listen to them
    connection.on('MyEvent', (questionId, score) => {
      questionHub.$emit('score-changed', { questionId, score })
    })

    Vue.provide("$hub", connection)
  }
}