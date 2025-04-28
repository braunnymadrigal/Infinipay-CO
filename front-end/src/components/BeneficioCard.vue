<template>
  <div>
    <div style="max-width: 600px; width: 100%">
      <div>
        <h5>{{ beneficio.nombre }}</h5>

        <p>
          {{ beneficio.descripcion }}
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { defineProps } from "vue";
import { ref } from "vue";

const props = defineProps({
  nombre: {
    type: String,
    required: true,
  },
  descripcion: {
    type: String,
    required: true,
  },
});

const beneficio = ref({
  nombre: props.nombre,
  descripcion: props.descripcion,
});
</script>

<style lang="scss" scoped></style>
