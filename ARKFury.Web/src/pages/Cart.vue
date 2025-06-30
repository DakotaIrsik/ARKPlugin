<template>
  <div>
    <div class="page-header clear-filter" filter-color="orange">
      <parallax class="page-header-image" style="background-image:url('img/bg5.jpg')"></parallax>
      <div class="container fluid">
        <div class="section resultContainer" v-if="cart">
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
              <b-row class="text-center">
                <b-col class="my-auto">
                  <p>{{item.Name}}</p>
                </b-col>
                <b-col class="my-auto">
                  <img :src="item.Image" style="height: 50px;" />
                </b-col>
                <b-col class="my-auto">
                  <p>{{item.Price}}</p>
                </b-col>
                <div v-if="isAdmin">
                  <b-col class="my-auto" v-if="item.lootType === 'Item'">
                    <input
                      id="item.Id"
                      type="number"
                      min="0"
                      max="10"
                      class="form-control"
                      v-model="item.quality"
                    />
                  </b-col>
                  <b-col class="my-auto" v-if="item.lootType === 'Dinosaur'">
                    <input
                      id="item.Id"
                      type="number"
                      min="0"
                      max="150"
                      class="form-control"
                      v-model="item.level"
                    />
                  </b-col>
                </div>

                <b-col class="my-auto">
                  <input type="number" min="0" class="form-control" v-model="item.Amount" />
                </b-col>
                <b-col class="my-auto">
                  <p>{{item.Amount * item.Price}}</p>
                </b-col>
                <b-col class="my-auto">
                  <a @click="removeFromCart(item.Name)">Remove</a>
                </b-col>
              </b-row>
            </div>
            <b-row class="text-center">
              <b-col class="my-auto"></b-col>
              <b-col class="my-auto"></b-col>
              <b-col class="my-auto"></b-col>
              <b-col class="my-auto"></b-col>
              <b-col class="my-auto">
                <div v-if="!canCheckout">
                  <n-button
                    disabled
                    type="btn btn-primary"
                    @click.native="modals.checkoutConfirmation = true"
                  >Login above to checkout! ({{total}})</n-button>
                </div>
                <div class="row">
                  <div class="col-sm-3"></div>
                  <div class="col-sm-3">
                    <select v-model="selected" class="form-control">
                      <option
                        v-for="option in options"
                        v-bind:value="option"
                        v-bind:key="option"
                      >{{ option }}</option>
                    </select>
                  </div>
                  <div class="col-sm-2">
                    <label>Select your Server</label>
                  </div>
                </div>
                <div v-if="canCheckout">
                  <n-button
                    type="btn btn-primary"
                    @click="claimItem"
                    class="btn btn-primary"
                  >Purchase ({{total}})</n-button>
                </div>
                <div v-if="isAdmin">
                  <n-button type="btn btn-primary" @click="saveLootbox">Save Lootbox ({{total}})</n-button>
                </div>
              </b-col>
            </b-row>
          </b-container>
          <b-container v-if="cart.length == 0" class="no-results">
            <h2>Oh, no! It looks like your shopping cart is empty ...</h2>
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
        </div>
      </div>

      <modal
        id="responseSummaryModal"
        :show.sync="modals.claimConfirmation"
        headerClasses="justify-content-center"
      >
        <h4 slot="header" class="title title-up">
         <!-- Claim Summary -->
         Success
          </h4>
          Your items have been delivered to your inventory!
          <!-- Check the summary of your Loot delivery below. Don't worry if you see an error below, we don't deduct points until a confirmed delivery. 
          Please ensure you are logged into steam, you have a valid shopping cart, and have an available inventory in-game and try again late. 
          If issues persist, contact an administrator in -->
        <!-- <a
          href="https://discord.gg/dVGCNu"
        >Discord</a> -->
        <br />
        <br />
        <b-container>
          <div v-if="claimResponse.dinosaurs && claimResponse.dinosaurs.length > 0">
            <b-row>
              <b-col>Dino</b-col>
              <b-col>Delivered</b-col>
              <b-col>Chance</b-col>
              <b-col>Roll</b-col>
              <b-col>Status</b-col>
            </b-row>
            <br />
            <br />
            <b-row v-for="response in claimResponse.dinosaurs" :key="response.name">
              <b-col>{{response.name}}</b-col>
              <b-col>{{response.amount}}</b-col>
              <b-col>{{response.chanceAsPercentage}}</b-col>
              <b-col>{{response.theRoll}}</b-col>
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
              <b-col>Chance</b-col>
              <b-col>Roll</b-col>
              <b-col>Status</b-col>
            </b-row>
            <br />
            <br />
            <b-row v-for="response in claimResponse.items" :key="response.name">
              <b-col>{{response.name}}</b-col>
              <b-col>{{response.amount}}</b-col>
              <b-col>{{response.chanceAsPercentage}}</b-col>
              <b-col>{{response.theRoll}}</b-col>
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
          <div v-if="claimResponse.lootboxes && claimResponse.lootboxes.length > 0">
            <b-row>
              <b-col>Lootbox</b-col>
              <b-col>Delivered</b-col>
              <b-col>Chance</b-col>
              <b-col>Roll</b-col>
              <b-col>Status</b-col>
            </b-row>
            <br />
            <br />
            <b-row v-for="response in claimResponse.lootboxes" :key="response.name">
              <b-col>{{response.name}}</b-col>
              <b-col>{{response.amount}}</b-col>
              <b-col>{{response.chanceAsPercentage}}</b-col>
              <b-col>{{response.theRoll}}</b-col>
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
</template>
<script>
import axios from "axios";
import Toasted from "vue-toasted";
import { Card, Button, Modal, FormGroupInput } from "@/components";
import { Popover, Tooltip, DatePicker } from "element-ui";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";

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
          total += this.cart[i].Price * this.cart[i].Amount;
        }
      }
      return parseInt(total);
    },
    canCheckout() {
      let total = 0;
      if (this.cart) {
        for (var i = 0; i < this.cart.length; i++) {
          total += this.cart[i].Price * this.cart[i].Amount;
        }
      }
      return this.$cookies.get("SteamId") && parseInt(total) > 0;
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
  data() {
    return {
      cart: undefined,
      modals: {
        checkoutConfirmation: false,
        claimConfirmation: false
      },
      claimResponse: [],
      selected: "",
      options: [],
      servers: []
    };
  },
  mounted() {
    this.cart = JSON.parse(this.$cookies.get("cart"));

    axios
      .post(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/server/All", {})
      .then(response => {
        this.errored = false;
        this.servers = response.data;
        this.options = this.servers.map(s => s.Name);

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
    claimItem: function(response) {
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
      let server = this.servers.filter(obj => {
        return obj.Name == this.selected;
      });
      for (var i = 0; i < cart.length; i++) {
        if (cart[i].LootType === "Dinosaur") dinos.push(cart[i]);
        else if (cart[i].LootType === "Item") items.push(cart[i]);
        else if (cart[i].LootType === "Lootbox") lootboxes.push(cart[i]);
      }

      var request = {};
      this.user = this.$cookies.get("SteamId");
      axios
        .post(
          process.env.VUE_APP_ARKFURY_API_ENDPOINT +
            "/Inventory/" +
            this.user +
            "/Add",
          {
            Items: items,
            Dinosaurs: dinos,
            Lootboxes: lootboxes,
            Server: server[0]
          }
        )
        .then(response => {
          if (response.data.error && response.data.error.length > 0)
          {
            alert(response.data.error)
            return;
          }
console.log(response.data);
          this.claimResponse = response.data;
          this.modals.claimConfirmation = true;
          let cart = JSON.parse(this.$cookies.get("cart"));

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

          this.$cookies.set("cart", JSON.stringify(newCart));
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
      let cart = JSON.parse(this.$cookies.get("cart"));

      let newCart = [];
      for (var i = 0; i < cart.length; i++) {
        if (cart[i].LootType === "Dinosaur") {
          dinos.push(cart[i]);
        } else if (cart[i].LootType === "Item") {
          items.push(cart[i]);
        } else if (cart[i].LootType === "Lootbox") {
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
          let cart = JSON.parse(this.$cookies.get("cart"));

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
    },
    objectQueryString: function(theObject) {
      let y = Object.keys(theObject)
        .map(key => key + "=" + theObject[key])
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