<template>
  <div>
    <div class="page-header clear-filter" filter-color="orange">
      <parallax class="page-header-image" style="background-image:url('img/bg5.jpg')"></parallax>
      <div class="container fluid">
        <b-container class="bv-example-row">
          <b-row>
            <b-col sm="1" offset="7">Search...</b-col>
            <b-col sm="3">
              <b-input v-model="searchText" v-on:keyup="itemTypeChecked($event)">Enter Name</b-input>
            </b-col>
          </b-row>
          <b-row>
            <b-col sm="auto" lg="2" xl="2" style="text-align: left;">
              <div v-for="itemType in itemTypes" :key="itemType">
                <input
                  type="checkbox"
                  class="itemTypes"
                  :value="itemType"
                  v-model="filterList"
                  @change="itemTypeChecked($event)"
                />
                {{itemType}}
              </div>
            </b-col>
            <b-col sm="auto" lg="10" xl="10">
              <b-container>
                <div v-for="(deck) in decks" :key="deck" class="resultContainer">
                  <b-row>
                    <div v-for="(card) in deck" :key="card">
                      <b-row class="text-center">
                        <b-col>
                          <p style="font-size:large;">
                            <b>{{card.Name}} x {{card.StackSize}}</b>
                            <br />
                            Level {{card.Level}}
                          </p>
                          <p>
                            <b>{{card.Price}} Points {{card.LootType}}</b>
                          </p>
                        </b-col>
                      </b-row>
                      <b-col>
                        <img :src="card.Image" />
                        <b-button
                          @click="addToCart($event.toElement.id)"
                          :id="card.Name"
                          class="addBtn btn-primary btn"
                        >Add</b-button>
                        <b-col>
                          <b-row>
                            <b-col sm="6" offset="3">
                              <input class="form-control"  min="0" type="number" v-model="card.Amount" />
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

export default {
  data() {
    return {
      perPage: 9,
      currentPage: 1,
      items: undefined,
      itemTypes: undefined,
      searchText: "",

      filterList: [],
      boxedItems: undefined,
      status
    };
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
      .get(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/Dinosaur?Sort=-IsActive,+Name&IncludeInActive=false&" + this.jsonPaging())
      .then(response => {
        this.errored = false;
        this.items = this.boxedItems = response.data;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));

    axios
      .get(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/Dinosaur/Types?" + this.jsonPaging())
      .then(response => {
        this.errored = false;
        this.itemTypes = response.data;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));
  },
  methods: {
    itemTypeChecked: function(args) {
      let filteredItems = [];
      if (this.filterList.length === 0 && this.searchText.length === 0) {
        this.items = this.boxedItems;
      } else {
        for (var i = 0; i < this.boxedItems.length; i++) {
          if (
            this.searchText.length > 0 &&
            this.boxedItems[i].Name.toLowerCase().indexOf(
              this.searchText.toLowerCase()
            ) !== -1
          )
            filteredItems.push(this.boxedItems[i]);
          {
            for (var j = 0; j < this.filterList.length; j++) {
              if (this.filterList[j] == this.boxedItems[i].Type)
                filteredItems.push(this.boxedItems[i]);
            }
          }
        }
        this.items = filteredItems;
      }
    },
    search: function(args) {
      let filteredItems = [];
      if (this.searchText.length === 0) {
        this.items = this.boxedItems;
      } else {
        for (var i = 0; i < this.boxedItems.length; i++) {
          if (
            this.boxedItems[i].Name.toLowerCase().indexOf(
              this.searchText.toLowerCase()
            ) !== -1
          )
            filteredItems.push(this.boxedItems[i]);
        }

        this.items = filteredItems;
      }
    },
    addToCart: function(args) {


       this.$toasted.show("Dinosaur added to cart!!", {
        theme: "toasted-primary",
        position: "bottom-right",
        duration: 5000
      });


  let cart = JSON.parse(this.$cookies.get("cart"));

      if (!cart)
    cart = new Array();


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
          Level: xl.Level,
          BlueprintPath: xl.BlueprintPath,
          Id: xl.Id

        };
        cart.push(dto);
        document.getElementById("cartItemCount").innerText = parseInt(document.getElementById("cartItemCount").innerText) + 1;
      }
      this.$cookies.set("cart", JSON.stringify(cart));
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
</style>
