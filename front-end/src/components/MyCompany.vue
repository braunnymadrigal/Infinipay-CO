<template>

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
        company: {
          name: "Cooperativa de Productores de Leche Dos Pinos R.L",
          description: "Somos la Cooperativa de Productores de Leche Dos Pinos, empresa de alimentos 100% costarricense y líder en la industria láctea de Centroamérica y Caribe. Contamos con una amplia oferta de productos de consumo humano que supera las 900 variedades. Adicionalmente, a través de la División Agrocomercial, producimos alimentos balanceados para animales y comercializamos más de 3.000 productos para el sector agropecuario nacional. Tenemos presencia productiva en varios países de Centroamérica y exportamos a 10 mercados del mundo; avanzando a paso firme para consolidarnos como una transnacional de alimentos de primerísima calidad y alto valor agregado, fortaleciendo el bienestar, la salud y la nutrición, consecuente con su promesa  de Siempre con Algo Mejor.",
          email: "centrodecontactos@dospinos.com",
          phoneNumber: "2508-2525",
          identityCard: "3004045002",
          paymentType: "quincenal",
          numberOfBenefits: 3,
          birthDate: "24/12/1954",
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