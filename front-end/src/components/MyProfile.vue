<template>
  <CompanyHeader :rol="profile.Rol" />

  <div v-if="!showPopup" @click.stop 
  class="card p-4 mx-auto bg-transparent border-0 w-50" >
    <h1
      class="text-center font-weight-bold"
      style="color: #405D72">
      Mi perfil
    </h1>

    <form @submit.prevent="editExample">
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
            v-model="profile.PrimerNombre"
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
            v-model="profile.SegundoNombre" 
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
            v-model="profile.PrimerApellido" 
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
            v-model="profile.SegundoApellido" 
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
          v-model="profile.NombreUsuario" 
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
          v-model="profile.Cedula" 
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
          v-model="profile.Correo" 
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
            v-model="profile.Telefono" 
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
            v-model="profile.Provincia"
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
            v-model="profile.Canton" 
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
            v-model="profile.Distrito" 
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
          v-model="profile.DireccionExacta" 
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
          v-model="profile.Genero" 
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
          v-model="profile.FechaNacimiento" 
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
          v-model="profile.Empresa" 
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
          v-model="profile.Rol" 
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

  <div v-if="showPopup" @click.stop class="d-flex justify-content-center my-5 py-5">
    <div class="display-1 text-danger">Debe iniciar sesión para ver su perfil.</div>
  </div> 

  <MainFooter/>
</template>

<script>
import MainFooter from "./MainFooter.vue";
import CompanyHeader from "./HeaderCompany.vue";
import axios from "axios";
export default {
  components: {
    MainFooter,
    CompanyHeader,
  },
  data() {
    return {
      showPopup: false,
      profile: {
        PrimerNombre: "",
        SegundoNombre: "",
        PrimerApellido: "",
        SegundoApellido: "",
        NombreUsuario: "",
        Cedula: "",
        Correo: "",
        Telefono: "",
        Provincia: "",
        Canton: "",
        Distrito: "",
        DireccionExacta: "",
        Genero: "",
        FechaNacimiento: "",
        Empresa: "",
        Rol: "", 
      },
    };
  },
  methods: {
    editExample() {
      console.log("Próximamente llegará la opción 'Editar'.");
    },

    getProfile() {
      let jwtCookie = this.$cookies.get('jwt');
      axios.get("https://localhost:7275/api/Profile", 
      { headers: {"Authorization" : `Bearer ${jwtCookie}`} })
        .then(
          response => {
            this.showPopup = false;
            this.profile = response.data;
          }
        )
        .catch(
          error => {
            this.showPopup = true;
            console.log(error);
            setTimeout(() => {
              this.$router.push('LoginUser');
            }, 4000);
          }
        )
      ;
    },
  },

  mounted() {
    this.getProfile();
  },
};
</script>

<style>
  @import '../assets/css/HeaderFooter.css';
</style>