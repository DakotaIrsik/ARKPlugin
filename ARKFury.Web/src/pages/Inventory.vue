<template>
  <div>
    <!-- <ActionableMetric></ActionableMetric> -->

    <div class="page-header clear-filter" filter-color="orange">
      <parallax class="page-header-image" style="background-image:url('img/bg5.jpg')"></parallax>
      <div class="container fluid">
        <div class="section resultContainer" v-if="cart && hasAnyItems">
          <b-container v-if="cart.length > 0">
            <b-row>
              <b-col class="my-auto">
                <p>Name</p>
              </b-col>

              <b-col class="my-auto">
                <p>Price</p>
              </b-col>
              <div v-if="isAdmin">
                <b-col class="my-auto">
                  <p>Lvl | Qlty</p>
                </b-col>
              </div>
              <b-col class="my-auto">
                <p>Quantity</p>
              </b-col>
              <b-col class="my-auto text-left">
                <p>Total</p>
              </b-col>
            </b-row>
            <div v-for="(item) in cart" :key="item.Id">
              <b-row v-if="!item.isClaimed" class="text-center">
                <b-col class="my-auto">
                  <p>{{item.name}}</p>
                </b-col>
                <b-col class="my-auto">
                  <img :src="item.image" style="height: 50px;" />
                </b-col>
                <b-col class="my-auto">
                  <p>{{item.price}}</p>
                </b-col>
                <div v-if="isAdmin">
                  <div v-if="item.lootType === 'Item'">
                    <b-col class="my-auto">
                      <input
                        type="number"
                        min="0"
                        max="10"
                        class="form-control"
                        v-model="item.quality"
                      />
                    </b-col>
                  </div>
                  <b-col class="my-auto" v-if="item.lootType === 'Dinosaur'">
                    <b-col class="my-auto">
                      <input
                        type="number"
                        min="0"
                        max="150"
                        class="form-control"
                        v-model="item.level"
                      />
                    </b-col>
                  </b-col>
                </div>
                <b-col class="my-auto">
                  <input type="number" min="0" class="form-control" v-model="item.amount" />
                </b-col>
                <b-col class="my-auto">
                  <p>{{item.amount * item.price}}</p>
                </b-col>
                <b-col class="my-auto">
                  <n-button
                    class="btn btn-primary"
                    @click="claimItem(item.id)"
                  >Claim ({{item.amount * item.price}})</n-button>
                </b-col>
              </b-row>
            </div>
            <b-row class="text-center">
              <b-col class="my-auto"></b-col>
              <b-col class="my-auto"></b-col>
              <b-col class="my-auto"></b-col>
              <b-col class="my-auto"></b-col>
              <b-col class="my-auto">
                <div v-if="isAdmin">
                  <n-button @click="claimItems()" type="btn btn-primary">Claim All! ({{total}})</n-button>
                </div>
              </b-col>
            </b-row>
          </b-container>
     
        </div>
             <b-container v-if="!hasAnyItems" class="no-results">
            <h2>Oh, no! It looks like your Inventory is empty ...</h2>
            <router-link to="/items" class="no-results">
              <b-button class="btn btn-primary">Return to Item Store</b-button>
            </router-link>
            <router-link to="/dinosaurs" class="no-results">
              <b-button class="btn btn-primary">Return to Dino Store</b-button>
            </router-link>
            <router-link to="/points" class="no-results">
              <b-button class="btn btn-primary">Return to Point Store</b-button>
            </router-link>
          </b-container>
        <modal :show.sync="modals.checkoutConfirmation" headerClasses="justify-content-center">
          <h4 slot="header" class="title title-up">Claim Method</h4>If you are currently logged into steam on the website and logged into the game in a safe place you may claim your items now.
          Items will go into your inventory and newly tamed dinos will be near you. Alternatively, you may generate a unique code to claim your loot in-game later. VIP status from lootbox and points can be seen on your profile page.
          <br />
          <br />
          <template slot="footer">
            <!-- <n-button
              class="btn btn-primary"
              @click.native="modals.actionableMetric = true"
            >Code (for later)!</n-button>-->
            <n-button @click="claimItem" class="btn btn-primary">I'm ready!</n-button>
          </template>
        </modal>
        <modal
          id="responseSummaryModal"
          :show.sync="modals.claimConfirmation"
          headerClasses="justify-content-center"
        >
          <h4 slot="header" class="title title-up">Claim Summary</h4>Check the summary of your Loot delivery below. Don't worry if you see an error below, we don't deduct points until a confirmed delivery. Please ensure you are logged into steam, you have a valid shopping cart, and have an available inventory in-game and try again late. If issues persist, contact an administrator in
          <a
            href="https://discord.gg/dVGCNu"
          >Discord</a>
          <br />
          <br />
          <b-container>
            <div v-if="claimResponse.dinosaurs && claimResponse.dinosaurs.length > 0">
              <b-row>
                <b-col>Dino</b-col>
                <b-col>Delivered</b-col>
                <b-col>Status</b-col>
              </b-row>
              <br />
              <br />
              <b-row v-for="response in claimResponse.dinosaurs" :key="response.name">
                <b-col>{{response.name}}</b-col>
                <b-col>{{response.amount}}</b-col>
                <b-col>
                  <label
                    v-if="!response.error || response.error.toLower().indexOf('success') !== -1"
                    style="color: green;"
                  >Success</label>
                  <label
                    v-if="response.error"
                    style="color: red; height: 50px;"
                    v-b-tooltip.hover
                    :title="response.error"
                  >Failure</label>
                </b-col>
              </b-row>
            </div>
            <div v-if="claimResponse.items && claimResponse.items.length > 0">
              <b-row>
                <b-col>Item</b-col>
                <b-col>Delivered</b-col>
                <b-col>Status</b-col>
              </b-row>
              <br />
              <br />
              <b-row v-for="response in claimResponse.items" :key="response.name">
                <b-col>{{response.name}}</b-col>
                <b-col>{{response.amount}}</b-col>
                <b-col>
                  <label
                    v-if="!response.error || response.error.toLowerCase().indexOf('success') !== -1"
                    style="color: green;"
                  >Success</label>
                  <label
                    v-if="response.error"
                    style="color: red; height: 50px;"
                    v-b-tooltip.hover
                    :title="response.error"
                  >Failure</label>
                </b-col>
              </b-row>
            </div>
          </b-container>
          <template slot="footer">
            <a class="nav-link">
              <n-button @click.native="this.$router.go()" class="btn btn-primary">Back to Cart</n-button>
            </a>
          </template>
        </modal>
      </div>
    </div>
  </div>
