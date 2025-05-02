<template>
  <HeaderCompany />

  <div
    class="card p-4 mx-auto"
    style="max-width: 1000px; background-color: #fff8f3; border: none"
  >
    <h1 class="text-center" style="color: #405d72">Agregar beneficio</h1>
    <h2 class="text-center" style="color: #758694">Datos de el beneficio</h2>
    <div class="">
      <form @submit.prevent="submitForm">
        <div class="mb-3">
          <label for="BenefitName" class="form-label">Nombre</label>
          <input
            type="text"
            class="form-control"
            id="BenefitName"
            style="background-color: #fff8f3"
            v-model="newBenefit.name"
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
            v-model="newBenefit.description"
            id="BenefitDescription"
            maxlength="300"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"
            placeholder="Sólo se permiten espacios, letras y acentos del abecedario español."
            rows="4"
          ></textarea>
        </div>
        <div class="mb-3">
          <label for="BenefitAppliesTo" class="form-label">Aplica a</label>
          <div>
            <label class="d-block">
              <input
                type="checkbox"
                value="Supervisores"
                v-model="newBenefit.appliesTo"
                class="me-2"
              />
              Supervisores
            </label>
            <label class="d-block">
              <input
                type="checkbox"
                value="Tiempo completo"
                v-model="newBenefit.appliesTo"
                class="me-2"
              />
              Tiempo completo
            </label>
            <label class="d-block">
              <input
                type="checkbox"
                value="Medio tiempo"
                v-model="newBenefit.appliesTo"
                class="me-2"
              />
              Medio tiempo
            </label>
            <label class="d-block">
              <input
                type="checkbox"
                value="Servicios profesionales"
                v-model="newBenefit.appliesTo"
                class="me-2"
              />
              Servicios profesionales
            </label>
          </div>
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
            v-model.number="newBenefit.minMonths"
            required
            min="0"
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
              v-model="newBenefit.typeFormula"
              required
            >
              <option value="Porcentaje">Porcentaje</option>
              <option value="Monto fijo">Monto fijo</option>
            </select>
          </div>

          <div class="mb-3">
            <label for="BenefitFormula" class="form-label">Fórmula</label>
            <input
              type="number"
              class="form-control"
              id="BenefitFormula"
              style="background-color: #fff8f3"
              v-model="newBenefit.formula"
              required
              min="0"
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

<script setup>
import { ref } from "vue";
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";

const newBenefit = ref({
  name: "",
  description: "",
  appliesTo: [],
  minMonths: 0,
  formula: 0,
  typeFormula: "",
});

function submitForm() {
  console.table(
    "Formulario enviado:",
    JSON.parse(JSON.stringify(newBenefit.value))
  );

  newBenefit.value.name = "";
  newBenefit.value.description = "";
  newBenefit.value.appliesTo = [];
  newBenefit.value.minMonths = 0;
  newBenefit.value.formula = 0;
  newBenefit.value.typeFormula = "";
  // axios.post
}
</script>

<style lang="scss" scoped></style>
