<template>
    <div>

    </div>

    <MainFooter/>
</template>

<script>
import MainFooter from "./MainFooter.vue";
export default {
  components: {
    MainFooter
  },
    data() {
      return {
        user: { userId: "", userPassword: "" },
      };
    },
    methods: {
      startLogin() {
        console.log("Saved:", this.user);
      },
    },
  };
</script>

<style lang="scss" scoped>

</style>