</template>
<script>
import axios from "axios";
import Toasted from "vue-toasted";
import { Card, Button, Modal, FormGroupInput } from "@/components";
import { Popover, Tooltip, DatePicker } from "element-ui";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";
import ActionableMetric from "../components/Modals/ActionableMetric";

export default {
  components: {
    Card,
    Modal,
    [Button.name]: Button,
    [Popover.name]: Popover,
    [Tooltip.name]: Tooltip,
    [DatePicker.name]: DatePicker,
    [FormGroupInput.name]: FormGroupInput
  },
  computed: {
    total() {
      let total = 0;
      if (this.cart) {
        for (var i = 0; i < this.cart.length; i++) {
          if (!this.cart[i].isClaimed) {
            total += this.cart[i].price * this.cart[i].amount;
          }
        }
      }

      return parseInt(total);
    },
    canCheckout() {
      let total = 0;
      if (this.cart) {
        for (var i = 0; i < this.cart.length; i++) {
          total += this.cart[i].price * this.cart[i].amount;
        }
      }
      return total() > 0 && hasAnyItems();
    },

      hasAnyItems() {
  let result = false;
      if (this.cart) {
        for (var i = 0; i < this.cart.length; i++) {
          if (!this.cart[i].isClaimed) {
          result = true;
        }
      }
      }
      return result;
    },


    hasUnclaimedItems() {
  let result = false;
      if (this.cart) {
        for (var i = 0; i < this.cart.length; i++) {
          if (!this.cart[i].isClaimed) {
          result = true;
        }
      }
      }
      return result;
    },

      hasClaimedItems() {
  let result = false;
      if (this.cart) {
        for (var i = 0; i < this.cart.length; i++) {
          if (this.cart[i].isClaimed) {
          result = true;
        }
      }
      }
      return result;
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
    },
    cartIsEmpty() {
      let user = this.$cookies.get("SteamId");
      if (user) {
        if (
          this.$cookies.get("SteamId") === "76561198079290065" ||
          this.$cookies.get("SteamId") === "76561198079632245" ||
          this.$cookies.get("SteamId") === "76561198805944455" ||
          this.$cookies.get("SteamId") === "76561198391529744"  ||
          this.$cookies.get("SteamId") === "76561198216743151"
        )
          return true;
      }
      return false;
    }
  },
  data() {
    return {
      cart: undefined,
      modals: {
        action: false,
        actionableMetric: false,
        checkoutConfirmation: false,
        claimConfirmation: false
      },
      claimResponse: []
    };
  },
  mounted() {
    this.user = this.$cookies.get("SteamId");
    axios
      .get(
        process.env.VUE_APP_ARKFURY_API_ENDPOINT +
          "/Player/" +
          this.user +
          "/Inventory?" +
          this.jsonPaging()
      )
      .then(response => {
        this.errored = false;
        this.items = this.cart = response.data.data;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));
  },
  methods: {
    removeFromCart: function(itemName) {
      let cart = JSON.parse(this.$cookies.get("cart"));

      var itemToRemoveIndex = cart.indexOf(cart.find(c => c.Name == itemName));

      if (itemToRemoveIndex > -1) {
        cart.splice(itemToRemoveIndex, 1);
      }
      this.cart = cart;
      this.$cookies.set("cart", JSON.stringify(cart));
      document.getElementById("cartItemCount").innerText =
        parseInt(document.getElementById("cartItemCount").innerText) - 1;
    },
    claimItems: function() {
      let loader = this.$loading.show({
        // Pass props by their camelCased names
        container: this.$refs.loadingContainer,
        canCancel: false, // default false
        onCancel: this.yourMethodName,
        color: "#285738",
        loader: "spinner",
        width: 64,
        height: 64,
        backgroundColor: "#ffffff",
        opacity: 0.5,
        zIndex: 999
      });
      let dinos = [];
      let items = [];
      let points = [];
      let lootboxes = [];
      let cart = this.cart;

      let newCart = [];
      for (var i = 0; i < cart.length; i++) {
        if (cart[i].LootType === "Dinosaur") dinos.push(cart[i]);
        else if (cart[i].LootType === "Item") items.push(cart[i]);
        else if (cart[i].LootType === "Lootbox") lootboxes.push(cart[i]);
      }

      this.user = this.$cookies.get("SteamId");
      axios
        .post(
          process.env.VUE_APP_ARKFURY_API_ENDPOINT +
            "/Inventory/" +
            this.user +
            "/Claim",
          {
            Items: items,
            Dinosaurs: dinos,
            Lootboxes: lootboxes,
            SteamId: this.$cookies.get("SteamId")
          }
        )
        .then(response => {
          this.errored = false;
          this.claimResponse = response.data;
          this.modals.claimConfirmation = true;
          let cart = this.cart;
          let newCart = [];
          for (var i = 0; i < cart.length; i++) {
            for (var j = 0; j < this.claimResponse.items.length; j++) {
              if (
                this.claimResponse.items[j].name == cart[i].Name &&
                !this.claimResponse.items[j].error
              ) {
                cart = cart.splice(i, i);
              }
            }

            for (var j = 0; j < this.claimResponse.dinosaurs.length; j++) {
              if (
                this.claimResponse.dinosaurs[j].name == cart[i].Name &&
                !this.claimResponse.dinosaurs[j].error
              ) {
                cart = cart.splice(i, i);
              }
            }
          }

          this.cart = newCart;
        })
        .catch(() => (this.errored = true))
        .finally(() => loader.hide());
    },
    claimItem: function(itemId) {
      let loader = this.$loading.show({
        // Pass props by their camelCased names
        container: this.$refs.loadingContainer,
        canCancel: false, // default false
        onCancel: this.yourMethodName,
        color: "#285738",
        loader: "spinner",
        width: 64,
        height: 64,
        backgroundColor: "#ffffff",
        opacity: 0.5,
        zIndex: 999
      });

      this.user = this.$cookies.get("SteamId");
 

      axios
        .get(
          process.env.VUE_APP_ARKFURY_API_ENDPOINT +
            "/Inventory/" +
            this.user +
            "/Claim/" +
            itemId
        )
        .then(response => {
          this.errored = false;
          this.claimResponse = response.data;
          this.modals.claimConfirmation = true;
          let cart = this.cart;
          let newCart = [];
          for (var i = 0; i < cart.length; i++) {
            for (var j = 0; j < this.claimResponse.items.length; j++) {
              if (
                this.claimResponse.items[j].name == cart[i].name &&
                !this.claimResponse.items[j].error
              ) {
                cart = cart.splice(i, i);
              }
            }

            for (var j = 0; j < this.claimResponse.dinosaurs.length; j++) {
              if (
                this.claimResponse.dinosaurs[j].name == cart[i].name &&
                !this.claimResponse.dinosaurs[j].error
              ) {
                cart = cart.splice(i, i);
              }
            }
          }

          this.cart = newCart;
        })
        .catch(() => (this.errored = true))
        .finally(() => loader.hide());
    },
    saveLootbox: function() {
      let loader = this.$loading.show({
        // Pass props by their camelCased names
        container: this.$refs.loadingContainer,
        canCancel: false, // default false
        onCancel: this.yourMethodName,
        color: "#285738",
        loader: "spinner",
        width: 64,
        height: 64,
        backgroundColor: "#ffffff",
        opacity: 0.5,
        zIndex: 999
      });
      let dinos = [];
      let items = [];
      let points = [];
      let lootboxes = [];
      let cart = this.cart;

      let newCart = [];
      for (var i = 0; i < cart.length; i++) {
        if (cart[i].lootType === "Dinosaur") {
          dinos.push(cart[i]);
        } else if (cart[i].lootType === "Item") {
          items.push(cart[i]);
        } else if (cart[i].lootType === "Lootbox") {
          lootboxes.push(cart[i]);
        }
      }
      axios
        .post(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/Lootbox/Save", {
          Items: items,
          Dinosaurs: dinos,
          Lootboxes: lootboxes,
          SteamId: this.$cookies.get("SteamId")
        })
        .then(response => {
          this.errored = false;
          this.claimResponse = response.data;
          this.modals.claimConfirmation = true;
          let cart = this.cart;
          let newCart = [];
          for (var i = 0; i < cart.length; i++) {
            for (var j = 0; j < this.claimResponse.items.length; j++) {
              if (
                this.claimResponse.items[j].name == cart[i].name &&
                !this.claimResponse.items[j].error
              ) {
                cart = cart.splice(i, i);
              }
            }

            for (var j = 0; j < this.claimResponse.dinosaurs.length; j++) {
              if (
                this.claimResponse.dinosaurs[j].name == cart[i].name &&
                !this.claimResponse.dinosaurs[j].error
              ) {
                cart = cart.splice(i, i);
              }
            }
          }

          this.$cookies.set("cart", JSON.stringify(newCart));
        })
        .catch(() => (this.errored = true))
        .finally(() => loader.hide());
    },
    jsonPaging: function() {
      let x = {
        Size: 1500,
        From: 0,
        Sort: "+Name",
        Fields: ""
      };
      let y = Object.keys(x)
        .map(key => key + "=" + x[key])
        .join("&");
      return y;
    }
  }
};
</script>
<style>
.no-results {
  padding: 50px;
}

.modal-body {
  max-height: 800px;
  color: black;
}

.modal-content {
  margin-top: 60px;
}

p {
  color: #fff;
}
.resultContainer {
  overflow-y: auto !important;
  height: 1100px;
}
</style>