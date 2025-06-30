<template>
  <div>
    <div class="page-header clear-filter" filter-color="orange">
      <parallax class="page-header-image" style="background-image:url('img/bg5.jpg')"></parallax>
      <div class="container fluid">
        <b-container>
          <b-row>
            <b-col cols="2" class="text-left" style="padding-top: 40px;">
              <div v-for="itemType in itemTypes" :key="itemType.Id">
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
            <b-col cols="10">
              <div>
                <b-container>
                  <b-row>
                    <b-col sm="1" offset="7">Search...</b-col>
                    <b-col sm="3">
                      <b-input v-model="searchText" v-on:keyup="itemTypeChecked($event)">Enter Name</b-input>
                    </b-col>
                  </b-row>
                  <div v-for="(deck) in decks" :key="deck" class="resultContainer">
                    <b-row cols="12">
                      <div
                        class="sm-4 md-4 lg-4"
                        v-for="(card) in deck"
                        :key="card"
                        style="width: 230px; padding: 5px;"
                      >
                        <b-row class="text-center">
                          <b-col>
                            <p style="font-size:large;">
                              <b class="w-100 text-truncate">{{card.Name}}</b>
                            </p>
                            <b-row>
                              <b-col>Price:</b-col>
                              <b-col cols="6">
                                <input
                                  class="form-control"
                                  min="0"
                                  type="number"
                                  v-model="card.Price"
                                />
                              </b-col>
                            </b-row>
                            <b-row>
                              <b-col>Quality:</b-col>
                              <b-col cols="6">
                                <select class="form-control" v-model="card.Quality">
                                  <option
                                    v-for="quality in itemQualities"
                                    :value="quality"
                                    :key="quality"
                                  >{{quality.Name}}</option>
                                </select>
                              </b-col>
                            </b-row>
                            <b-row>
                              <b-col>Stack Size:</b-col>
                              <b-col cols="6">
                                <input
                                  class="form-control"
                                  min="0"
                                  type="number"
                                  v-model="card.StackSize"
                                />
                              </b-col>
                            </b-row>
                            <b-row>
                              <b-col>Is Active:</b-col>
                              <b-col cols="5">
                                <b-checkbox v-model="card.IsActive" />
                              </b-col>
                            </b-row>
                          </b-col>
                        </b-row>
                        <b-col>
                          <img :src="card.Image" />
                          <b-button
                            @click="saveDinosaur($event.toElement.id)"
                            :id="card.Name"
                            class="addBtn btn-primary btn"
                          >Save</b-button>
                        </b-col>
                      </div>
                    </b-row>
                  </div>
                  <b-row>
                    <b-col sm="3" offset="3" style="padding-top: 50px;">
                      <b-pagination
                        v-model="currentPage"
                        :total-rows="rows"
                        :per-page="perPage"
                        aria-controls="my-table"
                      ></b-pagination>
                    </b-col>
                  </b-row>
                </b-container>
              </div>
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
      itemQualities: undefined,
      itemTypes: undefined,
      searchText: "",

      filterList: [],
      boxedItems: undefined,
      status,
      selectedQuality: ""
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
      .get(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/Item?Sort=-IsActive,+Name&IncludeInActive=true&" + this.jsonPaging())
      .then(response => {
        this.errored = false;
        this.items = this.boxedItems = response.data;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));


    axios
      .get(
        process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/ItemQuality/All?" + this.jsonPaging()
      )
      .then(response => {
        this.errored = false;
        this.itemQualities = response.data;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));

    axios
      .get(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/Item/Types?" + this.jsonPaging())
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
    saveDinosaur: function(args) {
      let dino = this.boxedItems.find(b => b.Name == args);
      this.$toasted.show("Item updated", {
        theme: "toasted-primary",
        position: "bottom-right",
        duration: 5000
      });

      axios
        .post(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/Item/Update", {
          Name: dino.Name,
          Type: dino.Type,
          StackSize: dino.StackSize,
          ItemId: dino.ItemId,
          Image: dino.Image,
          Price: dino.Price,
          Quality: dino.Quality,
          Amount: dino.Amount,
          Blueprint: dino.Blueprint,
          BlueprintPath: dino.BlueprintPath,
          Id: dino.Id,
          IsActive: dino.IsActive,
          Maps: dino.Maps
        })
        .then(response => {
          this.errored = false;
        })
        .catch(() => (this.errored = true))
        .finally(() => (this.loading = false));
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

select {
  background-color: #fff;
}
</style>
