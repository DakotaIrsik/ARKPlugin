<template>
  <div>
    <div class="page-header clear-filter" filter-color="orange">
      <parallax
        class="page-header-image"
        style="background-image:url('img/bg5.jpg')"
      >
      </parallax>
      <div class="container">

      </div>
    </div>
    <div class="section">
      <div class="container">
         <h2>Success. Redirecting you momentarily...</h2>
      </div>
    </div>
  </div>
</template>
<script>
import { Tabs, TabPane } from '@/components';
import axios from 'axios'

export default {
  name: 'profile',
  bodyClass: 'profile-page',
  components: {
    Tabs,
    TabPane
  },
   data() {
    return {
      response: this.$route.query["openid.identity"].toString().substring(37)
      
    };
  },
   mounted () {
     this.$cookies.set('SteamId', this.response);

          this.$cookies.set('ServerIdentifier', '7fe2a465-d191-449f-a6ff-cc1a103d8667');

// let si = this.$cookies.get("ServerIdentifier")
// if (!si || si == undefined)
// si = '7fe2a465-d191-449f-a6ff-cc1a103d8667' //this is our server id... TODO Make this db driven or something...

     axios.get(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/User/" + 
                 this.$cookies.get("SteamId") + "/Login/" + this.$cookies.get("ServerIdentifier")
      )
      .then(response => {
        this.errored = false;
        this.items = this.boxedItems = response.data;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));


     window.location = "/"
  }
};
</script>
<style></style>
