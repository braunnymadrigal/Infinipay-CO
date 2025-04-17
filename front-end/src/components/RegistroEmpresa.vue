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
            <a href="#" class="botones_izquierda">Personal</a>
            <a href="#" class="botones_izquierda">Empresa</a>
          </div>
          <!-- Contenedor de los enlaces principales -->
          <div class="navbar-links-center">
            <a href="#"> Inicio</a>
            <a href="#"> Sobre Nosotros</a>
            <a href="#"> ¿Tenés dudas?</a>
          </div>
          <!-- Contenedor de los botones de la derecha -->
          <div class="navbar-links-right">
            <a href="#" class="botones_derecha">Ingresar</a>
            <a href="#" class="botones_derecha">Registrá tu empresa</a>
          </div>
        </nav>
      </div>
    </header>
  </div>

  <!-- Formulario de Registro -->
  <div class="form">
    <h1 style="color: #405D72;">Registrá tu empresa</h1>
    <h2 style="color: #758694;">Datos de la empresa</h2>
    <form @submit.prevent="submit_form">

      <!-- Nombre -->
      <div class="input">
        <label for="nombre">Nombre</label>
        <input 
          type="text" 
          v-model="nombre" 
          id="nombre"
          required 
          maxlength="30"
          pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ&.\s]+$"
          title="Sólo se permiten letras y acentos del abecedario español
            , '&', '.' y espacios" 
        />
      </div>

      <!-- Descripción -->
      <div class="input">
        <label for="descripcion">Descripción de la empresa</label>
        <textarea 
          v-model="descripcion" 
          id="descripcion"
          maxlength="300"
          pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"
          placeholder="Sólo se permiten espacios, letras y acentos del abecedario español."
          rows="4"
          style="resize: vertical; width: 100%;"
        ></textarea>
      </div>

      <!-- Cédula -->
      <div class="input">
        <label for="cedula">Cédula jurídica</label>
        <input 
          type="text" 
          v-model="cedula" 
          id="cedula"
          required
          pattern="^\d{10}$"
          title="Formato: X-XXX-XXXXXX"
          placeholder="10 dígitos, sin guiones"
        />
      </div>

      <!-- Teléfono -->
      <div class="input">
        <label for="telefono">Teléfono</label>
        <div 
          v-for="(tel, index) in telefonos" 
          :key="index"
          style="display: flex; align-items: center; margin-bottom: 8px;">
          <span style="padding: 0 8px;">+506</span>
          <input 
            type="text" 
            v-model="telefonos[index]"
            :id="'telefono-' + index"
            required
            pattern="\d{8}"
            title="Formato: XXXX-XXXX"
            placeholder="8 dígitos, sin guiones"
          />
          <button
            type="button" 
            class="boton_eliminar" style="margin-left: 8px;"
            @click="borrar_telefono(index)">🗑️
          </button>
        </div>
        <button 
          type="button" class= "boton_añadir"
          @click="agregar_telefono"
          :disabled="telefonos.length >= 100">Añadir teléfono
        </button>
      </div>

      <!-- Correo Electrónico -->
      <div class="input">
        <label for="email">Correo electrónico</label>
        <input 
          type="email" 
          v-model="email" 
          id="email"
          required
          maxlength="100"
          pattern="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
          placeholder="xxx@xxxx.xxx"
        />
      </div>

      <!-- Dirección -->
      <h3 style="text-align: left; font-weight: normal;
        margin-bottom: 20px;">Dirección </h3>

      <div class="direccion" v-for="(dir, index) in direcciones" :key="index">
        <div>
          <label :for="'provincia-' + index">Provincia</label>
          <input 
            type="text" 
            v-model="dir.provincia" 
            :id="'provincia-' + index" 
            required 
            maxlength="10"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label :for="'canton-' + index">Cantón</label>
          <input 
            type="text" 
            v-model="dir.canton" 
            :id="'canton-' + index" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label :for="'distrito-' + index">Distrito</label>
          <input 
            type="text" 
            v-model="dir.distrito" 
            :id="'distrito-' + index" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label :for="'otras_señas-' + index">Otras señas</label>
          <textarea 
            v-model="dir.otras_señas" 
            :id="'otras_señas-' + index" 
            required 
            maxlength="300"
            pattern="^[a-zA-Z0-9áéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras, números y espacios en blanco"
            rows="1"
            style="resize: vertical;"
          ></textarea>
        </div>

        <button
          type="button"
          class="boton_eliminar" style="margin-top: 8px;"
          @click="borrar_direccion(index)">🗑️
        </button>
      </div>

      <button type="button"
        class="boton_añadir"
        @click="agregar_direccion" :disabled="direcciones.length >= 100">
        Añadir otra dirección
      </button>
      
      <!-- Razón social -->
      <div class="input">
        <label for="razonSocial">Razón social</label>
        <textarea 
          type="text" 
          id="razonSocial" 
          v-model="razonSocial"
          required 
          maxlength="100"
          pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s&]+$"
          placeholder="Sólo letras, acentos, espacios y '&'"
          title="Sólo se permiten letras, acentos, espacios y el símbolo '&'"
          rows="2"
        ></textarea>
      </div>
      
      <!-- Beneficios -->
      <div class="input">
        <label for="beneficios">Cantidad máxima de beneficios por
          empleado</label>
        <select id="beneficios" v-model="beneficios" required>
          <option disabled value="">Seleccione una cantidad</option>
          <option v-for="n in 100" :key="n" :value="n-1">{{ n-1 }}</option>
        </select>
      </div>

      <!-- Tipo de Pago -->
      <div class="input">
        <label for="tipo_pago">Tipo de pago</label>
        <select id="tipo_pago" v-model="tipoPago" required>
          <option disabled value="">Seleccione un tipo de pago</option>
          <option value="semana">Semanal</option>
          <option value="quincenal">Quincenal</option>
          <option value="mensual">Mensual</option>
        </select>
    </div>

    <div class="contenedor_botones">
      <button class="boton_volver" type="button" @click="back">Volver</button>
      <button class="boton_siguiente" type="submit">Terminar registro</button>
    </div>
    </form>
  </div>

</template>

<script>
export default {
  data() {
    return {
      nombre: '',
      descripcion: '',
      cedula: '',
      telefonos: [''],
      email: '',
      direcciones: [{ provincia: '', canton: '', distrito: ''
        , otras_señas: '' }],
      beneficios: 0,
      tipoPago: '',
    };
  },
  methods: {
    agregar_telefono() {
      if (this.telefonos.length < 100) {
        this.telefonos.push('');
      }
    },
    borrar_telefono(index) {
      if (this.telefonos.length > 1) {
        this.telefonos.splice(index, 1);
      }
    },
    agregar_direccion() {
      if (this.direcciones.length < 100) {
        this.direcciones.push({ provincia: '', canton: '', distrito: ''
          , otras_señas: '' });
      }
    },
    borrar_direccion(index) {
      if (this.direcciones.length > 1) {
        this.direcciones.splice(index, 1);
      }
    },
    back() {
      this.$emit('previous');
    },
    submit() {
      this.$emit('next');
    }
  }
};
</script>

<style scoped>
@import '../assets/css/RegistroEmpresa.css';
</style>