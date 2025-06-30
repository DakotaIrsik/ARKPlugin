<template>
  <div>
    <div class="page-header clear-filter" filter-color="orange">
      <parallax
        class="page-header-image"
        style="background-image:url('img/bg5.jpg')">
      </parallax>
      <div class="container">
      </div>
    </div>
    <div class="section">
      <div class="container">
        <h3 class="title">Rules</h3>
        <p>
         <pre>{{ this.response }}</pre>
        </p>
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
      response: undefined
    };
  },
   mounted () {
    axios.get(process.env.VUE_APP_ARKFURY_API_ENDPOINT + '/info').then(r => this.response = r.data);
  }
};
</script>
<style></style>
