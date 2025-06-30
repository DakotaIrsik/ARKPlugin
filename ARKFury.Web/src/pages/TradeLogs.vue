<template>
  <div>
    <div class="page-header clear-filter" filter-color="orange">
      <parallax class="page-header-image" style="background-image:url('img/bg5.jpg')"></parallax>
      <b-col class="container fluid">
        <b-container>
            <b-container class="headerContainer">
              <b-row>
                <b-col>Id</b-col>
                <b-col>BluePrint</b-col>
                <b-col>Price</b-col>
                <b-col>Amount</b-col>
                <b-col>CreatedOn</b-col>
                <b-col>From</b-col>
                <b-col>SteamId</b-col>
              </b-row>
              <br />
              <br />
              <br />
              <br />
              <div class="resultContainer">
                <div v-for="item in pageItems" :key="item">
                  <b-row>
                    <b-col>{{item.id}}</b-col>
                    <b-col>{{item.blueprint}}</b-col>
                    <b-col>{{item.price}}</b-col>
                    <b-col>{{item.amount}}</b-col>
                    <b-col>{{item.createDate}}</b-col>
                    <b-col>{{item.from}}</b-col>
                    <b-col>{{item.steamId}}</b-col>
                  </b-row>
                </div>
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
        </b-container>
      </b-col>
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
      if (this.items && this.items.data) {
        if (this.currentPage === 1)
          return this.items.data.slice(0, this.perPage);
        else
          return this.items.slice(
            this.perPage * this.currentPage - this.perPage,
            this.perPage * this.currentPage
          );
      }
    }
  },
  mounted() {
    axios
      .post(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/Trade/Logs", {
        Size: 1500,
        From: 0,
        Sort: "-CreateDate",
        Fields: "",
        includeInactive: true
      })
      .then(response => {
        this.errored = false;
        this.items = this.boxedItems = response.data;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));
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
.headerContainer {
  color: #fff;
}
.resultContainer {
  overflow-y: auto !important;
  height: 800px;

  color: #fff;
}
</style>
