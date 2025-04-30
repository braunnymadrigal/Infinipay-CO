<template>
  <div class="container my-5">
    <div class="text-center mb-4">
      <img
        src="../assets/images/logo.png"
        alt="Company logo"
        class="img-fluid"
        style="max-width: 350px"
      />
    </div>

    <header class="mb-5 custom-header">
      <nav class="navbar navbar-expand-lg rounded custom-navbar">
        <div class="container-fluid">
          <div class="d-flex">
            <router-link
              to="/EmployerProfile"
              class="mx-2"
              style="color: #405d72"
              >Perfil</router-link
            >
            <a href="#" class="mx-2" style="color: #405d72">Empresa</a>
            <router-link to="/BenefitList" class="mx-2" style="color: #405d72"
              >Beneficios</router-link
            >
            <a href="#" class="mx-2" style="color: #405d72">Empleados</a>
          </div>
        </div>
      </nav>
    </header>

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
              v-model="newBenefit.value.name"
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
              v-model="newBenefit.value.description"
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
            <router-link to="/BenefitList">
              <button
                type="submit"
                class="btn btn-success"
                style="background-color: #405d72; border: transparent"
              >
                Agregar
              </button>
            </router-link>
          </div>
        </form>
      </div>
    </div>

    <footer class="py-5 custom-footer">
      <div class="container">
        <div class="row">
          <div class="col-md-3 mb-3">
            <p class="h5">Infinipay CO.</p>
            <div>
              <a href="#" class="fa fa-facebook"></a>
              <a href="#" class="fa fa-linkedin"></a>
              <a href="#" class="fa fa-youtube"></a>
              <a href="#" class="fa fa-instagram"></a>
            </div>
          </div>
          <div class="col-md-3 mb-3">
            <p class="h5">Empresa y equipo</p>
            <a href="#">Sobre nosotros</a>
          </div>
          <div class="col-md-3 mb-3">
            <p class="h5">Recursos</p>
            <a href="#">¿Cómo registro mi empresa?</a><br />
            <a href="#">¿Cómo registro empleados a mi empresa?</a><br />
            <a href="#">¿Cómo accedo a mi perfil?</a>
          </div>
          <div class="col-md-3 mb-3">
            <p class="h5">Contacto</p>
            <p>
              <i class="pi pi-phone" style="color: #405d72"></i> +506 2000-0000
            </p>
            <p>
              <i class="pi pi-home" style="color: #405d72"></i> San José, Montes
              de Oca, San Pedro
            </p>
          </div>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup>
import { ref } from "vue";

const newBenefit = ref({ name: "", description: "" });

function submitForm() {
  console.log("Formulario enviado:", newBenefit.value);

  newBenefit.value.name = "";
  newBenefit.value.description = "";

  // axios.post
}
</script>

<style lang="scss" scoped></style>
