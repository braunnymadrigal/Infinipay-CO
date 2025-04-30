<template>
  <HeaderCompany />

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

  <MainFooter />
</template>

<script setup>
import BenefitCard from "./BenefitCard.vue";
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";

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
