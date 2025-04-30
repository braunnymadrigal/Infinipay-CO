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
          <textarea
            class="form-control"
            id="BenefitName"
            style="background-color: #fff8f3"
            v-model="newBenefit.name"
            required
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s&]+$"
            placeholder="Sólo letras, acentos, espacios y '&'"
            rows="2"
          >
          </textarea>
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
        <div class="d-flex justify-content-center mt-4" style="gap: 10px">
          <router-link
            to="/BenefitList"
            class="btn btn-secondary"
            style="background-color: #405d72"
          >
            Cancelar</router-link
          >

          <button
            type="submit"
            class="btn btn-success"
            style="background-color: #405d72; border: transparent"
          >
            Agregar
          </button>
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

const newBenefit = ref({ name: "", description: "" });

function submitForm() {
  console.log("Formulario enviado:" + newBenefit.value);

  newBenefit.value.name = "";
  newBenefit.value.description = "";

  // axios.post
}
</script>

<style lang="scss" scoped></style>
