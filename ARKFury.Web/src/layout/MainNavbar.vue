<template>
  <navbar
    position="fixed"
    type="primary"
    :transparent="transparent"
    :color-on-scroll="colorOnScroll"
    menu-classes="ml-auto"
  >
    <nav-link to="/" class="nav-link">
      <!-- <img style="position: fixed;" src="./../../public/img/icon-100.png" /> -->
      <p style="padding-left: 80px;">ARK Fury</p>
    </nav-link>

    <template slot="navbar-menu">
      <drop-down
        tag="li"
        title="Admin"
        icon="now-ui-icons design_app"
        class="nav-item"
        v-if="isAdmin"
      >
        <nav-link to="/editDinosaurs">
          <i class="now-ui-icons education_paper"></i> Edit Dinosaurs
        </nav-link>
        <nav-link to="/edititems">
          <i class="now-ui-icons education_paper"></i> Edit Items
        </nav-link>
        <nav-link to="/tradelogs">
          <i class="now-ui-icons education_paper"></i> Trade Logs
        </nav-link>
      </drop-down>

      <drop-down tag="li" title="Shop" icon="now-ui-icons design_app" class="nav-item">
        <nav-link to="/points">
          <i class="now-ui-icons education_paper"></i> Points
        </nav-link>
        <nav-link to="/dinosaurs">
          <i class="now-ui-icons education_paper"></i> Dinosaurs
        </nav-link>
        <nav-link to="/items">
          <i class="now-ui-icons education_paper"></i> Items
        </nav-link>
        <nav-link to="/lootboxes">
          <i class="now-ui-icons education_paper"></i> Lootboxes
        </nav-link>
      </drop-down>

      <div v-if="this.steamId">
        <li class="nav-item">
          <a class="nav-link" data-placement="bottom" @click="signOut()">
            <i class="now-ui-icons arrows-1_cloud-download-93"></i>
            <p>
              Signout (
              <span id="pointCount">{{this.points}}</span>)
            </p>
          </a>
        </li>
      </div>
      <div v-else>
        <form action="https://steamcommunity.com/openid/login" id="steamLogin" method="post">
          <input
            type="hidden"
            name="openid.identity"
            value="http://specs.openid.net/auth/2.0/identifier_select"
          />
          <input
            type="hidden"
            name="openid.claimed_id"
            value="http://specs.openid.net/auth/2.0/identifier_select"
          />
          <input type="hidden" name="openid.ns" value="http://specs.openid.net/auth/2.0" />
          <input type="hidden" name="openid.mode" value="checkid_setup" />
          <input type="hidden" name="openid.realm" v-model="steamCallBackUrl" />
          <input type="hidden" name="openid.return_to" v-model="steamCallBackUrl" />
          <li class="nav-item">
            <a class="nav-link" data-placement="bottom" @click="redirectToSteam">
              <i class="now-ui-icons arrows-1_cloud-download-93"></i>
              <p>Signin</p>
            </a>
          </li>
        </form>
      </div>
      <li class="nav-item">
        <nav-link to="/inventory" class="nav-link">
          <i class="now-ui-icons arrows-1_cloud-download-93"></i>
          <p>Inventory</p>
        </nav-link>
      </li>
      <li class="nav-item">
        <nav-link to="/advantage" class="nav-link">
          <i class="now-ui-icons arrows-1_cloud-download-93"></i>
          <p>Advantage</p>
        </nav-link>
      </li>
       <li class="nav-item">
        <nav-link to="/settings" class="nav-link">
          <i class="now-ui-icons arrows-1_cloud-download-93"></i>
          <p>Settings</p>
        </nav-link>
      </li>
      <li class="nav-item">
        <nav-link to="/rules" class="nav-link">
          <i class="now-ui-icons arrows-1_cloud-download-93"></i>
          <p>Rules</p>
        </nav-link>
      </li>
      <li class="nav-item">
        <nav-link to="/cart" class="nav-link">
          <i class="fa fa-2x fa-shopping-cart"></i>
          <label id="cartItemCount">{{this.cartCount}}</label>
          <p class="d-lg-none d-xl-none"></p>
        </nav-link>
      </li>
      <drop-down tag="li" title="Vote Websites" icon="now-ui-icons design_app" class="nav-item">
        <li class="nav-item">
          <a
            class="nav-link"
            data-placement="bottom"
            href="https://www.trackyserver.com/server/arkfury-wipe-2-25-25x-10m-tribe-points-kits-lootboxes-orp-discord-active-admins-484621"
          >
            <i class="now-ui-icons arrows-1_cloud-download-93"></i>
            Tracky
          </a>
        </li>
        <li class="nav-item">
          <a
            class="nav-link"
            data-placement="bottom"
            href="https://toparkservers.com/server/52.4.214.43:27016"
          >
            <i class="now-ui-icons arrows-1_cloud-download-93"></i>
            Top ARK Servers
          </a>
        </li>
        <li class="nav-item">
          <a class="nav-link" data-placement="bottom" href="https://bestservers.com/server/163">
            <i class="now-ui-icons arrows-1_cloud-download-93"></i>
            Best Servers
          </a>
        </li>
        <li class="nav-item">
          <a
            class="nav-link"
            data-placement="bottom"
            href="https://ark-servers.net/server/214092/vote/"
          >
            <i class="now-ui-icons arrows-1_cloud-download-93"></i>
            ARK-Servers
          </a>
        </li>
      </drop-down>
    </template>
  </navbar>
