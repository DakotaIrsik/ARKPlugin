<template>
  <div>
    <div class="page-header clear-filter" filter-color="orange">
      <parallax class="page-header-image" style="background-image:url('img/bg5.jpg')"></parallax>
      <div class="container fluid">
        <modal :show.sync="modals.lootBoxInfo" headerClasses="justify-content-center">
          <h4 slot="header" id="lootboxInfoModalHeader" class="title title-up"></h4>
          <p id="lootboxInfoModalBody"></p>
          <br />
        </modal>

        <b-container class="bv-example-row">
          <b-row>
            <b-col sm="auto" lg="10" xl="10">
              <b-container>
                <div v-for="(deck) in decks" :key="deck" class="resultContainer">
                  <b-row>
                    <div v-for="(card) in deck" :key="card.Id">
                      <b-row class="text-center">
                        <b-col>
                          <p style="font-size:large;">
                            <b>{{card.Name}} x {{card.StackSize}}</b>
                          </p>
                          <p>
                            <b>{{card.Price}} Points</b>
                          </p>
                        </b-col>
                      </b-row>
                      <b-col>
                        <img style="width: 100px;" :src="card.Image" />
                        <b-button
                          @click="addToCart($event.toElement.id)"
                          :id="card.Name"
                          class="addBtn btn-primary btn"
                        >Add</b-button>

                        <b-button
                          @click="moreInfo($event.toElement.id)"
                          :id="card.Name"
                          class="addBtn btn-primary btn"
                        >More Info</b-button>
                        <b-col>
                          <b-row>
                            <b-col sm="6" offset="3">
                              <input
                                class="form-control"
                                min="0"
                                type="number"
                                v-model="card.Amount"
                              />
                            </b-col>
                          </b-row>
                        </b-col>
                      </b-col>
                    </div>
                  </b-row>
                </div>
                <b-row>
                  <b-col sm="4" offset="3" style="padding-top: 50px;">
                    <b-pagination
                      v-model="currentPage"
                      :total-rows="rows"
                      :per-page="perPage"
                      aria-controls="my-table"
                    ></b-pagination>
                  </b-col>
                </b-row>
              </b-container>
            </b-col>
          </b-row>
        </b-container>
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
// Import stylesheet
import "vue-loading-overlay/dist/vue-loading.css";

export default {
  data() {
    return {
      perPage: 9,
      currentPage: 1,
      items: undefined,
      searchText: "",

      filterList: [],
      boxedItems: undefined,
      status,
      modals: {
        lootBoxInfo: false
      }
    };
  },
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
    rows() {
      if (this.items) return this.items.length;
      return 0;
    },
    pageItems() {
      if (this.items) {
        if (this.currentPage === 1) return this.items.slice(0, this.perPage);
        else
          return this.items.slice(
            this.perPage * this.currentPage - this.perPage,
            this.perPage * this.currentPage
          );
      }
    },
    decks() {
      if (this.pageItems) {
        let howMany = this.perPage / 5;
        var idx = 0;
        let result = [];

        while (idx < this.pageItems.length) {
          if (idx % howMany === 0) result.push([]);
          result[result.length - 1].push(this.pageItems[idx++]);
        }
        return result;
      }
    }
  },
  mounted() {
    axios
      .post(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/Lootbox/All", {
        Size: 1500,
        From: 0,
        Sort: "+Price",
        Fields: ""
      })
      .then(response => {
        this.errored = false;
        this.items = this.boxedItems = response.data;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));
  },
  methods: {
    addToCart: function(args) {
      //this.$cookies.remove("cart");

      if (this.boxedItems.find(b => b.Name == args).Amount == 0) return;

      this.$toasted.show("Item added to cart!!", {
        theme: "toasted-primary",
        position: "bottom-right",
        duration: 5000
      });

      if (!this.$cookies.get("cart")) {
        this.$cookies.set("cart", JSON.stringify(new Array()));
      }

      let cart = JSON.parse(this.$cookies.get("cart"));

      if (cart.find(c => c.Name === args)) {
        cart.find(c => c.Name === args).Amount =
          parseInt(cart.find(c => c.Name === args).Amount) +
          parseInt(this.boxedItems.find(b => b.Name == args).Amount);
      } else {

        let xl = this.boxedItems.find(b => b.Name == args);
        let dto = {
          Name: xl.Name,
          Price: xl.Price,
          Amount: xl.Amount,
          Image: xl.Image,
          LootType: xl.LootType,
          Quality: xl.Quality,
          Level: xl.Level
        };
        cart.push(dto);
        document.getElementById("cartItemCount").innerText = parseInt(document.getElementById("cartItemCount").innerText) + 1;
      }
      this.$cookies.set("cart", JSON.stringify(cart));

      document.getElementById("cartItemCount").html =
        parseInt(document.getElementById("cartItemCount").html) + 1;
    },
    moreInfo: function(name) {
      let box = this.boxedItems.find(b => b.Name === name);
      document.getElementById("lootboxInfoModalHeader").innerText = box.Name;
      document.getElementById("lootboxInfoModalBody").innerHTML = "";
      for (var i = 0; i < box.Description.length; i++)
        document.getElementById("lootboxInfoModalBody").innerHTML += box.Description[i] + "<br />";

      this.modals.lootBoxInfo = true;
    }
  }
};
</script>
<style scoped>
.addBtn {
  margin-left: 25px;
}
.page-header .container {
  min-height: 1400px;
}

.row {
  margin: 0;
}

p {
  color: #fff;
}
.resultContainer {
  overflow-y: auto !important;
  height: 1100px;
}

.modal-dialog p {
  color: #000000;
}
</style>
