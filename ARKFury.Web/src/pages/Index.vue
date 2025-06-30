<template>
  <div>
    <div class="page-header clear-filter" filter-color="orange">
      <parallax class="page-header-image padding=10px;" style="background-image:url('img/header.jpg')"></parallax>
      <div class="container">
        <b-row>
          <b-col>
            <div class="content-center brand">
              <h1>ARK Fury</h1>Play
              <br />Earn
              <br />Build
              <br />Survive
              <br />
            </div>
          </b-col>
        </b-row>
      </div>
    </div>
  </div>
</template>


<script>
import { Parallax } from "@/components";
import BasicElements from "./components/BasicElementsSection";
import Navigation from "./components/Navigation";
import TabsSection from "./components/Tabs";
import ProgressPagination from "./components/ProgressPagination";
import Notifications from "./components/Notifications";
import Typography from "./components/Typography";
import JavascriptComponents from "./components/JavascriptComponents";
import CarouselSection from "./components/CarouselSection";
import NucleoIconsSection from "./components/NucleoIconsSection";
import SignupForm from "./components/SignupForm";
import ExamplesSection from "./components/ExamplesSection";


export default {
  name: "index",
  bodyClass: "index-page",
  components: {
    Parallax,
    BasicElements,
    Navigation,
    TabsSection,
    ProgressPagination,
    Notifications,
    Typography,
    JavascriptComponents,
    CarouselSection,
    NucleoIconsSection,
    SignupForm,
    ExamplesSection
  },
  data() {
    return {
    cartJson: this.$cookies.get("cart")
    }
  },
};
</script>
<style>

</style>
