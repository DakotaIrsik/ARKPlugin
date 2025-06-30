<template>
  <div>
    <div class="page-header clear-filter" filter-color="orange">
      <parallax class="page-header-image" style="background-image:url('img/bg5.jpg')"></parallax>
      <div class="container"></div>
    </div>
    <div class="section">
      <div class="container">
        <h3 class="title">Shopping Cart - Packages</h3>
        <h4 class="title">Coming Soon</h4>
      </div>
    </div>
  </div>
</template>
<script>
import { Tabs, TabPane } from "@/components";

export default {
  name: "profile",
  bodyClass: "profile-page",
  components: {
    Tabs,
    TabPane
  },
  data() {
    return {
      ammount: 0
    };
  },
  mounted() {
    paypal.Button.render(
      {
        env: "sandbox",
        client: {
          sandbox:
            "AWQwvzs3AeeFfYSaDv8mv57i6hzwuD8_0Y4hWFUw_1rFESLXnUPsXrXTzKeQfVdmNOHS0FRJaR8SIBhC",
          production:
            "AWQwvzs3AeeFfYSaDv8mv57i6hzwuD8_0Y4hWFUw_1rFESLXnUPsXrXTzKeQfVdmNOHS0FRJaR8SIBhC"
        },

        locale: "en_US",
        style: {
          size: "medium",
          color: "gold",
          shape: "pill"
        },

        commit: true,

        payment: function(data, actions) {
          return actions.payment.create({
            transactions: [
              {
                amount: {
                  total: ammount,
                  currency: "USD"
                }
              }
            ]
          });
        },

        onAuthorize: function(data, actions) {
          return actions.payment.execute().then(function() {
            window.alert("Thank you for your purchase!");
          });
        }
      },
      "#paypal-button"
    );
  }
};
</script>
<style></style>
