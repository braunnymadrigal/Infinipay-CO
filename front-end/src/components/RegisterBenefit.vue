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
            v-model="newBenefit.elegibleEmployees"
            required
          >
            <option disabled value="">Seleccione una opción</option>
            <option value="todos">Todos</option>
            <option value="semanal">Semanal</option>
            <option value="quincenal">Quincenal</option>
            <option value="mensual">Mensual</option>
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
            Deducción
          </legend>

          <div class="mb-3">
            <label for="BenefitTypeFormula" class="form-label"
              >Tipo de deduccion</label
            >
            <select
              id="BenefitTypeFormula"
              class="form-select"
              style="background-color: #fff8f3"
              v-model="newBenefit.formulaType"
              required
            >
              <option value="Porcentaje">Porcentaje</option>
              <option value="Monto fijo">Monto fijo</option>
              <option value="Api">Api</option>
            </select>
          </div>

          <div
            v-if="
              newBenefit.formulaType === 'Porcentaje' ||
              newBenefit.formulaType === 'Monto fijo'
            "
            class="mb-3"
          >
            <label for="BenefitFormula" class="form-label">Deducción</label>
            <input
              type="number"
              class="form-control"
              id="BenefitFormula"
              style="background-color: #fff8f3"
              v-model="newBenefit.param1"
              required
              min="0"
            />
          </div>

          <!-- Mostrar campos para la opción API -->
          <div v-if="newBenefit.formulaType === 'Api'" class="mb-3">
            <label for="ApiURL" class="form-label">URL</label>
            <input
              type="text"
              class="form-control"
              id="ApiURL"
              style="background-color: #fff8f3"
              v-model="newBenefit.apiUrl"
            />
          </div>

          <div v-if="newBenefit.formulaType === 'Api'" class="mb-3">
            <label for="Parameter1" class="form-label">Parametro 1</label>
            <input
              type="text"
              class="form-control"
              id="Parameter1"
              style="background-color: #fff8f3"
              v-model="newBenefit.param1"
            />
          </div>

          <div v-if="newBenefit.formulaType === 'Api'" class="mb-3">
            <label for="Parameter2" class="form-label">Parametro 2</label>
            <input
              type="text"
              class="form-control"
              id="Parameter2"
              style="background-color: #fff8f3"
              v-model="newBenefit.param2"
              min="0"
            />
          </div>

          <div v-if="newBenefit.formulaType === 'Api'" class="mb-3">
            <label for="Parameter3" class="form-label">Parametro 3</label>
            <input
              type="text"
              class="form-control"
              id="Parameter3"
              style="background-color: #fff8f3"
              v-model="newBenefit.param3"
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
import axios from "axios";

const newBenefit = ref({
  name: "",
  description: "",
  elegibleEmployees: "",
  minMonths: 0,
  formulaType: "",
  param1: "",
  param2: "",
  param3: "",
  userid: "",
  apiUrl: "",
});

async function submitForm() {
  if (
    newBenefit.value.name.trim() === "" ||
    newBenefit.value.description.trim() === "" ||
    newBenefit.value.elegibleEmployees === "" ||
    newBenefit.value.minMonths <= 0 ||
    newBenefit.value.formulaType === "" ||
    newBenefit.value.userid === ""
  ) {
    alert("Por favor, completa todos los campos requeridos.");
    return;
  }

  try {
    const response = await axios.get(
      "https://localhost:7275/api/Login/GetLoggedUser",
      {
        headers: {
          Authorization: `Bearer ${this.$cookies.get(`jwt`)}`,
        },
      }
    );
    newBenefit.value.userid = response.data.id;
    await createBenefit(newBenefit.value);
    alert("Beneficio creado exitosamente.");
    newBenefit.value = {
      name: "",
      description: "",
      elegibleEmployees: "",
      minMonths: 0,
      formulaType: "",
      param1: "",
      param2: "",
      param3: "",
      userid: "",
      apiUrl: "",
    };
  } catch (error) {
    alert("Error al crear el beneficio. Inténtalo de nuevo.");
  }
}

async function createBenefit(benefit) {
  const response = await axios.post(
    "http://localhost:7275/api/Benefit",
    benefit
  );
  return response.data;
}
</script>

<style lang="scss" scoped></style>
