<template>
  <!-- HEADER -->
  <div class="container my-5">
    <div class="text-center mb-4">
      <img src="../assets/images/logo.png" alt="Company logo" class="img-fluid"
        style="max-width: 350px;">
    </div>
    <header class="mb-5 custom-header">
      <nav class="navbar navbar-expand-lg rounded custom-navbar">
        <div class="container-fluid">
          <div class="d-flex">
            <router-link to="/LoginUser" class="btn btn-outline-primary me-2"
              style="background-color: #405D72; color: #FFFFFF;
              border: transparent;"> Iniciar sesión</router-link>
            <router-link to="/RegisterEmployer" class="btn btn-primary"
              style="background-color: #405D72;
              border: transparent;">Registrá tu empresa
          </router-link>
          </div>
          <div class="ms-auto">
            <router-link to="/" class="btn btn-primary me-2"
              style="background-color: #F7E7DC; color: #2b3f4e;
                border: 2px solid transparent;">Página Principal
            </router-link>
            <router-link to="/" class="btn btn-secondary"
              style="background-color: #F7E7DC; color: #2b3f4e;
                border: 2px solid transparent;">¿Necesitás ayuda?
            </router-link>
          </div>
        </div>
      </nav>
    </header>
  </div>

  <!-- CONTENT -->
  <div class="h-100 d-flex align-items-center justify-content-center 
  bg-transparent">
    <div class="card p-4 bg-transparent border-0" 
    style="max-width: 600px; width: 100%; text-align: center">
      <i class="pi pi-user" style="font-size: 8.5rem; color: #405D72"></i>
    </div>
  </div>
  <div class="h-100 d-flex align-items-center justify-content-center 
  bg-transparent">
    <div class="card p-4 bg-transparent border-0" 
    style="max-width: 600px; width: 100%">
      <h1 class="font-weight-bold" style="text-align: center; color: #405D72">
        Iniciar sesión</h1>
    </div>
  </div>
  <div class="h-100 d-flex align-items-center justify-content-center 
  bg-transparent">
    <div class="card p-4 bg-transparent border-0" 
    style="max-width: 600px; width: 100%">
      <form @submit.prevent="startLogin" class="form">
        <div class="form-group text-start">
          <label for="userId">Correo electrónico o nombre de usuario</label>
          <input
            v-model="user.userId"
            type="text"
            id="userId"
            pattern="([^\s@]+@[^\s@]+\.[^\s@]+)|(^[A-Za-z._]{1,30}$)"
            minlength="1"
            maxlength="100"
            title="Ingrese un correo electrónico o nombre de usuario valido 
            (No se permiten espacios al inicio ni al final). El nombre de 
            usuario sólo acepta letras y los caracteres especiales '.' o '_'. "
            placeholder=""
            class="form-control bg-transparent"
            required
          />
        </div>
        <div class="form-group text-start mb-3">
          <label for="userPassword">Contraseña</label>
          <input
            v-model="user.userPassword"
            type="password"
            id="userPassword"
            pattern="^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@#$%&*!?]).+$"
            minlength="10"
            maxlength="100"
            title="La contraseña debe superar los 10 caracteres y no sobrepasar 
            los 100. La contraseña debe contener al menos una mayúscula, una 
            minúscula y un número. La contraseña debe contener al menos un 
            carácter especial de la siguiente lista: '@', '#', '$', '%', '&', 
            '*', '!', '?'."
            placeholder=""
            class="form-control bg-transparent"
            required
          />
        </div>
        <div style="text-align: center">
          <button type="submit" class="btn btn-dark btn-round" 
          style="background-color: #405D72; border-color: #2b3f4e;">
            Iniciar sesión
          </button>
        </div>
      </form>
    </div>
  </div>
  <div class="h-100 d-flex align-items-center justify-content-center 
  bg-transparent">
    <div class="card p-4 bg-transparent border-0" 
    style="max-width: 600px; width: 100%">
      <a href="#" class="text-start">¿Has olvidado tu contraseña?</a> 
    </div>
  </div>

  <!-- FOOTER -->
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
        user: { userId: "", userPassword: "" },
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