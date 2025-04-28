<template>
  <div class="container my-5">
    <div class="text-center mb-4">
      <img src="../assets/images/logo.png" alt="Company logo" class="img-fluid"
        style="max-width: 350px;">
    </div>
    
    <header class="mb-5 custom-header">
      <nav class="navbar navbar-expand-lg rounded custom-navbar">
        <div class="container-fluid">
          <div class="d-flex">
            <router-link to="/EmployerProfile" class="mx-2"
              style="color: #405D72;">Perfil</router-link>
            <a href="#" class="mx-2" style="color: #405D72;">Empresa</a>
            <a href="#" class="mx-2" style="color: #405D72;">Beneficios</a>
            <a href="#" class="mx-2" style="color: #405D72;">Empleados</a>
          </div>
          <div class="ms-auto">
            <router-link to="/RegisterEmployee" class="btn btn-primary me-2"
              style="background-color: #F7E7DC; color: #2b3f4e;
                border: 2px solid transparent;">Registrar empleado
            </router-link>
          </div>
        </div>
      </nav>
    </header>
  </div>
</template>

<style>
  @import '../assets/css/HeaderFooter.css';
</style>