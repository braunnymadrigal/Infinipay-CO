<template>
  <HeaderCompany/>
  <MainFooter/>
</template>

<script>
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";
export default {
  components: {
    HeaderCompany,
    MainFooter
  }
};
</script>

<style>
  @import '../assets/css/HeaderFooter.css';
</style>