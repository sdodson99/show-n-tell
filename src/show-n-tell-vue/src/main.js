import Vue from "vue";
import router from "./router";
import store from './store'

import { BootstrapVue } from "bootstrap-vue";

import "bootstrap/dist/css/bootstrap.css";
import "bootstrap-vue/dist/bootstrap-vue.css";

import App from "./components/App.vue";
import ServiceContainer from "./services/service-container";

Vue.use(BootstrapVue);
Vue.config.productionTip = false;

new Vue({
  router,
  store,
  render: h =>
    h(App, {
      props: {
        authenticationService: ServiceContainer.AuthenticationService
      }
    })
}).$mount("#app");
