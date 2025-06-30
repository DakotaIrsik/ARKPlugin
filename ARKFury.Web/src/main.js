/*!

 =========================================================
 * Vue Now UI Kit - v1.1.0
 =========================================================

 * Product Page: https://www.creative-tim.com/product/now-ui-kit
 * Copyright 2019 Creative Tim (http://www.creative-tim.com)

 * Designed by www.invisionapp.com Coded by www.creative-tim.com

 =========================================================

 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

 */
import Vue from 'vue';
import App from './App.vue';
// You can change this import to `import router from './starterRouter'` to quickly start development from a blank layout.
import router from './router';
import VueCookies from 'vue-cookies'
import NowUiKit from './plugins/now-ui-kit';
import LoadScript from 'vue-plugin-load-script'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'
import { BButtonToolbar } from 'bootstrap-vue'


// register the plugin on vue
import Toasted from 'vue-toasted';
import Loading from 'vue-loading-overlay';
// Import stylesheet
import 'vue-loading-overlay/dist/vue-loading.css';

Vue.use(Loading);
Vue.use(Toasted)

// Install BootstrapVue
Vue.use(BootstrapVue)
// Optionally install the BootstrapVue icon components plugin
Vue.use(IconsPlugin)
Vue.component('b-button-toolbar', BButtonToolbar)

Vue.config.productionTip = false;


Vue.use(require('vue-cookies'))
Vue.use(VueCookies)
// set default config
Vue.$cookies.config('7d')
// set global cookie
Vue.$cookies.set('theme','default');
Vue.$cookies.set('hover-time','1s');

Vue.use(NowUiKit);
Vue.use(LoadScript);

Vue.loadScript('https://js.braintreegateway.com/web/3.57.0/js/paypal-checkout.min.js');
Vue.loadScript('https://js.braintreegateway.com/web/3.57.0/js/client.min.js');
Vue.loadScript('https://demos.creative-tim.com/now-ui-kit/assets/css/now-ui-kit.min.css?v=1.3.0');


Vue.use(LoadScript);


new Vue({
  router,
  render: h => h(App)
}).$mount('#app');
