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
              to="/LoginUser"
              class="btn btn-outline-primary me-2"
              style="
                background-color: #405d72;
                color: #ffffff;
                border: transparent;
              "
            >
              Iniciar sesión</router-link
            >
            <router-link
              to="/RegisterEmployer"
              class="btn btn-primary"
              style="background-color: #405d72; border: transparent"
              >Registrá tu empresa
            </router-link>
          </div>
          <div class="ms-auto">
            <router-link
              to="/"
              class="btn btn-secondary"
              style="
                background-color: #f7e7dc;
                color: #2b3f4e;
                border: 2px solid transparent;
              "
              >Página principal
            </router-link>
          </div>
        </div>
      </nav>
    </header>

    <div class="container w-100">
      <router-link
        to="/RegisterBenefit"
        class="btn btn-outline-primary me-2 mb-4"
        style="background-color: #405d72; color: #ffffff; border: transparent"
      >
        Agregar
      </router-link>

      <div class="d-flex flex-column align-items-center">
        <div
          class="w-100 mb-4"
          v-for="beneficio in beneficios"
          :key="beneficio.id"
        >
          <BenefitCard
            :nombre="beneficio.nombre"
            :descripcion="beneficio.descripcion"
          />
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
            <p>
              <i class="pi pi-phone" style="color: #405d72"></i> +506 2000-0000
            </p>
            <p>
              <i class="pi pi-home" style="color: #405d72"></i> San José, Montes
              de Oca, San Pedro
            </p>
          </div>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup>
import BenefitCard from "./BenefitCard.vue";

const beneficios = [
  {
    id: 1,
    nombre: "Asociacion solidarista",
    descripcion:
      "Como empresa, valoramos profundamente el bienestar de nuestros\
                colaboradores y reconocemos el papel fundamental que desempeñan en nuestro",
  },
  {
    id: 2,
    nombre: "Gimnasio",
    descripcion:
      "En nuestra empresa, creemos firmemente en la importancia del equilibrio...",
  },
  {
    id: 3,
    nombre: "Plan dental",
    descripcion:
      "En nuestra empresa, entendemos que una buena salud bucodental...",
  },
];
</script>

<style lang="scss" scoped></style>
