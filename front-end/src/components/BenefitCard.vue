<template>
  <div class="card shadow-sm border-0 w-100">
    <div class="card-body py-4 px-3">
      <h5 class="card-title fw-bold mb-2" style="color: #405d72">
        {{ beneficio.nombre }}
      </h5>
      <p class="card-text text-muted" style="font-size: 0.95rem">
        {{ beneficio.descripcion }}
      </p>
      <button
        class="btn btn-sm"
        style="background-color: #405d72; color: white"
      >
        Detalles
      </button>
    </div>
  </div>
</template>

<script setup>
import { defineProps, ref } from "vue";

const props = defineProps({
  nombre: { type: String, required: true },
  descripcion: { type: String, required: true },
});

const beneficio = ref({
  nombre: props.nombre,
  descripcion: props.descripcion,
});
</script>

<style lang="scss" scoped></style>
