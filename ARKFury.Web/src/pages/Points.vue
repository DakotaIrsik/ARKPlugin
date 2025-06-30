<template>
  <div>
    <div class="page-header clear-filter" filter-color="orange">
      <parallax class="page-header-image" style="background-image:url('img/bg5.jpg')"></parallax>
      <div class="container"></div>
    </div>
    <div class="section">
      <div class="container">
        <h3 class="title">Shopping Cart - Points 2</h3>
        <h4 class="title">New Server Pricing: 1000 points = $1.00 USD</h4>
        <br />
        <div class="row">
          <div class="col-sm-3"></div>
          <div class="col-sm-3">
            <input
              type="number"
              v-model="ammount"
              id="ammount"
              class="form-control"
              placeholder="Amount"
            />
          </div>
          <div class="col-sm-2">
            <label>x 1000 pts = {{this.ammount * 1000}} pts</label>
          </div>
        </div>
        <div class="row h-100 justify-content-center align-items-center">
          <h4 v-show="ammount > 0">${{this.ammount}}.00</h4>
        </div>
        <br />
        <div class="row">
          <div class="col-sm-3"></div>
          <div class="col-sm-3">
            <select v-model="selected" class="form-control">
              <option
                v-for="option in options"
                v-bind:value="option"
                v-bind:key="option"
              >{{ option }}</option>
            </select>
          </div>
          <div class="col-sm-2">
            <label>Select your Server</label>
          </div>
        </div>
        <br />
        <br />
         <div class="row h-100 justify-content-center align-items-center" v-show="!canPurchase">
           <p>Sign in, select a server, and choose a valid quantity to check out.</p>
         </div>
        <div class="row h-100 justify-content-center align-items-center" v-show="canPurchase">
          <PayPal
            id="payPalCtrl"
            v-bind:amount="ammount"
            currency="USD"
            env="sandbox"
            :client="paypal"
            v-on:payment-completed="paymentCompleted">
            </PayPal>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { Tabs, TabPane } from "@/components";
import PayPal from "vue-paypal-checkout";
import axios from "axios";

export default {
  name: "profile",
  bodyClass: "profile-page",
  authorized: "",
  completed: "",
  components: {
    Tabs,
    TabPane,
    PayPal
  },
  data() {
    return {
      SteamId: "",
      ammount: "0",
      paypal: {
        sandbox:
          "AWQwvzs3AeeFfYSaDv8mv57i6hzwuD8_0Y4hWFUw_1rFESLXnUPsXrXTzKeQfVdmNOHS0FRJaR8SIBhC",
        production: "",
        braintree: window.braintree
      },
      selected: "",
      options: []
    };
  },
  computed: {
      canPurchase() {
      return this.selected && this.$cookies.get("SteamId") && this.ammount > 0;
    }
  },
  mounted() {
    const script = document.createElement("script");
    script.src = "https://js.braintreegateway.com/web/3.11.0/js/client.min.js";
    document.body.appendChild(script);

    const script2 = document.createElement("script2");
    script2.src =
      "https://js.braintreegateway.com/web/3.11.0/js/paypal-checkout.min.js";
    document.body.appendChild(script2);

    axios
      .get(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/server/names")
      .then(response => {
        this.errored = false;
        this.options = response.data;
      })
      .catch(() => (this.errored = true))
      .finally(() => (this.loading = false));
  },
  methods: {
    paymentCompleted: function(data) {
      axios
        .post(process.env.VUE_APP_ARKFURY_API_ENDPOINT + "/paypal/Complete", {
          OrderID: data.cart,
          Id: data.cart,
          Intent: data.intent,
          State: "Complete",
          ProductDescription:
            document.getElementById("ammount").value + "000 Points",
          Transactions: data.transactions,
          SteamId: this.$cookies.get("SteamId"),
          Server: this.selected
        })
        .then(response => {
          this.errored = false;
          this.$router.push({path: '/'})
                  document.getElementById("pointCount").innerText =
          parseInt(document.getElementById("pointCount").innerText) + parseInt(response.data);
          alert("Success! Return to game, to use your points.");
        })
        .catch(() => (this.errored = true))
        .finally(() => (this.loading = false));
    }
  }
};
</script>
<style></style>
