import { Router } from '@vaadin/router';

import '../src/components/show-n-tell-app/show-n-tell-app'

const outlet = document.getElementById('outlet');
const router = new Router(outlet);

router.setRoutes([
  {path: '/', component: 'show-n-tell-app'},
  {path: '(.*)', component: 'show-n-tell-app'}
]);