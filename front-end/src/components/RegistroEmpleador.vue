<template>
  <div>
    <!-- Logo de la empresa -->
    <div class="logo_empresa">
      <img src="../assets/images/logo.png" alt="Logo de la empresa"
        class="logo_img"/>
    </div>
    <!-- Header -->
    <header class="header">
      <div class="navbar">
        <nav class="navbar-links">
          <!-- Contenedor de los botones de la izquierda -->
          <div class="navbar-links-left">
            <a href="#" class="botones_izquierda">Iniciar sesión</a>
            <router-link to="/registrar_empleador" class="botones_izquierda">Registrá tu empresa</router-link>
          </div>
          <!-- Contenedor de los botones de la derecha -->
          <div class="navbar-links-right">
            <router-link to="/" class="botones_derecha">Página principal</router-link>
          </div>
        </nav>
      </div>
    </header>
  </div>

  <!-- Formulario de Registro -->
  <div class="form">
    <h1 style="color: #405D72;">Registrá tu empresa</h1>
    <h2 style="color: #758694; margin-bottom: 50px;">Datos del dueño de la empresa</h2>
    <form @submit.prevent="submitForm">

      <!-- Primer y segundo nombre -->
      <div class="nombre">
        <div>
          <label class="label" for="primer_nombre">Primer nombre</label>
          <input class="input"
            type="text" 
            v-model="primer_nombre" 
            id="primer_nombre" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label class="label" for="segundo_nombre">Segundo nombre</label>
          <input class="input"
            type="text" 
            v-model="segundo_nombre" 
            id="segundo_nombre" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>
      </div>

      <!-- Primer y segundo apellido -->
      <div class="nombre">
        <div>
          <label class="label" for="primer_apellido">Primer apellido</label>
          <input class="input"
            type="text" 
            v-model="primer_apellido" 
            id="primer_apellido" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label class="label" for="segundo_apellido">Segundo apellido</label>
          <input class="input"
            type="text" 
            v-model="segundo_apellido" 
            id="segundo_apellido" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>
      </div>

      <!-- Cédula -->
      <div class="caja_input">
        <label class="label" for="cedula">Cédula</label>
        <input class="input"
          type="text" 
          v-model="cedula" 
          id="cedula"
          required
          pattern="^\d{9}$"
          title="Formato: X-XXXX-XXXX"
          placeholder="9 dígitos, sin guiones"
        />
      </div>

      <!-- Nombre de usuario -->
      <div class="caja_input">
        <label class="label" for="usuario">Nombre de usuario</label>
        <input class="input"
          type="text" 
          v-model="usuario" 
          id="usuario"
          required 
          maxlength="30"
          pattern="^[a-z_\.]+$" 
          title="ejemplo_de_usuario" 
          placeholder="Sólo se permiten letras minúsculas, '_' y '.'"
        />
      </div>

      <!-- Contraseña -->
      <div class="caja_input">
        <label class="label" for="password">Contraseña</label>
        <input class="input"
          type="password" 
          v-model="password" 
          id="password"
          required
          minlength="10"
          maxlength="100"
          pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*?])[A-Za-z\d!@#$%^&*?]{10,100}$"
          title="La contraseña debe contener al menos una mayúscula, una minúscula, un número y un carácter especial"
          placeholder="Al menos 10 caracteres"
        />
      </div>

      <div class="caja_input">
        <label class="label" for="telefono">Teléfono</label>
        <div style="display: flex; align-items: center;">
          <span style="padding: 0 8px;">+506</span>
          <input class="input"
            type="text" 
            v-model="telefono" 
            id="telefono"
            required
            pattern="\d{8}"
            title="Formato: XXXX-XXXX"
            placeholder="8 dígitos, sin guión"
          />
        </div>
      </div>

      <!-- Correo Electrónico -->
      <div class="caja_input">
        <label class="label" for="email">Correo electrónico</label>
        <input class="input"
          type="email" 
          v-model="email" 
          id="email"
          required
          maxlength="100"
          pattern="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
          placeholder="xxx@xxxx.xxx"
        />
      </div>

      <h3 style="text-align: left; font-weight: normal; margin-bottom: 20px;">Dirección</h3>

      <!-- Dirección -->
      <div class="direccion">
        <div>
          <label class="label" for="provincia">Provincia</label>
          <input class="input"
            type="text" 
            v-model="provincia" 
            id="provincia" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label class="label" for="canton">Cantón</label>
          <input class="input"
            type="text" 
            v-model="canton" 
            id="canton" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label class="label" for="distrito">Distrito</label>
          <input class="input"
            type="text" 
            v-model="distrito" 
            id="distrito" 
            required 
            maxlength="100"
            pattern="^[a-zA-Z0-9áéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>
  
      <div>
        <label class="label" for="otras_señas">Otras señas</label>
        <textarea class="textarea"
          type="text" 
          v-model="otras_señas" 
          id="otras_señas" 
          required 
          maxlength="100"
          pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
          placeholder="Sólo se permiten números, letras y acentos del abecedario español"
          rows="1"
          style="resize: vertical; margin-top: 15px;"
        ></textarea>
      </div>
    </div>

    <button class="boton_siguiente" type="submit">Siguiente</button>
    </form>
  </div>
</template>

<script>
export default {
  data() {
    return {
      primer_nombre: '',
      segundo_nombre: '',
      primer_apellido: '',
      segundo_apellido: '',
      cedula: '',
      usuario: '',
      password: '',
      telefono: '',
      email: '',
      provincia: '',
      canton: '',
      distrito: '',
      otras_señas: ''
    };
  },
  methods: {
    submitForm() {
      this.$router.push('/registrar_empresa');
    }
  }
};
</script>

<style scoped>
@import '../assets/css/RegistroEmpleador.css';
</style>