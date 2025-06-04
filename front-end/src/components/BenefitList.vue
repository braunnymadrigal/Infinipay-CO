<template>
  <HeaderCompany />

  <div class="container mt-5 mb-5">
    <h1 class="text-center" style="color: #405d72">Lista de beneficios</h1>
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
          <th style="white-space: nowrap">Califican</th>
          <th style="white-space: nowrap">Deduccion</th>
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
              <button
                class="btn btn-danger btn-sm"
                style="width: 70px; border: transparent; width: 70px"
              >
                Eliminar
              </button>
              <router-link :to="'/BenefitDetails/' + benefit.id">
                <button
                  class="btn btn-primary btn-sm"
                  style="
                    background-color: #405d72;
                    border: transparent;
                    width: 70px;
                  "
                >
                  Ver
                </button>
              </router-link>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>

  <MainFooter />
</template>

<script setup>
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";
import { ref } from "vue";
// import { ref, onMounted } from "vue";

function truncateString(str, maxLength) {
  if (str.length > maxLength) {
    return str.substring(0, maxLength) + "...";
  }
  return str;
}

// onMounted() {
//   // Fetch data from the API or perform any other setup tasks here
//   // For example, you can use axios to fetch data from your backend
//   // axios.get('/api/benefits').then(response => {
//   //   benefits.value = response.data;
//   // });
// }

// Temporary data for benefits
const benefits = ref([
  {
    id: 1,
    name: "Asociacion solidarista",
    description:
      "Como empresa, valoramos profundamente el bienestar de nuestros colaboradores y reconocemos el papel fundamental que desempeñan en nuestro éxito.",
    minMonths: 6,
    appliesTo: ["Quincenal", "Mensual"],
    formula: "40%",
  },
  {
    id: 2,
    name: "Gimnasio",
    description:
      "En nuestra empresa, creemos firmemente en la importancia del equilibrio...",
    minMonths: 3,
    appliesTo: ["Mensual", "Quincenal"],
    formula: "25 000 CRC",
  },
  {
    id: 3,
    name: "Plan dental",
    description:
      "En nuestra empresa, entendemos que una buena salud bucodental...",
    minMonths: 12,
    appliesTo: ["Quincenal", "Mensual", "Semanal"],
    formula: "60 000 CRC",
  },
]);
</script>

<style lang="scss" scoped></style>
