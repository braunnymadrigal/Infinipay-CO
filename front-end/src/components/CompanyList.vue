<template>
  <div>
    <!-- Header -->
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
    </div>

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
  </div>
</template>

<script>
import axios from "axios";
import MainFooter from "./MainFooter.vue";

export default {
  components: {
    MainFooter,
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
