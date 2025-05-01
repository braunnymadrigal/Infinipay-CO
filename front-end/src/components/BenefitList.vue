<template>
  <HeaderCompany />

  <div class="container mt-5 mb-5">
    <h1 class="text-center" style="color: #405d72">Agregar beneficio</h1>
    <div class="row justify-content-start">
      <div class="col-2">
        <router-link to="/RegisterBenefit">
          <button
            type="button"
            class="btn btn-success"
            style="background-color: #405d72; border: transparent"
          >
            Agregar
          </button>
        </router-link>
      </div>
    </div>
    <table
      class="table is-bordered table-striped is-narrow is-hoverable is-fullwidth"
    >
      <thead>
        <tr>
          <th style="white-space: nowrap">Nombre</th>
          <th style="white-space: nowrap">Descripción</th>
          <th style="white-space: nowrap">Tiempo minimo</th>
          <th style="white-space: nowrap">Aplica a</th>
          <th style="white-space: nowrap">Formula</th>
          <th style="white-space: nowrap">Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(benefit, index) of benefits" :key="index">
          <td>{{ benefit.name }}</td>
          <td>{{ truncateString(benefit.description, 150) }}</td>
          <td>{{ benefit.minMonths + " meses" }}</td>
          <td>{{ benefit.appliesTo.join(", ") }}</td>
          <td>{{ benefit.formula }}</td>
          <td>
            <div class="d-flex justify-content-center gap-2">
              <button v-on:click="eliminar" class="btn btn-danger btn-sm">
                Eliminar
              </button>
              <button
                class="btn btn-secondary btn-sm"
                style="background-color: #405d72; border: transparent"
              >
                Ver
              </button>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>

  <MainFooter />
</template>

<script setup>
// import BenefitCard from "./BenefitCard.vue";
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";

function truncateString(str, maxLength) {
  if (str.length > maxLength) {
    return str.substring(0, maxLength) + "...";
  }
  return str;
}

// Temporary data for benefits
const benefits = [
  {
    id: 1,
    name: "Asociacion solidarista",
    description:
      "Como empresa, valoramos profundamente el bienestar de nuestros colaboradores y reconocemos el papel fundamental que desempeñan en nuestro",
    minMonths: 6,
    appliesTo: ["Supervisores", "Tiempo completo"],
    formula: "40%",
  },
  {
    id: 2,
    name: "Gimnasio",
    description:
      "En nuestra empresa, creemos firmemente en la importancia del equilibrio...",
    minMonths: 3,
    appliesTo: ["Tiempo completo", "Medio tiempo"],
    formula: "25 000 CRC",
  },
  {
    id: 3,
    name: "Plan dental",
    description:
      "En nuestra empresa, entendemos que una buena salud bucodental...",
    minMonths: 12,
    appliesTo: ["Supervisores", "Tiempo completo", "Servicios profesionales"],
    formula: "60 000 CRC",
  },
];
</script>

<style lang="scss" scoped></style>
