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
            v-model="newBenefit.BenefitName"
            required
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s&]+$"
            placeholder="Sólo letras, acentos, espacios y '&'"
          />
        </div>

        <div class="mb-3">
          <label for="BenefitDescription" class="form-label">Descripción</label>
          <textarea
            class="form-control"
            style="background-color: #fff8f3"
            v-model="newBenefit.BenefitDescription"
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
            v-model="newBenefit.BenefitElegibleEmployees"
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
            v-model.number="newBenefit.BenefitMinTime"
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
              >Tipo de deducción</label
            >
            <select
              id="BenefitTypeFormula"
              class="form-select"
              style="background-color: #fff8f3"
              v-model="newBenefit.FormulaType"
              required
            >
              <option value="Porcentaje">Porcentaje</option>
              <option value="Monto fijo">Monto fijo</option>
              <option value="Api">Api</option>
            </select>
          </div>

          <div
            v-if="
              newBenefit.FormulaType === 'Porcentaje' ||
              newBenefit.FormulaType === 'Monto fijo'
            "
            class="mb-3"
          >
            <label for="BenefitFormula" class="form-label">Deducción</label>
            <input
              type="number"
              class="form-control"
              id="BenefitFormula"
              style="background-color: #fff8f3"
              v-model="newBenefit.formulaParamUno"
              required
              min="0"
            />
          </div>

          <div v-if="newBenefit.FormulaType === 'Api'" class="mb-3">
            <label for="ApiURL" class="form-label">URL</label>
            <input
              type="text"
              class="form-control"
              id="ApiURL"
              style="background-color: #fff8f3"
              v-model="newBenefit.urlAPI"
            />
          </div>

          <div v-if="newBenefit.FormulaType === 'Api'" class="mb-3">
            <label for="Parameter1" class="form-label">Parametro 1</label>
            <input
              type="text"
              class="form-control"
              id="Parameter1"
              style="background-color: #fff8f3"
              v-model="newBenefit.formulaParamUno"
            />
          </div>

          <div v-if="newBenefit.FormulaType === 'Api'" class="mb-3">
            <label for="Parameter2" class="form-label">Parametro 2</label>
            <input
              type="text"
              class="form-control"
              id="Parameter2"
              style="background-color: #fff8f3"
              v-model="newBenefit.formulaParamDos"
            />
          </div>

          <div v-if="newBenefit.FormulaType === 'Api'" class="mb-3">
            <label for="Parameter3" class="form-label">Parametro 3</label>
            <input
              type="text"
              class="form-control"
              id="Parameter3"
              style="background-color: #fff8f3"
              v-model="newBenefit.formulaParamTres"
            />
          </div>
        </fieldset>

        <div class="d-flex justify-content-between mt-4">
          <router-link to="/BenefitList"
            ><button
              type="submit"
              class="btn btn-success"
              style="background-color: #405d72; border: transparent"
            >
              Guardar
            </button>
          </router-link>

          <router-link to="/BenefitList" class="btn btn-danger"
            >Cancelar</router-link
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
import cookies from "vue-cookies";

const newBenefit = ref({
  BenefitName: "",
  BenefitDescription: "",
  BenefitElegibleEmployees: "",
  BenefitMinTime: 0,
  FormulaType: "",
  formulaParamUno: "",
  formulaParamDos: "",
  formulaParamTres: "",
  urlAPI: "",
});

function submitForm() {
  if (
    newBenefit.value.BenefitName.trim() === "" ||
    newBenefit.value.BenefitDescription.trim() === "" ||
    newBenefit.value.BenefitElegibleEmployees === "" ||
    newBenefit.value.BenefitMinTime < 0 ||
    newBenefit.value.FormulaType === ""
  ) {
    alert("Por favor, completa todos los campos requeridos.");
    return;
  }

  const jwt = cookies.get("jwt");
  console.log("New:", newBenefit.value);
  axios
    .post(
      "https://localhost:7275/api/Benefit",
      {
        BenefitName: newBenefit.value.BenefitName,
        BenefitDescription: newBenefit.value.BenefitDescription,
        BenefitElegibleEmployees: newBenefit.value.BenefitElegibleEmployees,
        BenefitMinTime: newBenefit.value.BenefitMinTime,
        FormulaType: newBenefit.value.FormulaType,
        formulaParamUno: newBenefit.value.formulaParamUno,
        urlAPI: newBenefit.value.urlAPI,
        formulaParamDos: newBenefit.value.formulaParamDos,
        formulaParamTres: newBenefit.value.formulaParamTres,
      },
      {
        headers: {
          Authorization: `Bearer ${jwt}`,
          "Content-Type": "application/json",
        },
      }
    )
    .then(() => {
      alert("Beneficio registrado exitosamente.");
    })
    .catch((error) => {
      console.error("Error al registrar el beneficio:", error);
    });
}
</script>

<style lang="scss" scoped></style>
