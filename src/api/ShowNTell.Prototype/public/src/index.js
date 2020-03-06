import { Router } from "../node_modules/@vaadin/router/dist/vaadin-router.js";
import "./components/show-n-tell-app/show-n-tell-app.js";
const outlet = document.getElementById('outlet');
const router = new Router(outlet);
router.setRoutes([{
  path: '/',
  component: 'show-n-tell-app'
}, {
  path: '(.*)',
  component: 'show-n-tell-app'
}]);