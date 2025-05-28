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
        <tr v-for="(benefit, index) in benefits" :key="index">
          <td>{{ benefit.benefit.name }}</td>
          <td>{{ truncateString(benefit.benefit.description, 150) }}</td>
          <td>{{ benefit.benefit.minEmployeeTime + " meses" }}</td>
          <td>{{ benefit.benefit.elegibleEmployees }}</td>
          <td>{{ benefit.benefit.paramOneAPI }}</td>
          <td>
            <div class="d-flex justify-content-center gap-2">
              <button
                class="btn btn-danger btn-sm"
                style="width: 70px; border: transparent"
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

<script>
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";

export default {
  name: "BenefitList",
  components: {
    HeaderCompany,
    MainFooter,
  },
  data() {
    return {
      benefits: [],
    };
  },
  methods: {
    truncateString(str, maxLength) {
      if (!str) return "";
      if (str.length > maxLength) {
        return str.substring(0, maxLength) + "...";
      }
      return str;
    },
    async getBenefits() {
      try {
        const response = await this.$api.getCompanyBenefits();
        this.benefits = response.data;
      } catch (error) {
        console.error("Error obteniendo los beneficios:", error);
      }
    },
  },
  mounted() {
    this.getBenefits();
  },
};
</script>

<style scoped lang="scss"></style>
