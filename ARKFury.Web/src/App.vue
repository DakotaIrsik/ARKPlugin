<template>
  <div id="app">
    <router-view name="header" />
    <div class="wrapper">
<iframe src="https://discordapp.com/widget?id=680544052978122783&theme=dark" style="position: absolute; margin-left: 85%; margin-top:10%;" width="250" height="500" allowtransparency="true" frameborder="0"></iframe>
          <router-view />

<crate></crate>


    </div>
    <router-view name="footer" />
  </div>
</template>
<script>
export default {
  data() {
    return {
    crate: "",
    widgetbot: ""
    }
  },
  
    mounted () {
    this.$loadScript("https://cdn.jsdelivr.net/npm/@widgetbot/crate@3")
      .then(() => {
        // eslint-disable-next-line no-undef
        this.crate = new Crate({
            server: '680544052978122783',
            channel: '683193366300852247',
            shard: 'https://e.widgetbot.io'
        });
      })
      .catch(() => {
      // Failed to fetch script
      });
  },
  destroyed () {
    this.crate.hide();
  },
};
</script>
<style>
.content-center {
  padding-top: 33% !important;
}
</style>