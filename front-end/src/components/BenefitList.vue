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
          <td>{{ benefit.elegibleEmployees }}</td>
          <td>{{ benefit.param1 }}</td>
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
import axios from "axios";
import { ref, onMounted } from "vue";
import cookies from "vue-cookies";

function truncateString(str, maxLength) {
  if (str.length > maxLength) {
    return str.substring(0, maxLength) + "...";
  }
  return str;
}

const benefits = ref([]);

function getBenefits() {
  console.log("Obteniendo beneficios...");

  let jwt = cookies.get("jwt");
  console.log("JWT:", jwt);

  axios
    .get("https://localhost:7275/api/Benefit/", {
      headers: { Authorization: `Bearer ${jwt}` },
    })
    .then((response) => {
      benefits.value = response.data;
    })
    .catch((error) => {
      console.error("Error obteniendo los beneficios:", error);
    });
}

onMounted(() => {
  getBenefits();
});
</script>

<style lang="scss" scoped></style>
