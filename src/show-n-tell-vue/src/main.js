import Vue from "vue";
import router from "./router"

import { BootstrapVue } from 'bootstrap-vue'

import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

import App from "./components/App.vue";

Vue.use(BootstrapVue)
Vue.config.productionTip = false;

new Vue({
  router: router,
  render: h => h(App)
}).$mount("#app");
