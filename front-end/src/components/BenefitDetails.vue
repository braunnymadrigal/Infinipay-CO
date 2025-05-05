<template>
  <div>
    <HeaderCompany />

    <div class="container mt-5 mb-5" style="max-width: 1000px">
      <div class="row mb-4">
        <div class="col-12 text-center">
          <h1 style="color: #405d72">{{ name }}</h1>
        </div>
      </div>

      <!-- Botón de regresar debajo del título, alineado a la izquierda -->
      <div class="row mb-4">
        <div class="col-12">
          <router-link to="/BenefitList">
            <button
              type="button"
              class="btn btn-success"
              style="background-color: #405d72; border: transparent"
            >
              Regresar
            </button>
          </router-link>
        </div>
      </div>

      <!-- Descripción alineada a la izquierda con más texto -->
      <div class="col-12 mt-4">
        <p class="text-start">
          {{ description }}
        </p>
      </div>

      <!-- Lista de empleados sin estilos extra -->
      <div class="col-12 mt-4">
        <h5 class="text-start">Califican:</h5>
        <ul>
          <li v-for="(employee, index) in appliesTo" :key="index">
            {{ employee }}
          </li>
        </ul>
      </div>

      <div class="col-12 mt-4">
        <h5 class="text-start">Tiempo mínimo de permanencia:</h5>
        <p class="text-start">{{ minMonths }} meses</p>
      </div>
      <div class="col-12 mt-4">
        <h5 class="text-start">Tipo de deducción: {{ typeFormula }}</h5>
        <p class="text-start">Monto: {{ formula }}</p>
      </div>

      <!-- <div class="d-flex justify-content-between">
        <router-link to="#">
          <button
            class="btn btn-danger"
            style="width: 70px; border: transparent; width: 70px"
          >
            Borrar
          </button>
        </router-link>
        <router-link to="#">
          <button
            type="submit"
            class="btn btn-success"
            style="background-color: #405d72; border: transparent"
          >
            Editar
          </button>
        </router-link>
      </div> -->
    </div>

    <MainFooter />
  </div>
</template>

<script>
// import { useRoute } from "vue-router";
// import { ref, onMounted } from "vue";
// import axios from "axios";
import { ref } from "vue";
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";

export default {
  components: {
    HeaderCompany,
    MainFooter,
  },
  setup() {
    // const route = useRoute();

    const name = ref("Asociación solidarista");
    const description = ref(
      "Como empresa, valoramos profundamente el bienestar de nuestros colaboradores y reconocemos el papel fundamental que desempeñan en nuestro éxito. Ofrecemos diversos beneficios para promover su bienestar, como asociaciones que les permitan acceder a recursos adicionales que mejoren su calidad de vida y bienestar en el trabajo."
    );
    const appliesTo = ref(["Quincenal", "Mensual"]);
    const minMonths = ref(6);
    const formula = ref("40%");
    const typeFormula = ref("Porcentaje");

    // const getBenefit = async (benefitId) => {
    //   try {
    //     const response = await axios.get(
    //       `http://localhost:7271/api/benefits/${benefitId}`
    //     );
    //     const data = response.data;

    //     name.value = data.name;
    //     description.value = data.description;
    //     appliesTo.value = data.appliesTo;
    //     minMonths.value = data.minMonths;
    //     formula.value = data.payment;
    //     typeFormula.value = data.deductionType;
    //   } catch (error) {
    //     console.error("Error al cargar el beneficio:", error);
    //   }
    //},

    // onMounted(() => {
    //   const benefitId = route.params.benefitId;
    //   getBenefit(benefitId);
    // });

    return {
      name,
      description,
      appliesTo,
      minMonths,
      formula,
      typeFormula,
    };
  },
};
</script>

<style lang="scss" scoped></style>
