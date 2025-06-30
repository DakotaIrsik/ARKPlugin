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
         <h2>Server Located... Redirecting you momentarily...</h2>
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
   mounted () {
     console.log(this.$route);
     this.$cookies.set('ServerIdentifier', this.$route.query["ServerIdentifier"].toString());
     window.location = "/"
  }
};
</script>
<style></style>
