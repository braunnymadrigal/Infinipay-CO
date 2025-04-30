<template>
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
          <router-link to="/EmployerProfile" class="mx-2" style="color: #405d72"
            >Perfil</router-link
          >
          <a href="#" class="mx-2" style="color: #405d72">Empresa</a>
          <router-link to="/BenefitList" class="mx-2" style="color: #405d72"
            >Beneficios</router-link
          >
          <a href="#" class="mx-2" style="color: #405d72">Empleados</a>
        </div>
      </div>
    </nav>
  </header>
</template>
<script setup></script>
