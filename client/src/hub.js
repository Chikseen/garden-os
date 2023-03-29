import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'
 
const connection = new HubConnectionBuilder()
  .withUrl('http://localhost:5082/question-hub')
  .configureLogging(LogLevel.Information)
  .build()
connection.start()