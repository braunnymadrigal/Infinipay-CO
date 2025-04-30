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
            <router-link to="/LoginUser" class="btn btn-outline-primary me-2"
              style="background-color: #405D72; color: #FFFFFF;
              border: transparent;"> Iniciar sesión</router-link>
            <router-link to="/RegisterEmployer" class="btn btn-primary"
              style="background-color: #405D72;
              border: transparent;">Registrá tu empresa
          </router-link>
          </div>
          <div class="ms-auto">
            <router-link to="/" class="btn btn-primary me-2"
              style="background-color: #F7E7DC; color: #2b3f4e;
                border: 2px solid transparent;">Página Principal
            </router-link>
            <router-link to="/" class="btn btn-secondary"
              style="background-color: #F7E7DC; color: #2b3f4e;
                border: 2px solid transparent;">¿Necesitás ayuda?
            </router-link>
          </div>
        </div>
      </nav>
    </header>

    <div class="position-relative">
      <img src="../assets/images/AboutUs/image1.jpeg" alt="Image"
        class="img-fluid w-100" style="height: 60vh; object-fit: cover;" />
      <div class="position-absolute top-0 left-0 w-100 h-100 bg-dark"
        style="opacity: 0.4;"></div>
      <div class="position-absolute top-50 start-50 translate-middle
        text-center text-white">
        <h1>Sobre Nosotros</h1>
        <h2>
          Simplificamos el proceso, vos solo tenés que enfocarte en hacer
          crecer tu empresa.
        </h2>
      </div>
    </div>

    <p class="text-center my-5" style="color: #405D72; font-size: 1.2rem;
      line-height: 1.5;">
      Infinipay CO está dirigido a emprendedores y pymes en Costa Rica para
      automatizar la gestión de planillas, asegurando el cálculo preciso de
      salarios, deducciones y beneficios según la legislación vigente.
    </p>

    <h1 class="text-center" style="color: #405D72; margin-top: 50px;">
      ¿Por qué elegir Infinipay CO?</h1>

      <div class="container my-5">
        <div class="row justify-content-center">
          <div class="col-md-5 mb-">
            <div>
              <h5 style="color: #405D72;">Solución automatizada para Costa Rica
              </h5>
              <p style="text-align: justify;">Plataforma web que simplifica la
                gestión de planillas, asegurando el cálculo correcto de
                salarios, deducciones y beneficios según la legislación
                costarricense.</p>
            </div>
          </div>
          <div class="col-md-5 mb-4">
            <div>
              <h5 style="color: #405D72;">Cumplimiento legal garantizado</h5>
              <p style="text-align: justify;">Aplicamos las deducciones
                obligatorias de ley como el CCSS, impuesto sobre la renta y
                asociaciones solidaristas, con cálculos basados en fuentes
                oficiales.</p>
            </div>
          </div>
        </div>
        <div class="row justify-content-center">
          <div class="col-md-5 mb-4">
            <div>
              <h5 style="color: #405D72;">Optimización para emprendedores y
                pymes</h5>
              <p style="text-align: justify;">Diseñada para dueños de negocios
                que necesitan gestionar pagos sin depender de terceros,
                reduciendo errores y ahorrando tiempo.</p>
            </div>
          </div>
          <div class="col-md-5 mb-4">
            <div>
              <h5 style="color: #405D72;">Reportes claros y completos</h5>
              <p style="text-align: justify;">Generamos informes detallados
                sobre salarios brutos, deducciones y pagos, brindando control
                total sobre la planilla y las obligaciones fiscales.</p>
            </div>
          </div>
        </div>
      </div>


    <h1 class="text-center my-5" style="color: #405D72;">Nuestro equipo</h1>
    <div class="container">
      <div class="row justify-content-center">
        <div class="col-md-3 mb-4">
          <div class="card text-center" style="min-height: 182px;
            display: flex; flex-direction: column;
            justify-content: space-between;">
            <img src="../assets/images/HomePage/Reviews/kirby_review.png"
              alt="Persona 1" class="card-img-top rounded-circle mx-auto mt-3"
              style="width: 80px; height: 80px; object-fit: cover;">
            <div class="card-body">
              <h5 class="card-title" style="margin-bottom: 5px;">
                Daniel Gómez Solís</h5>
            </div>
          </div>
        </div>
        <div class="col-md-3 mb-4">
          <div class="card text-center" style="min-height: 182px; display: flex;
             flex-direction: column; justify-content: space-between;">
            <img src="../assets/images/HomePage/Reviews/sylveon_review.jpg"
              alt="Persona 2" class="card-img-top rounded-circle mx-auto mt-3"
              style="width: 80px; height: 80px; object-fit: cover;">
            <div class="card-body">
              <h5 class="card-title" style="margin-bottom: 5px;">
                María Fernanda Herrera Gutiérrez</h5>
            </div>
          </div>
        </div>
        <div class="col-md-3 mb-4">
          <div class="card text-center" style="min-height: 182px; display: flex;
            flex-direction: column; justify-content: space-between;">
            <img src="../assets/images/HomePage/Reviews/fallguys_review.jpg"
              alt="Persona 3" class="card-img-top rounded-circle mx-auto mt-3"
              style="width: 80px; height: 80px; object-fit: cover;">
            <div class="card-body">
              <h5 class="card-title" style="margin-bottom: 5px;">
                Braunny Madrigal Barrantes</h5>
            </div>
          </div>
        </div>
        <div class="col-md-3 mb-4">
          <div class="card text-center" style="min-height: 182px; display: flex;
            flex-direction: column; justify-content: space-between;">
            <img src="../assets/images/HomePage/Reviews/fallguys_review.jpg"
              alt="Persona 4" class="card-img-top rounded-circle mx-auto mt-3"
              style="width: 80px; height: 80px; object-fit: cover;">
            <div class="card-body">
              <h5 class="card-title" style="margin-bottom: 5px;">
                Fabián Mora Arias</h5>
            </div>
          </div>
        </div>
      </div>
    </div>
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
  label {
    display: block;
    text-align: left;
    margin-bottom: 0.5rem;
  }
  @import '../assets/css/HeaderFooter.css';
</style>