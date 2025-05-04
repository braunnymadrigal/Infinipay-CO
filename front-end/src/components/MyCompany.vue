<template>
  <CompanyHeader/>

  <div v-if="!showPopup" @click.stop 
  class="card p-4 mx-auto bg-transparent border-0 w-50" >
    <h1
      class="text-center font-weight-bold"
      style="color: #405D72">
      Mi empresa
    </h1>

    <form @submit.prevent="editExample">
      <div class="mb-3">
        <label
          for="name"
          class="form-label">
          Razón social
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="name"
          v-model="company.Name" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="description"
          class="form-label">
          Descripción
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="description"
          v-model="company.Description" 
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
          v-model="company.Email" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="phone"
          class="form-label">
          Teléfono
        </label>
        <div class="d-flex align-items-center mb-2">
          <div class="me-3 text-start">+506</div>
          <input
            class="form-control bg-transparent"
            type="text"
            id="phone"
            v-model="company.Phone" 
            readonly
          >
        </div>
      </div>

      <div class="mb-3">
        <label
          for="document"
          class="form-label">
          Cédula jurídica
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="document"
          v-model="company.Document" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="paymentType"
          class="form-label">
          Tipo de pago
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="paymentType"
          v-model="company.PaymentType" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="benefits"
          class="form-label">
          Cantidad máxima de beneficios por empleado
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="benefits"
          v-model="company.Benefits" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="birth"
          class="form-label">
          Fecha de creación
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="birth"
          v-model="company.Birth" 
          readonly
        >
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
            v-model="company.Province"
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
            v-model="company.Canton" 
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
            v-model="company.District" 
            readonly
          >
        </div>
      </div>

      <div class="mb-3">
        <label
          for="address"
          class="form-label">
          Dirección exacta
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="address"
          v-model="company.Address" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="owner"
          class="form-label">
          Dueño de la empresa
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="owner"
          v-model="company.Owner" 
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

  <div v-if="showPopup" @click.stop 
  class="d-flex justify-content-center my-5 py-5">
    <div class="display-1 text-danger" style="padding: 150px;">
      No tiene permisos para ver esta información.
    </div>
  </div> 

  <!-- FOOTER -->
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
        company: {
          Name: "",
          Description: "",
          Email: "",
          Phone: "",
          Document: "",
          PaymentType: "",
          Benefits: "",
          Birth: "",
          Province: "",
          Canton: "",
          District: "",
          Address: "",
          Owner: "",
        },
      };
    },
    methods: {
      editExample() {
        alert("Próximamente llegará la opción 'Editar'.");
      },

      getCompany() {
        let jwtCookie = this.$cookies.get('jwt');
        axios.get("https://localhost:7275/api/MyCompany", 
        { headers: {"Authorization" : `Bearer ${jwtCookie}`} })
          .then(
            response => {
              this.showPopup = false;
              this.company = response.data;
            }
          )
          .catch(
            error => {
              this.showPopup = true;
              console.log(error);
            }
          )
        ;
      },
    },
    mounted() {
      this.getCompany();
    },
  };
</script>

<style>
  @import '../assets/css/HeaderFooter.css';
</style>