<template>
  <div class="container my-5">
    <div class="text-center mb-4">
      <img src="../assets/images/logo.png"
           alt="Company logo"
           class="img-fluid"
           style="max-width: 350px" />
    </div>
    <header class="mb-5 custom-header">
      <nav class="navbar navbar-expand-lg rounded custom-navbar">
        <div class="container-fluid">
          <div class="d-flex">
            <router-link to="/MyProfile"
                         class="mx-2"
                         style="color: #405d72">Perfil</router-link>
            <router-link to="/MyCompany"
                         class="mx-2"
                         style="color: #405d72">Empresa</router-link>
            <a @click="goToBenefits" class="mx-2" style="color: #405d72; cursor: pointer;">
              Beneficios
            </a>
            <a href="#" class="mx-2" style="color: #405d72">Empleados</a>
          </div>
        </div>
      </nav>
    </header>
  </div>

</template>
<script>
  import axios from "axios";
  export default {
    methods: {
      async goToBenefits() {
        try {
          const response = await axios.get("https://localhost:7275/api/Login/GetLoggedUser", {
            headers: {
              Authorization: `Bearer ${this.$cookies.get('jwt')}`
            }
          });

          const role = response.data.Role;

          if (role === 'empleado') {
            this.$router.push('/AssignedBenefitList');
          } else {
            this.$router.push('/BenefitList');
          }
        } catch (error) {
          console.error("Error fetching user role:", error);
          this.$router.push('/MyCompany');
        }
      }
    }

  }

</script>

<style>
  @import '../assets/css/HeaderFooter.css';
</style>