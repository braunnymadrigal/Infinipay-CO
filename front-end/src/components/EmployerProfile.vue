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

  <footer class="py-5 custom-footer">
    <div class="container">
      <div class="row">
        <div class="col-md-3 mb-3">
          <p class="h5">Infinipay CO.</p>
          <div>
            <a href="#" class="fa fa-facebook"></a>
            <a href="#" class="fa fa-linkedin"></a>
            <a href="#" class="fa fa-youtube"></a>
            <a href="#" class="fa fa-instagram"></a>
          </div>
        </div>
        <div class="col-md-3 mb-3">
          <p class="h5">Empresa y equipo</p>
          <a href="#">Sobre nosotros</a>
        </div>
        <div class="col-md-3 mb-3">
          <p class="h5">Recursos</p>
          <a href="#">¿Cómo registro mi empresa?</a><br />
          <a href="#">¿Cómo registro empleados a mi empresa?</a><br />
          <a href="#">¿Cómo accedo a mi perfil?</a>
        </div>
        <div class="col-md-3 mb-3">
          <p class="h5">Contacto</p>
          <p><i class="pi pi-phone" style="color:#405D72;"></i>
            +506 2000-0000</p>
          <p><i class="pi pi-home" style="color:#405D72;"></i>
            San José, Montes de Oca, San Pedro</p>
        </div>
      </div>
    </div>
  </footer>
</template>

<style>
  @import '../assets/css/HeaderFooter.css';
</style>