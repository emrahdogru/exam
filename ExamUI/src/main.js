import Vue from 'vue';
import App from './App.vue';
import router from './router'
import axios from 'axios'

Vue.config.productionTip = true;

axios.defaults.baseURL = 'https://localhost:44366/';

new Vue({
    router: router,
    render: h => h(App)
}).$mount('#app');
