<template>
  <div>
    <!-- Logo de la empresa -->
    <div class="logo_empresa">
      <img
        src="../assets/images/logo.png"
        alt="Logo de la empresa"
        class="logo_img"
      />
    </div>
    <!-- Header -->
    <header class="header">
      <div class="navbar">
        <nav class="navbar-links">
          <!-- Contenedor de los botones de la izquierda -->
          <div class="navbar-links-left">
            <a href="#" class="botones_izquierda">Iniciar sesión</a>
            <router-link to="/registrar_empleador" class="botones_izquierda"
              >Registrá tu empresa</router-link
            >
          </div>
          <!-- Contenedor de los botones de la derecha -->
          <div class="navbar-links-right">
            <router-link to="/" class="botones_derecha"
              >Página principal</router-link
            >
          </div>
        </nav>
      </div>
    </header>

    <div>
      <router-link to="/registrar_beneficio"
        ><button type="button" class="boton_siguiente">
          Agregar beneficio
        </button></router-link
      >
      <ul v-for="beneficio in beneficios" :key="beneficio.id">
        <!-- <li v-for="beneficio in beneficios" :key="beneficio.id"> -->
        <!-- <h3>{{ beneficio.nombre }}</h3> -->
        <!-- <p>{{ beneficio.descripcion }}</p> -->
        <!-- </li> -->
        <BeneficioCard
          :nombre="beneficio.nombre"
          :descripcion="beneficio.descripcion"
        ></BeneficioCard>
      </ul>
    </div>
  </div>
</template>

<script setup>
import BeneficioCard from "./BeneficioCard.vue";

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
