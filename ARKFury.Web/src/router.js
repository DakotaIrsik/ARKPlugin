import Vue from 'vue';
import Router from 'vue-router';
import Index from './pages/Index.vue';
import Lootboxes from './pages/Lootboxes.vue';
import Rules from './pages/Rules.vue'
import Points from './pages/Points.vue';
import Items from './pages/Items.vue';
import EditItems from './pages/EditItems.vue';
import Dinosaurs from './pages/Dinosaurs.vue';
import EditDinosaurs from './pages/EditDinosaurs.vue';
import Packages from './pages/Packages.vue';
import TradeLogs from './pages/TradeLogs.vue';
import Servers from './pages/Servers.vue';
import Cart from './pages/Cart.vue';
import Inventory from './pages/Inventory.vue';
import Settings from './pages/Settings.vue';
import MainNavbar from './layout/MainNavbar.vue';
import MainFooter from './layout/MainFooter.vue';
import SteamAuth from './components/SteamAuth.vue';
import RegisterServer from './components/RegisterServer.vue'
import Advantage from './pages/Advantage.vue';

Vue.use(Router);

export default new Router({
  linkExactActiveClass: 'active',
  routes: [
    {
      path: '/',
      name: 'index',
      components: { default: Index, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/lootboxes',
      name: 'lootboxes',
      components: { default: Lootboxes, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/inventory',
      name: 'inventory',
      components: { default: Inventory, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/advantage',
      name: 'advantage',
      components: { default: Advantage, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/settings',
      name: 'settings',
      components: { default: Settings, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/points',
      name: 'points',
      components: { default: Points, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/dinosaurs',
      name: 'dinosaurs',
      components: { default: Dinosaurs, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/editdinosaurs',
      name: 'editdinosaurs',
      components: { default: EditDinosaurs, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/tradelogs',
      name: 'tradelogs',
      components: { default: TradeLogs, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/items',
      name: 'items',
      components: { default: Items, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/edititems',
      name: 'edititems',
      components: { default: EditItems, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/cart',
      name: 'cart',
      components: { default: Cart, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/packages',
      name: 'packages',
      components: { default: Packages, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/rules',
      name: 'rules',
      components: { default: Rules, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/steam/callback',
      name: 'steam',
      components: { default: SteamAuth, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/servers',
      name: 'servers',
      components: { default: Servers, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    },
    {
      path: '/registerserver',
      name: 'registerserver',
      components: { default: RegisterServer, header: MainNavbar, footer: MainFooter },
      props: {
        header: { colorOnScroll: 400 },
        footer: { backgroundColor: 'black' }
      }
    }
  ],
  scrollBehavior: to => {
    if (to.hash) {
      return { selector: to.hash };
    } else {
      return { x: 0, y: 0 };
    }
  }
});
