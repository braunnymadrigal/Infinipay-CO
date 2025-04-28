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
            <a href="#" class="btn btn-outline-primary me-2"
              style="background-color: #405D72; color: #FFFFFF;
                border: transparent;"> Iniciar sesión</a>
            <router-link to="/RegisterEmployer" class="btn btn-primary"
              style="background-color: #405D72;
              border: transparent;">Registrá tu empresa
            </router-link>
          </div>
          <div class="ms-auto">
            <router-link to="/AboutUs" class="btn btn-primary me-2"
              style="background-color: #F7E7DC; color: #2b3f4e;
                border: 2px solid transparent;">Sobre nosotros
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
      <img src="../assets/images/HomePage/image1.jpg" alt="Image"
        class="img-fluid w-100" style="height: 60vh; object-fit: cover;" />
      <div class="position-absolute top-0 left-0 w-100 h-100 bg-dark"
        style="opacity: 0.4;"></div>
      <div class="position-absolute top-50 start-50 translate-middle
        text-center text-white">
        <h1 class="display-4">
          Simplificá la gestión de planillas de tus trabajadores</h1>
      </div>
    </div>

    <h1 class="text-center" style="color: #405D72; margin-top: 50px;">
      ¿Por qué elegirnos?</h1>
    <div class="container" style="margin-top: 100px;">
      <div class="row d-flex justify-content-center">
        <div class="col-md-3">
          <div class="mb-4">
            <div class="text-center">
              <h5 style="color: #405D72;">Accesibilidad</h5>
              <p style="text-align: justify;">Brindamos una alternativa
                accesible a pequeñas empresas que no pueden pagar servicios
                contables</p>
            </div>
          </div>
          <div class="mb-4">
            <div class="text-center">
              <h5 style="color: #405D72;">Automatización</h5>
              <p style="text-align: justify;">Evitamos errores humanos en el
                cálculo de salarios y deducciones</p>
            </div>
          </div>
          <div class="mb-4">
            <div class="text-center">
              <h5 style="color: #405D72;">Cumplimiento legal</h5>
              <p style="text-align: justify;">Nos aseguramos de que los
                empleadores cumplan con todas las normativas laborales y
                fiscales</p>
            </div>
          </div>
          <div class="mb-4">
            <div class="text-center">
              <h5 style="color: #405D72;">Optimización</h5>
              <p style="text-align: justify;">Reducimos la carga administrativa
                para que los emprendedores se enfoquen en hacer crecer su
                negocio</p>
            </div>
          </div>
        </div>

        <div class="col-md-4">
          <div class="my-5">
            <img src="../assets/images/HomePage/image2.webp" alt="Image"
              class="img-fluid mx-auto d-block" style="height: 480px;
              object-fit: cover; margin-top: -40px;" />
          </div>
        </div>
      </div>
    </div>

    <h1 class="text-center my-5" style="color: #405D72;">Reseñas</h1>
    <div class="container">
      <div class="row justify-content-center">
        <div class="col-md-4 mb-4">
          <div class="card text-center" style="min-height: 250px;
            display: flex; flex-direction: column;
            justify-content: space-between;">
            <img src="../assets/images/HomePage/Reviews/kirby_review.png"
              alt="Persona 1" class="card-img-top rounded-circle mx-auto mt-3"
              style="width: 80px; height: 80px; object-fit: cover;">
            <div class="card-body">
              <p class="card-text" style="font-style: italic;
                margin-bottom: 15px;">
                “¡Infinipay CO hace que pagar a los Waddle Dees sea pan comido!”
              </p>
              <h5 class="card-title" style="margin-bottom: 5px;">Kirby</h5>
              <p class="card-subtitle text-muted" style="font-size: 0.875rem;">
                Dream Land CO.</p>
            </div>
          </div>
        </div>
        <div class="col-md-4 mb-4">
          <div class="card text-center" style="min-height: 250px; display: flex;
             flex-direction: column; justify-content: space-between;">
            <img src="../assets/images/HomePage/Reviews/sylveon_review.jpg"
              alt="Persona 2" class="card-img-top rounded-circle mx-auto mt-3"
              style="width: 80px; height: 80px; object-fit: cover;">
            <div class="card-body">
              <p class="card-text" style="font-style: italic;
                margin-bottom: 15px;">
                “Gestionar pagos nunca fue tan encantador”</p>
              <h5 class="card-title" style="margin-bottom: 5px;">Sylveon</h5>
              <p class="card-subtitle text-muted" style="font-size: 0.875rem;">
                Encantos Sylveon</p>
            </div>
          </div>
        </div>
        <div class="col-md-4 mb-4">
          <div class="card text-center" style="min-height: 250px; display: flex;
            flex-direction: column; justify-content: space-between;">
            <img src="../assets/images/HomePage/Reviews/fallguys_review.jpg"
              alt="Persona 3" class="card-img-top rounded-circle mx-auto mt-3"
              style="width: 80px; height: 80px; object-fit: cover;">
            <div class="card-body">
              <p class="card-text" style="font-style: italic;
                margin-bottom: 15px;">
                “¡Nada de tropezar con cálculos de planilla!”</p>
              <h5 class="card-title" style="margin-bottom: 5px;">Fall Guys</h5>
              <p class="card-subtitle text-muted" style="font-size: 0.875rem;">
                Fall Corp. Eventos Extremos</p>
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
  @import '../assets/css/HeaderFooter.css';
</style>