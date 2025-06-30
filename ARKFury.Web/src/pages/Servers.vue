<template>
  <div>
    <div class="page-header clear-filter" filter-color="orange">
      <parallax class="page-header-image" style="background-image:url('img/bg5.jpg')"></parallax>
      <div class="container fluid">
        <b-container class="bv-example-row">
          <b-row>
            <b-col sm="auto" lg="10" xl="10">
              <b-container>
                    <div v-for="(card) in items" :key="card">
                      <b-row class="text-center">
                        <b-col>
                          <p style="font-size:large;">
                       <a :href="steamUrl(card.Url, card.QueryPort)">{{steamUrl(card.Url, card.QueryPort)}}</a>
                          </p><br />
                          <img :src="serverStatusImage(card.IPAddress, card.QueryPort)" />
                        </b-col>
                      </b-row>
                    </div>
                    <b-col
                      style="padding-top: 50px;"
                      sm="12"
                      offset-sm="2"
                      md="12"
                      offset-md="4"
                      lg="12"
                      offset-lg="4">
                      <b-pagination
                        v-model="currentPage"
                        :total-rows="rows"
                        :per-page="perPage"
                        aria-controls="my-table"
                      ></b-pagination>
                    </b-col>
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
     .post(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/Server/All", {
        Size: 1500,
        From: 0,
        Sort: "+Name",
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
    steamUrl(url, port) {
      return "steam://connect/" + url + ":" + port;
    },
    serverStatusImage(ip, port) {
        return "https://toparkservers.com/banner/" + ip + ":" + port + "/banner.png"
    }
  }
};
</script>
<style scoped>
.addBtn {
  margin-left: 25px;
}

a {
    color: #ffffff !important;
}
</style>
