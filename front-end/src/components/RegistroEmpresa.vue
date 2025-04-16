<template>
  <div>
    <h2>Datos de la empresa</h2>
    <form @submit.prevent="submitForm">

      <div class="button-container">
        <button @click="$emit('previous')">Volver</button>
        <button @click="submitForm">Siguiente</button>
      </div>

      <button type="submit">Terminar registro</button>
    </form>
  </div>
</template>

<script>
export default {
  data() {
    return {
    };
  },
  methods: {
    submitForm() {
    }
  }
};
</script>