</template>

<script>
import { DropDown, NavbarToggleButton, Navbar, NavLink } from "@/components";
import { Popover } from "element-ui";
import axios from "axios";

export default {
  name: "main-navbar",
  props: {
    transparent: Boolean,
    colorOnScroll: Number
  },
  components: {
    DropDown,
    Navbar,
    NavbarToggleButton,
    NavLink,
    [Popover.name]: Popover
  },
  data() {
    return {
      steamId: "",
      steamCallbackUrl: undefined,
      points: 0
    };
  },
  watchers: {
  cartCount() {
      let cart = JSON.parse(this.$cookies.get("cart"));
      if (cart) return cart.length;
      return 0;
    },
  },
  computed: {
    cartCount() {
      let cart = JSON.parse(this.$cookies.get("cart"));
      if (cart) return cart.length;
      return 0;
    },
    isAdmin() {
      let user = this.$cookies.get("SteamId");
      if (user) {
        if (
          this.$cookies.get("SteamId") === "76561198079290065" ||
          this.$cookies.get("SteamId") === "76561198079632245" ||
          this.$cookies.get("SteamId") === "76561198805944455" ||
          this.$cookies.get("SteamId") === "76561198391529744" ||
          this.$cookies.get("SteamId") === "76561198216743151"
        )
          return true;
      }
      return false;
    }
  },

  mounted() {
    this.steamId = this.$cookies.get("SteamId");
    this.steamCallBackUrl = process.env.VUE_APP_STEAM_CALLBACK_URL;
    axios
      .get(
        process.env.VUE_APP_ARKFURY_API_ENDPOINT +
          "/Player/" + this.steamId + "/Points"
      )
      .then(response => {
        this.errored = false;
        this.items = this.points = response.data;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));
  },
  methods: {
    signOut() {
      this.$cookies.remove("SteamId");
      this.$cookies.remove("cart");
      this.steamId = undefined;
      this.$router.push({ path: "/" });
    },
    redirectToSteam() {
      document.getElementById("steamLogin").submit();
    }
  }
};
</script>

<style scoped>
#cartItemCount {
  vertical-align: top;
  color: orangered;
  font-weight: 900;
  font-size: x-small;
  margin-left: 5px;
  margin-top: -15px;
}

.dropdown-item.active,
.dropdown-item:active {
  background-color: #007bff00;
}

.navbar a:not(.btn):not(.dropdown-item) {
  color: #555555;
}
</style>