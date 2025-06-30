<template>
  <footer
    class="footer"
    :class="{ [`footer-${type}`]: type }"
    :data-background-color="backgroundColor"
  >
    <div class="container">
      <nav>
        <ul>
          <li>
            <router-link to="/">About Us</router-link>
          </li>

             <li>
            <router-link to="/servers"> Servers</router-link>
          </li>
          <li>
            <a href="https://www.trackyserver.com/ark-server"><img alt="Ark server" src="https://www.trackyserver.com/pages/images/logo4.png"></a>	
          </li>
        </ul>
      </nav>
      <div class="copyright">&copy; {{ year }}, ARK Fury </div>
    </div>
  </footer>
</template>
<script>
export default {
  props: {
    backgroundColor: String,
    type: String
  },
  data() {
    return {
      year: new Date().getFullYear()
    };
  }
};
</script>
<style></style>
