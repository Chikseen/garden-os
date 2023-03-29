import { createApp } from 'vue'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import store from './store'
import Hub from './hub'

createApp(App).use(store).use(router).use(Hub).mount('#app')

