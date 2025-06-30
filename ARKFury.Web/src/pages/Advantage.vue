<template>
  <div>
    <div class="page-header clear-filter" filter-color="orange">
      <parallax class="page-header-image" style="background-image:url('img/bg5.jpg')"></parallax>
      <div class="container fluid">
        <h3 class="title">Advantage</h3>
        <div>
          <b-tabs content-class="mt-3" justified>
            <b-tab title="Tribe" active>
              <div></div>
            </b-tab>
            <b-tab title="Attack">
              <div></div>
            </b-tab>
            <b-tab title="Defense">
              <div></div>
            </b-tab>
            <b-tab title="Dinosaurs">
              <div v-if="this.dinosaurs">
                <b-row>
                  <b-col>Name</b-col>
                  <b-col></b-col>
                </b-row>
                <div v-for="(item) in this.dinosaurs" :key="item.Id">
                  <b-row>
                    <b-col>
                      <p>{{item.Name}}</p>
                    </b-col>
                    <b-col>
                      <b-button
                        @click="whistleDino($event.toElement.id)"
                        :id="item.DinosaurId1"
                        class="addBtn btn-primary btn"
                      >Whistle</b-button>
                    </b-col>
                  </b-row>
                </div>
                  <b-button
                        @click="whistleAll()"
                        class="addBtn btn-primary btn"
                        size="xl"
                      >Whistle All</b-button>
              </div>
            </b-tab>
          </b-tabs>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { Tabs, TabPane } from "@/components";
import axios from "axios";

export default {
  name: "profile",
  bodyClass: "profile-page",
  components: {
    Tabs,
    TabPane
  },
  data() {
    return {
      response: undefined,
      dinosaurs: undefined
    };
  },
  mounted() {
    this.user = this.$cookies.get("SteamId");

    axios
      .get(
        process.env.VUE_APP_ARKFURY_API_ENDPOINT +
          "/Player/" +
          this.user +
          "/Dinosaurs?" +
          this.jsonPaging()
      )
      .then(response => {
        this.errored = false;
        this.dinosaurs = response.data;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));
  },
  methods: {
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
    whistleDino: function(args) {
      //this.$cookies.remove("cart");
      let theDino = this.dinosaurs.find(d => d.DinosaurId1 == args);
       axios
      .post(
        process.env.VUE_APP_ARKFURY_API_ENDPOINT +
          "/Player/" +
          this.user +
          "/Dinosaurs/" +
          theDino.DinosaurId1 + "/" +
          theDino.DinosaurId2 +
          "/Call",
          this.jsonPaging()
      )
      .then(response => {
        this.errored = false;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));
    },
      whistleAll: function(args) {
       axios
      .post(
        process.env.VUE_APP_ARKFURY_API_ENDPOINT +
          "/Player/" +
          this.user +
          "/WhistleAll"
      )
      .then(response => {
        this.errored = false;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));
    }
  }
};
</script>
<style>
.page-header .container {
  min-height: 1400px;
}
</style>
