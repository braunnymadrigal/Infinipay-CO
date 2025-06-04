<template>
  <HeaderCompany />

  <div
    class="card p-4 mx-auto"
    style="max-width: 1000px; background-color: #fff8f3; border: none"
  >
    <h1 class="text-center" style="color: #405d72">Editar beneficio</h1>
    <h2 class="text-center" style="color: #758694">Datos del beneficio</h2>
    <div class="">
      <form @submit.prevent="submitForm">
        <div class="mb-3">
          <label for="BenefitName" class="form-label">Nombre</label>
          <input
            type="text"
            class="form-control"
            id="BenefitName"
            style="background-color: #fff8f3"
            v-model="benefitform.benefit.name"
            required
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s&]+$"
            placeholder="Sólo letras, acentos, espacios y '&'"
          />
        </div>

        <div class="mb-3">
          <label for="BenefitDescription" class="form-label">
            Descripción</label
          >
          <textarea
            class="form-control"
            style="background-color: #fff8f3"
            v-model="benefitform.benefit.description"
            id="BenefitDescription"
            maxlength="300"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"
            placeholder="Sólo se permiten espacios, letras y acentos del abecedario español."
            rows="4"
          ></textarea>
        </div>
        <div class="mb-3">
          <label for="BenefitAppliesTo" class="form-label"
            >Empleados elegibles</label
          >
          <select
            id="BenefitAppliesTo"
            class="form-select"
            style="background-color: #fff8f3"
            v-model="benefitform.benefit.elegibleEmployees"
            required
          >
            <option disabled value="">Seleccione una opción</option>
            <option value="tiempoCompleto">Tiempo Completo</option>
            <option value="medioTiempo">Medio Tiempo</option>
            <option value="horas">Por Horas</option>
            <option value="servicios">Servicios Profesionales</option>
            <option value="todos">Todos</option>
          </select>
        </div>

        <div class="mb-3">
          <label for="BenefitMinMonths" class="form-label"
            >Tiempo mínimo en meses</label
          >
          <input
            type="number"
            class="form-control"
            id="BenefitMinMonths"
            style="background-color: #fff8f3"
            v-model.number="benefitform.benefit.minEmployeeTime"
            required
            min="0"
            step="0.01"
          />
        </div>

        <fieldset
          class="mb-4 p-3 border rounded"
          style="background-color: #fff8f3"
        >
          <legend class="fs-6 fw-bold mb-3" style="color: #405d72">
            Fórmula del beneficio
          </legend>

          <div class="mb-3">
            <label for="BenefitTypeFormula" class="form-label"
              >Tipo de fórmula</label
            >
            <select
              id="BenefitTypeFormula"
              class="form-select"
              style="background-color: #fff8f3"
              v-model="benefitform.benefit.deductionType"
              required
            >
              <option value="porcentaje">Porcentaje</option>
              <option value="montoFijo">Monto fijo</option>
              <option value="api">API</option>
            </select>
          </div>

          <div
            v-if="
              benefitform.benefit.deductionType === 'porcentaje' ||
              benefitform.benefit.deductionType === 'montoFijo'
            "
            class="mb-3"
          >
            <label for="BenefitFormula" class="form-label">Deducción</label>
            <input
              type="number"
              class="form-control"
              id="BenefitFormula"
              style="background-color: #fff8f3"
              v-model="benefitform.benefit.paramOneAPI"
              required
              min="0"
            />
          </div>

          <div v-if="benefitform.benefit.deductionType === 'api'" class="mb-3">
            <label for="ApiURL" class="form-label">URL</label>
            <input
              type="text"
              class="form-control"
              id="ApiURL"
              style="background-color: #fff8f3"
              v-model="benefitform.benefit.urlAPI"
            />
          </div>

          <div v-if="benefitform.benefit.deductionType === 'api'" class="mb-3">
            <label for="Parameter1" class="form-label">Parametro 1</label>
            <input
              type="text"
              class="form-control"
              id="Parameter1"
              style="background-color: #fff8f3"
              :v-model="String(benefitform.benefit.paramOneAPI)"
            />
          </div>

          <div v-if="benefitform.benefit.deductionType === 'api'" class="mb-3">
            <label for="Parameter2" class="form-label">Parametro 2</label>
            <input
              type="text"
              class="form-control"
              id="Parameter2"
              style="background-color: #fff8f3"
              :v-model="String(benefitform.benefit.paramTwoAPI)"
            />
          </div>

          <div v-if="benefitform.benefit.deductionType === 'api'" class="mb-3">
            <label for="Parameter3" class="form-label">Parametro 3</label>
            <input
              type="text"
              class="form-control"
              id="Parameter3"
              style="background-color: #fff8f3"
              :v-model="String(benefitform.benefit.paramThreeAPI)"
            />
          </div>
        </fieldset>

        <div class="d-flex justify-content-between mt-4">
          <button
            type="submit"
            class="btn btn-success"
            style="background-color: #405d72; border: transparent"
          >
            Guardar
          </button>

          <router-link to="/BenefitList" class="btn btn-danger">
            Cancelar</router-link
          >
        </div>
      </form>
    </div>
  </div>

  <MainFooter />
</template>

<script>
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";

export default {
  components: {
    HeaderCompany,
    MainFooter,
  },
  data() {
    return {
      benefitform: {
        benefit: {
          id: null,
          companyId: null,
          name: "",
          description: "",
          elegibleEmployees: "",
          minEmployeeTime: 0,
          deductionType: "",
          paramOneAPI: "",
          paramTwoAPI: "",
          paramThreeAPI: "",
          urlAPI: "",
        },
      },
    };
  },
  methods: {
    fetchBenefitData() {
      this.$api
        .getCompanyBenefitById(this.$route.params.id)
        .then((response) => {
          this.benefitform.benefit = response.data.benefit;
        })
        .catch((error) => {
          console.error("Error al obtener los datos del beneficio:", error);
          alert(
            "No se pudo cargar el beneficio. Por favor, inténtalo de nuevo."
          );
        });
    },
    submitForm() {
      console.log("Submitting form with data:", this.benefitform);

      this.$api
        .updateCompanyBenefit(this.benefitform, this.benefitform.benefit.id)
        .then(() => {
          this.$router.push("../BenefitList");
        })
        .catch((error) => {
          console.error("Error al actualizar el beneficio:", error);
          alert(
            "No se pudo actualizar el beneficio. Por favor, inténtalo de nuevo."
          );
        });
    },
  },
  mounted() {
    this.fetchBenefitData();
  },
};
</script>

<style lang="scss" scoped></style>
