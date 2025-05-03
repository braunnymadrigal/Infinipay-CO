<template>
  <div class="card p-4 mx-auto bg-transparent border-0 w-50" >
    <h1
      class="text-center font-weight-bold"
      style="color: #405D72">
      Mi perfil
    </h1>

    <form @submit.prevent="submitForm">
      <div class="row mb-3 justify-content-center" style="margin-top: 30px;">
        <div class="col-md-6 col-lg-6">
          <label 
            for="firstName"
            class="form-label">
            Primer nombre
          </label>
          <input
            class="form-control bg-transparent"
            type="text"
            id="firstName" 
            v-model="user.userId"
            readonly
          >
        </div>
        <div class="col-md-6 col-lg-6">
          <label
            for="secondName"
            class="form-label">
            Segundo nombre
          </label>
          <input
            class="form-control bg-transparent"
            type="text"
            id="secondName"
            v-model="user.userId" 
            readonly
          >
        </div>
      </div>

      <div class="row mb-3 justify-content-center">
        <div class="col-md-6 col-lg-6">
          <label
            for="firstLastName"
            class="form-label">
            Primer apellido
          </label>
          <input
            class="form-control bg-transparent"
            type="text"
            id="fistLastName"
            v-model="user.userId" 
            readonly
          >
        </div>
        <div class="col-md-6 col-lg-6">
          <label
            for="secondLastName"
            class="form-label">
            Segundo apellido
          </label>
          <input
            class="form-control bg-transparent"
            type="text"
            id="secondLastName"
            v-model="user.userId" 
            readonly
          >
        </div>
      </div>

      <div class="mb-3">
        <label
          for="username"
          class="form-label">
          Nombre de usuario
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="username"
          v-model="user.userId" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="identityDocument"
          class="form-label">
          Cédula
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="identityDocument"
          v-model="user.userId" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="email"
          class="form-label">
          Correo electrónico
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="email"
          v-model="user.userId" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="phoneNumber"
          class="form-label">
          Teléfono
        </label>
        <div class="d-flex align-items-center mb-2">
          <div class="me-3 text-start">+506</div>
          <input
            class="form-control bg-transparent"
            type="text"
            id="phoneNumber"
            v-model="user.userId" 
            readonly
          >
        </div>
      </div>

      <div class="row mb-3 justify-content-center" style="margin-top: 30px;">
        <div class="col-md-4 col-lg-4">
          <label 
            for="province"
            class="form-label">
            Provincia
          </label>
          <input
            class="form-control bg-transparent"
            type="text"
            id="province" 
            v-model="user.userId"
            readonly
          >
        </div>
        <div class="col-md-4 col-lg-4">
          <label
            for="canton"
            class="form-label">
            Cantón
          </label>
          <input
            class="form-control bg-transparent"
            type="text"
            id="canton"
            v-model="user.userId" 
            readonly
          >
        </div>
        <div class="col-md-4 col-lg-4">
          <label
            for="district"
            class="form-label">
            Distrito
          </label>
          <input
            class="form-control bg-transparent"
            type="text"
            id="district"
            v-model="user.userId" 
            readonly
          >
        </div>
      </div>

      <div class="mb-3">
        <label
          for="exactAddress"
          class="form-label">
          Dirección exacta
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="exactAddress"
          v-model="user.userId" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="gender"
          class="form-label">
          Género
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="gender"
          v-model="user.userId" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="birthDate"
          class="form-label">
          Fecha de nacimiento
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="birthDate"
          v-model="user.userId" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="companyName"
          class="form-label">
          Empresa
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="companyName"
          v-model="user.userId" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="companyRole"
          class="form-label">
          Rol dentro de la empresa
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="companyRole"
          v-model="user.userId" 
          readonly
        >
      </div>

      <div class="my-5" style="text-align: center;">
        <button type="submit" class="btn btn-dark btn-round" 
        style="background-color: #405D72; border-color: #2b3f4e;">
          Editar
        </button>
      </div>
    </form>
  </div>

  <MainFooter/>
</template>

<script>
import MainFooter from "./MainFooter.vue";
export default {
  components: {
    MainFooter
  },
  data() {
    return {
      user: { userId: "Pepe", userPassword: "" },
    };
  },
  methods: {
    startLogin() {
      console.log("Saved:", this.user);
    },
  },
};
</script>

<style>
  @import '../assets/css/HeaderFooter.css';
</style>