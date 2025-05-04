<template>
  <CompanyHeader/>

  <div class="card p-4 mx-auto bg-transparent border-0 w-50" >
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
          v-model="company.name" 
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
          v-model="company.description" 
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
          v-model="company.email" 
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
            v-model="company.phoneNumber" 
            readonly
          >
        </div>
      </div>

      <div class="mb-3">
        <label
          for="identityDocument"
          class="form-label">
          Cédula jurídica
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="identityDocument"
          v-model="company.identityDocument" 
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
          v-model="company.paymentType" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="numberOfBenefits"
          class="form-label">
          Cantidad máxima de beneficios por empleado
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="numberOfBenefits"
          v-model="company.numberOfBenefits" 
          readonly
        >
      </div>

      <div class="mb-3">
        <label
          for="numberOfBenefits"
          class="birthDate">
          Fecha de creación
        </label>
        <input
          class="form-control bg-transparent"
          type="text"
          id="birthDate"
          v-model="company.birthDate" 
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
            v-model="company.province"
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
            v-model="company.canton" 
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
            v-model="company.district" 
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
          v-model="company.exactAddress" 
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
          v-model="company.owner" 
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
  <!-- FOOTER -->
  <MainFooter/>
</template>

<script>
  import MainFooter from "./MainFooter.vue";
  import CompanyHeader from "./HeaderCompany.vue";
  export default {
    components: {
      MainFooter,
      CompanyHeader,
    },
    data() {
      return {
        company: {
          name: "Cooperativa de Productores de Leche Dos Pinos R.L",
          description: "Somos la Cooperativa de Productores de Leche Dos Pinos, empresa de alimentos 100% costarricense y líder en la industria láctea de Centroamérica y Caribe. Contamos con una amplia oferta de productos de consumo humano que supera las 900 variedades. Adicionalmente, a través de la División Agrocomercial, producimos alimentos balanceados para animales y comercializamos más de 3.000 productos para el sector agropecuario nacional. Tenemos presencia productiva en varios países de Centroamérica y exportamos a 10 mercados del mundo; avanzando a paso firme para consolidarnos como una transnacional de alimentos de primerísima calidad y alto valor agregado, fortaleciendo el bienestar, la salud y la nutrición, consecuente con su promesa  de Siempre con Algo Mejor.",
          email: "centrodecontactos@dospinos.com",
          phoneNumber: "2508-2525",
          identityDocument: "3004045002",
          paymentType: "quincenal",
          numberOfBenefits: 3,
          birthDate: "31/12/1900",
          province: "Alajuela",
          canton: "Grecia",
          district: "San Roque",
          exactAddress: "DEL AEROPUERTO JUAN SANTAMARÍA 6 KM AL OESTE, CONTIGUO A ZONA FRANCA",
          owner: "Gonzalo Chaves Alvarado",
        },
      };
    },
    methods: {
      editExample() {
        console.log("Saved:", this.company);
      },
    },
  };
</script>

<style>
  @import '../assets/css/HeaderFooter.css';
</style>