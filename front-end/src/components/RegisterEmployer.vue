﻿<template>
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
              border: transparent;">Iniciar sesión</router-link>
            <router-link to="/RegisterEmployer" class="btn btn-primary"
              style="background-color: #405D72; border: transparent;">
              Registrá tu empresa
            </router-link>
          </div>
          <div class="ms-auto">
            <router-link to="/" class="btn btn-secondary"
              style="background-color: #F7E7DC; color: #2b3f4e;
                border: transparent;">Página principal
            </router-link>
          </div>
        </div>
      </nav>
    </header>

    <div class="card p-4 mx-auto" style="max-width: 1000px;
      background-color: #FFF8F3; border: none;">
      <h1 class="text-center" style="color: #405D72">Registrá tu empresa</h1>
      <h2 class="text-center" style="color: #758694">
        Datos del dueño de la empresa</h2>

      <form @submit.prevent="submitForm">

        <div class="row mb-3 justify-content-center" style="margin-top: 30px;">
          <div class="col-md-6 col-lg-6">
            <label for="firstName" class="form-label">Primer nombre</label>
            <input type="text" class="form-control"
                   style="background-color: #FFF8F3;" v-model="firstName"
                   id="firstName" required maxlength="50"
                   pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
                   placeholder="Sólo se permiten letras y acentos del abecedario español"
                   title="Sólo se permiten letras y acentos del abecedario español">
          </div>

          <div class="col-md-6 col-lg-6">
            <label for="secondName" class="form-label">Segundo nombre</label>
            <input type="text" class="form-control"
                   style="background-color: #FFF8F3;"
                   v-model="secondName" id="secondName"
                   maxlength="50" pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
                   placeholder="Sólo se permiten letras y acentos del abecedario español"
                   title="Sólo se permiten letras y acentos del abecedario español">
          </div>
        </div>

        <div class="row mb-3 justify-content-center">
          <div class="col-md-6 col-lg-6">
            <label for="firstLastName" class="form-label">
              Primer apellido
            </label>
            <input type="text" class="form-control"
                   style="background-color: #FFF8F3;" v-model="firstLastName"
                   id="firstLastName" required maxlength="50"
                   pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$" placeholder="Sólo se permiten letras y acentos del abecedario español"
                   title="Sólo se permiten letras y acentos del abecedario español">
          </div>

          <div class="col-md-6 col-lg-6">
            <label for="secondLastName" class="form-label">
              Segundo apellido
            </label>
            <input type="text" class="form-control"
                   style="background-color: #FFF8F3;" v-model="secondLastName"
                   id="secondLastName" required maxlength="50"
                   pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$" placeholder="Sólo se permiten letras y acentos del abecedario español"
                   title="Sólo se permiten letras y acentos del abecedario español">
          </div>
        </div>

        <div class="mb-3">
          <label for="gender" class="form-label">Género</label>
          <select id="gender" class="form-select"
                  style="background-color: #FFF8F3;" v-model="gender"
                  required>
            <option disabled value="">Seleccione una opción</option>
            <option value="masculino">Masculino</option>
            <option value="femenino">Femenino</option>
          </select>
        </div>

        <div class="mb-3">
          <label for="username" class="form-label">Nombre de usuario</label>
          <input type="text" class="form-control" v-model="username"
                 style="background-color: #FFF8F3;" id="username" required
                 maxlength="100" pattern="^[a-z_\.]+$" title="Sólo se permiten letras minúsculas, '_' y '.'"
                 placeholder="Sólo se permiten letras minúsculas, '_' y '.'" />
        </div>

        <div class="mb-3">
          <label for="password" class="form-label">Contraseña</label>
          <input type="password" class="form-control" v-model="password"
                style="background-color: #FFF8F3;" id="password" required
                maxlength="100"
                pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%&*!?])[A-Za-z\d@#$%&*!?]{10,100}$"
                title="Debe tener mínino 10 caracteres, al menos una mayúscula, una minúscula, un número y un carácter especial (@#$%&*!?)."
                placeholder="Debe tener mínino 10 caracteres, al menos una mayúscula, una minúscula, un número y un carácter especial (@#$%&*!?)." />
        </div>

        <div class="mb-3">
          <label for="idNumber" class="form-label">Cédula física</label>
          <input type="text"
                 class="form-control"
                 style="background-color: #fff8f3"
                 v-model="idNumber"
                 id="idNumber"
                 required
                 pattern="^(?!000000000)[1-79]\d{8}$"
                 placeholder="9 dígitos, sin guiones"
                 title="Debe tener 9 dígitos, empezar con 1-7 o 9 y no ser todo ceros" />
        </div>

        <div class="mb-3">
          <label class="form-label">Fecha de nacimiento</label>
          <div class="d-flex gap-2">
            <select id="birthDay" class="form-select" v-model="birthDay"
                    style="background-color: #FFF8F3;" required>
              <option value="">Día</option>
              <option v-for="day in 31" :key="day" :value="day">
                {{ day }}
              </option>
            </select>
            <select id="birthMonth" class="form-select" v-model="birthMonth"
                    style="background-color: #FFF8F3;" required>
              <option value="">Mes</option>
              <option v-for="(month, index)
                in months" :key="index" :value="index + 1">
                {{ month }}
              </option>
            </select>
            <select id="birthYear" class="form-select" v-model="birthYear"
                    style="background-color: #FFF8F3;" required>
              <option value="">Año</option>
              <option v-for="year in years" :key="year" :value="year">
                {{ year }}
              </option>
            </select>
          </div>
        </div>

        <div class="mb-3">
          <label for="phoneNumber" class="form-label">Teléfono</label>
          <div class="d-flex align-items-center mb-2">
            <span class="me-2">+506</span>
            <input type="text" class="form-control"
                   style="background-color: #FFF8F3;" v-model="phoneNumber"
                   id="phoneNumber" required pattern="\d{8}" title="8 dígitos, sin guiones"
                   placeholder="8 dígitos, sin guiones">
          </div>
        </div>

        <div class="mb-3">
          <label for="email" class="form-label">Correo electrónico</label>
          <input type="email" class="form-control"
                 style="background-color: #FFF8F3;" v-model="email" id="email"
                 required maxlength="100" placeholder="xxx@xxxx.xxx" title="Formato: xxx@xxxx.xxx"
                 @input="email = $event.target.value.toLowerCase()">
        </div>

        <h5 class="fw-normal mb-3 text-start">Dirección</h5>
        <div class="border p-3 rounded mb-3">
          <div class="row g-3">
            <div class="col-md-6">
              <label for="province" class="form-label">Provincia</label>
              <input type="text" class="form-control"
                     style="background-color: #FFF8F3;" v-model="address.province"
                     id="province" required maxlength="50" title="Sólo se permiten letras y acentos del abecedario español"
                     placeholder="Sólo se permiten letras y acentos del abecedario español"
                     pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$">
            </div>
            <div class="col-md-6">
              <label for="canton" class="form-label">Cantón</label>
              <input type="text" class="form-control"
                     style="background-color: #FFF8F3;" v-model="address.canton"
                     id="canton" required maxlength="50" title="Sólo se permiten letras y acentos del abecedario español"
                     placeholder="Sólo se permiten letras y acentos del abecedario español"
                     pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$">
            </div>
            <div class="col-md-6">
              <label for="district" class="form-label">Distrito</label>
              <input type="text" class="form-control"
                     style="background-color: #FFF8F3;" v-model="address.district"
                     id="district" required maxlength="50" title="Sólo se permiten letras y acentos del abecedario español"
                     placeholder="Sólo se permiten letras y acentos del abecedario español"
                     pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$">
            </div>
            <div class="col-md-6">
              <label for="otherSigns" class="form-label">Otras señas</label>
              <textarea class="form-control" style="background-color: #FFF8F3;
              height: 38px;" v-model="address.otherSigns" id="otherSigns"
                        maxlength="256" pattern="^[a-zA-Z0-9áéíóúÁÉÍÓÚ\s]+$"
                        rows="2" title="Sólo se permiten letras, números y espacios en blanco"
                        placeholder="Sólo se permiten letras, números y espacios en blanco">
            </textarea>
            </div>
          </div>
        </div>

        <div v-if="showError"
             class="alert alert-danger alert-dismissable fade show">
          {{errorMessage}}
          <button type="button" class="btn-close" @click="showError = false">
          </button>
        </div>

        <div class="d-flex justify-content-center">
          <button type="submit" class="btn btn-secondary"
                  style="background-color: #405D72; color: white;
            border: transparent;">
            Continuar
          </button>
        </div>
      </form>
    </div>
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
      idNumber: '',
      phoneNumber: '',
      email: '',
      firstName: '',
      secondName: '',
      firstLastName: '',
      secondLastName: '',
      username: '',
      password: '',
      address: {
        province: '',
        canton: '',
        district: '',
        otherSigns: ''
      },
      gender: '',
      birthDay: '',
      birthMonth: '',
      birthYear: '',
      months: [
        'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'
      ],
      years: this.generateYears(),
      showError: false,
      errorMessage: '',
    };
  },
  methods: {
    generateYears() {
      const current = new Date().getFullYear();
      const years = [];
      for (let y = current; y >= 1900; y--) {
        years.push(y);
      }
      return years;
    },
    submitForm: function () {
      const employerData = {
        idNumber: this.idNumber,
        phoneNumber: this.phoneNumber,
        email: this.email,
        firstName: this.firstName,
        secondName: this.secondName,
        firstLastName: this.firstLastName,
        secondLastName: this.secondLastName,
        username: this.username,
        password: this.password,
        province: this.address.province,
        canton: this.address.canton,
        district: this.address.district,
        otherSigns: this.address.otherSigns,
        gender: this.gender,
        birthDay: this.birthDay,
        birthMonth: this.birthMonth,
        birthYear: this.birthYear,
      };

      this.$api.registerEmployer(employerData)
      .then((response) => {
        if (response.data === true) {
          this.$router.push('/RegisterCompany');
        } else {
          this.errorMessage = "No se pudo registrar el empleador. \
            Verifica los datos ingresados.";
          this.showError = true;
        }
      })
      .catch((error) => {
        if (error.response) {
          this.errorMessage = error.response.data?.message ||
            "Error desconocido";
          this.showError = true;
        }
      });
    }
  }
};
</script>

<style>
  label {
    display: block;
    text-align: left;
    margin-bottom: 0.5rem;
  }
  @import '../assets/css/HeaderFooter.css';
</style>