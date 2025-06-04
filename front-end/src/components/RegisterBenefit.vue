<template>
  <HeaderCompany />

  <div
    class="card p-4 mx-auto"
    style="max-width: 1000px; background-color: #fff8f3; border: none"
  >
    <h1 class="text-center" style="color: #405d72">Agregar beneficio</h1>
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
            v-model="newBenefitForm.name"
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
            v-model="newBenefitForm.description"
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
            v-model="newBenefitForm.elegibleEmployees"
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
            v-model.number="newBenefitForm.minEmployeeTime"
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
              v-model="newBenefitForm.deductionType"
              required
            >
              <option value="porcentaje">Porcentaje</option>
              <option value="montoFijo">Monto fijo</option>
              <option value="api">API</option>
            </select>
          </div>

          <div
            v-if="
              newBenefitForm.deductionType === 'porcentaje' ||
              newBenefitForm.deductionType === 'montoFijo'
            "
            class="mb-3"
          >
            <label for="BenefitFormula" class="form-label">Deducción</label>
            <input
              type="number"
              class="form-control"
              id="BenefitFormula"
              style="background-color: #fff8f3"
              v-model="newBenefitForm.paramOneAPI"
              required
              min="0"
            />
          </div>

          <div v-if="newBenefitForm.deductionType === 'api'" class="mb-3">
            <label for="ApiURL" class="form-label">URL</label>
            <input
              type="text"
              class="form-control"
              id="ApiURL"
              style="background-color: #fff8f3"
              v-model="newBenefitForm.urlAPI"
            />
          </div>

          <div v-if="newBenefitForm.deductionType === 'api'" class="mb-3">
            <label for="Parameter1" class="form-label">Parametro 1</label>
            <input
              type="text"
              class="form-control"
              id="Parameter1"
              style="background-color: #fff8f3"
              v-model="newBenefitForm.paramOneAPI"
            />
          </div>

          <div v-if="newBenefitForm.deductionType === 'api'" class="mb-3">
            <label for="Parameter2" class="form-label">Parametro 2</label>
            <input
              type="text"
              class="form-control"
              id="Parameter2"
              style="background-color: #fff8f3"
              v-model="newBenefitForm.paramTwoAPI"
            />
          </div>

          <div v-if="newBenefitForm.deductionType === 'api'" class="mb-3">
            <label for="Parameter3" class="form-label">Parametro 3</label>
            <input
              type="text"
              class="form-control"
              id="Parameter3"
              style="background-color: #fff8f3"
              v-model="newBenefitForm.paramThreeAPI"
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
      newBenefitForm: {
        name: "",
        description: "",
        elegibleEmployees: "",
        minEmployeeTime: 0,
        deductionType: "",
        paramOneAPI: 0,
        paramTwoAPI: null,
        paramThreeAPI: null,
        urlAPI: "",
      },
    };
  },
  methods: {
    submitForm() {
      const form = this.newBenefitForm;

      if (
        form.name.trim() === "" ||
        form.description.trim() === "" ||
        form.elegibleEmployees === "" ||
        form.deductionType === "" ||
        form.paramOneAPI === null
      ) {
        alert("Por favor, completa todos los campos requeridos.");
        return;
      }

      const newBenefit = {
        benefit: {
          name: form.name,
          description: form.description,
          elegibleEmployees: form.elegibleEmployees,
          minEmployeeTime: form.minEmployeeTime,
          deductionType: form.deductionType,
          urlAPI: form.urlAPI ? String(form.urlAPI) : null,
          paramOneAPI: String(form.paramOneAPI),
          paramTwoAPI:
            form.paramTwoAPI !== null ? String(form.paramTwoAPI) : null,
          paramThreeAPI:
            form.paramThreeAPI !== null ? String(form.paramThreeAPI) : null,
        },
      };
      this.$api
        .createCompanyBenefit(newBenefit)
        .then(() => {
          alert("Beneficio creado exitosamente.");
          this.$router.push("/BenefitList");
        })
        .catch((error) => {
          console.error("Error al crear el beneficio:", error);
          alert(
            "Hubo un error al crear el beneficio. Por favor, inténtalo de nuevo."
          );
        });
    },
  },
};
</script>

<style lang="scss" scoped></style>
