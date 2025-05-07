<template>
  <HeaderCompany />

    <div class="container mt-5 mb-5">
      <h1 class="text-center mb-5" style="color: #405d72">Lista de empresas</h1>
      <table
        class="table is-bordered table-striped is-narrow is-hoverable is-fullwidth"
      >
        <thead>
          <tr>
            <th style="white-space: nowrap">Nombre</th>
            <th style="white-space: nowrap">Descripción</th>
            <th style="white-space: nowrap">Cédula jurídica</th>
            <th style="white-space: nowrap">Dirección</th>
            <th style="white-space: nowrap">Número de contacto</th>
            <th style="white-space: nowrap">Fecha de creación</th>
            <th style="white-space: nowrap">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(company, index) of companies" :key="index">
            <td>{{ company.legalName }}</td>
            <td>{{ truncateString(company.description, 150) }}</td>
            <td>{{ company.idNumber }}</td>
            <td>
              {{
                company.province +
                ", " +
                company.canton +
                ", " +
                company.district
              }}
            </td>
            <td>{{ company.phoneNumber }}</td>
            <td>
              {{ company.creationDay }}/{{ company.creationMonth }}/{{
                company.creationYear
              }}
            </td>

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

<script>
import axios from "axios";
import MainFooter from "./MainFooter.vue";
import HeaderCompany from "./HeaderCompany.vue";

export default {
  components: {
    MainFooter,
    HeaderCompany,
  },
  data() {
    return {
      companies: [],
    };
  },
  methods: {
    getCompanyList() {
      axios
        .get("https://localhost:7275/api/Company")
        .then((response) => {
          this.companies = response.data;
          console.log(this.companies);
        })
        .catch((error) => {
          console.error("Error fetching company list:", error);
        });
    },
    truncateString(str, maxLength) {
      if (str.length > maxLength) {
        return str.substring(0, maxLength) + "...";
      }
      return str;
    },
  },
  mounted() {
    this.getCompanyList();
  },
};
</script>

<style lang="scss" scoped></style>
