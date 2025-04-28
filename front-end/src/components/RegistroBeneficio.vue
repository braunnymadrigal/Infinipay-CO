<template>
  <div>
    <!-- Logo de la empresa -->
    <div class="logo_empresa">
      <img
        src="../assets/images/logo.png"
        alt="Logo de la empresa"
        class="logo_img"
      />
    </div>
    <!-- Header -->
    <header class="header">
      <div class="navbar">
        <nav class="navbar-links">
          <!-- Contenedor de los botones de la izquierda -->
          <div class="navbar-links-left">
            <a href="#" class="botones_izquierda">Iniciar sesión</a>
            <router-link to="/registrar_empleador" class="botones_izquierda"
              >Registrá tu empresa</router-link
            >
          </div>
          <!-- Contenedor de los botones de la derecha -->
          <div class="navbar-links-right">
            <router-link to="/" class="botones_derecha"
              >Página principal</router-link
            >
          </div>
        </nav>
      </div>
    </header>
  </div>
  <!-- Formulario de Registro -->
  <div class="form">
    <h1 style="color: #405d72">Agregar Beneficio</h1>
    <h2 style="color: #758694; margin-bottom: 50px">
      Informacion del beneficio
    </h2>
    <form @submit.prevent="submit_form">
      <!-- Nombre -->
      <div class="caja_input">
        <label class="label" for="nombre">Nombre</label>
        <input
          class="input"
          type="text"
          v-model="nombre"
          id="nombre"
          required
          maxlength="100"
          pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ&.\s]+$"
          title="Sólo se permiten letras y acentos del abecedario español, '&', '.' y espacios"
        />
      </div>

      <!-- Descripción -->
      <div class="caja_input">
        <label class="label" for="descripcion">Descripción de la empresa</label>
        <textarea
          class="textarea"
          v-model="descripcion"
          id="descripcion"
          required
          maxlength="300"
          pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"
          placeholder="Sólo se permiten espacios, letras y acentos del abecedario español."
          rows="4"
          style="resize: vertical; width: 100%"
        ></textarea>
      </div>

      <!-- Datos de auditoría -->
      <div class="caja_input">
        <label class="label" for="descripcion">Datos de auditoría</label>
        <textarea
          class="textarea"
          v-model="datos_auditoria"
          id="datos_auditoria"
          required
          maxlength="300"
          pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"
          placeholder="Sólo se permiten espacios, letras y acentos del abecedario español."
          rows="4"
          style="resize: vertical; width: 100%"
        ></textarea>
      </div>
      <div class="contenedor_botones">
        <router-link to="/"
          ><button class="boton_volver">Registrar</button></router-link
        >
        <router-link to="/">
          <button class="boton_siguiente" type="submit">Cancelar</button>
        </router-link>
      </div>
    </form>
  </div>
</template>

<script setup>
import { ref } from "vue";

const datos_auditoria = ref("");
const nombre = ref("");
const descripcion = ref("");

function submit_form() {
  console.log("Nombre:", nombre.value);
  console.log("Descripción:", descripcion.value);
  console.log("Datos de auditoría:", datos_auditoria.value);
  // axios.post("http://localhost:3000/beneficios", {
}
</script>

<style lang="scss" scoped></style>
