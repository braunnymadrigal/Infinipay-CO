<template>
  <div class="container my-5">
    <div class="text-center mb-4">
      <img
        src="../assets/images/logo.png"
        alt="Company logo"
        class="img-fluid"
        style="max-width: 350px"
      />
    </div>
      <header class="mb-5 custom-header">
        <nav class="navbar navbar-expand-lg rounded custom-navbar">
          <div class="container-fluid">
            <div class="d-flex">
              <router-link
                to="/MyProfile"
                class="mx-2"
                style="color: #405d72"
                >Perfil</router-link
              >
              <router-link
                to="/MyCompany"
                class="mx-2"
                style="color: #405d72"
                >Empresa</router-link
              >
              <router-link to="/BenefitList" class="mx-2"
                style="color: #405d72">Beneficios</router-link
              >
              <router-link to="/EmployeeList" class="mx-2"
                style="color: #405d72">Empleados</router-link
              >
              <router-link
                v-if="rol === 'empleador'"
                to="/RegisterEmployee"
                class="mx-2"
                style="color: #405d72"
              >
                Registrar empleados
              </router-link>
            </div>
          </div>
        </nav>
      </header>
    </div>

</template>
<script>
export default {
  name: 'CompanyHeader',
  props: {
    rol: {
      type: String,
      required: false,
      default: ''
    }
  }
}
</script>
<style>
  @import '../assets/css/HeaderFooter.css';
</style>