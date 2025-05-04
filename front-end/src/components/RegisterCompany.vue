<template>
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
              border: transparent;"> Iniciar sesión</router-link>
            <router-link to="/RegisterEmployer" class="btn btn-primary"
              style="background-color: #405D72;
              border: transparent;">Registrá tu empresa
            </router-link>
          </div>
          <div class="ms-auto">
            <router-link to="/" class="btn btn-secondary"
              style="background-color: #F7E7DC; color: #2b3f4e;
                border: 2px solid transparent;">Página principal
            </router-link>
          </div>
        </div>
      </nav>
    </header>

    <div class="card p-4 mx-auto" style="max-width: 1000px;
      background-color: #FFF8F3; border: none;">
      <h1 class="text-center" style="color: #405D72">Registrá tu empresa</h1>
      <h2 class="text-center" style="color: #758694">Datos de la empresa</h2>

      <form @submit.prevent="submitForm">

        <div class="mb-3">
          <label for="employerUsername" class="form-label">
            Usuario del empleador</label>
          <input type="text" class="form-control" v-model="employerUsername"
            style="background-color: #FFF8F3;" id="employerUsername" required 
            maxlength="30" pattern="^[a-z_\.]+$" title="ejemplo_usuario" 
            placeholder="Escriba el nombre de usuario que acaba de crear."
          />
        </div>

        <div class="mb-3">
          <label for="legalName" class="form-label">Razón social</label>
          <input type="text" class="form-control" id="legalName" 
            style="background-color: #FFF8F3;" v-model="legalName"
            required maxlength="256"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s&]+$"
            placeholder="Sólo letras, acentos, espacios y '&'" rows="2">
        </div>

        <div class="mb-3">
          <label for="description" class="form-label">
            Descripción de la empresa</label>
          <textarea class="form-control" style="background-color: #FFF8F3;"
            v-model="description" id="description" maxlength="256"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"
            placeholder=
            "Sólo se permiten espacios, letras y acentos del abecedario español."
            rows="4"></textarea>
        </div>

        <div class="mb-3">
          <label class="form-label">Fecha de creación</label>
          <div class="d-flex gap-2">
            <select id="creationDay" class="form-select"
              v-model="creationDay" style="background-color: #FFF8F3;" required>
              <option value="">Día</option>
              <option v-for="day in 31" :key="day" :value="day">{{ day }}
              </option>
            </select>
            <select id="creationMonth" class="form-select"
              v-model="creationMonth" style="background-color: #FFF8F3;"
              required>
              <option value="">Mes</option>
              <option v-for="(month, index)
                in months" :key="index" :value="index + 1"> {{ month }}
              </option>
            </select>
            <select id="creationYear" class="form-select" v-model="creationYear"
              style="background-color: #FFF8F3;" required>
              <option value="">Año</option>
              <option v-for="year in years" :key="year" :value="year">
                {{ year }}</option>
            </select>
          </div>
        </div>

        <div class="mb-3">
          <label for="idNumber" class="form-label">Cédula jurídica</label>
          <input type="text" class="form-control"
            style="background-color: #FFF8F3;" v-model="idNumber"
            id="idNumber" required pattern="^\d{10}$"
            placeholder="10 dígitos, sin guiones">
        </div>

        <div class="mb-3">
          <label for="phoneNumber" class="form-label">Teléfono</label>
          <div class="d-flex align-items-center mb-2">
            <span class="me-2">+506</span>
            <input type="text" class="form-control"
            style="background-color: #FFF8F3;" v-model="phoneNumber"
            id="phoneNumber" required pattern="\d{8}"
            placeholder="8 dígitos, sin guiones">
          </div>
        </div>

        <div class="mb-3">
          <label for="email" class="form-label">Correo electrónico</label>
          <input type="email" class="form-control"
            style="background-color: #FFF8F3;" v-model="email" id="email"
            required maxlength="100" placeholder="xxx@xxxx.xxx"
          @input="email = $event.target.value.toLowerCase()">
        </div>

        <h3 class="fw-normal mb-3">Dirección</h3>
        <div class="border p-3 rounded mb-3">
          <div class="row g-3">
            <div class="col-md-6">
              <label for="province" class="form-label">Provincia</label>
              <input type="text" class="form-control"
              style="background-color: #FFF8F3;" v-model="address.province"
              id="province" required maxlength="50"
              pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$">
            </div>
            <div class="col-md-6">
              <label for="canton" class="form-label">Cantón</label>
              <input type="text" class="form-control"
              style="background-color: #FFF8F3;" v-model="address.canton"
              id="canton" required maxlength="50"
              pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$">
            </div>
            <div class="col-md-6">
              <label for="district" class="form-label">Distrito</label>
              <input type="text" class="form-control"
              style="background-color: #FFF8F3;" v-model="address.district"
              id="district" required maxlength="50"
              pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$">
            </div>
            <div class="col-md-6">
              <label for="otherSigns" class="form-label">Otras señas</label>
              <textarea class="form-control" style="background-color: #FFF8F3;
              height: 38px;" v-model="address.otherSigns" id="otherSigns"
              maxlength="256" pattern="^[a-zA-Z0-9áéíóúÁÉÍÓÚ\s]+$"
              rows="2" placeholder=
              "Sólo se permiten letras, números y espacios en blanco">
            </textarea>
            </div>
          </div>
        </div>

        <div class="mb-3">
          <label for="benefits" class="form-label">
            Cantidad máxima de beneficios por empleado</label>
          <select id="benefits" class="form-select" 
           style="background-color: #FFF8F3;" v-model="benefits" required>
            <option disabled value="">Seleccione una cantidad</option>
            <option v-for="n in 100" :key="n" :value="n-1">{{ n-1 }}</option>
          </select>
        </div>

        <div class="mb-3">
          <label for="paymentType" class="form-label">Tipo de pago</label>
          <select id="paymentType" class="form-select"
            style="background-color: #FFF8F3;" v-model="paymentType"
            required>
            <option disabled value="">Seleccione un tipo de pago</option>
            <option value="semanal">Semanal</option>
            <option value="quincenal">Quincenal</option>
            <option value="mensual">Mensual</option>
          </select>
        </div>
        
        <div class="d-flex justify-content-center mt-4" style="gap: 10px;">
          <router-link to="/RegisterEmployer" class="btn btn-secondary"
            style="background-color: #405D72;"> Volver</router-link>
          <button type="submit" class="btn btn-secondary"
            style="background-color: #405D72; border: transparent;">
            Registrar
          </button>
        </div>
      </form>
    </div>
  </div>

  <footer class="py-5 custom-footer">
    <div class="container">
      <div class="row">
        <div class="col-md-3 mb-3">
          <p class="h5" style="margin-left: 10px;">Infinipay CO.</p>
          <div>
            <a href="#" class="fa fa-facebook"></a>
            <a href="#" class="fa fa-linkedin"></a>
            <a href="#" class="fa fa-youtube"></a>
            <a href="#" class="fa fa-instagram"></a>
          </div>
        </div>
        <div class="col-md-3 mb-3">
          <p class="h5">Empresa y equipo</p>
          <a href="#">Sobre nosotros</a>
        </div>
        <div class="col-md-3 mb-3">
          <p class="h5">Recursos</p>
          <a href="#">¿Cómo registro mi empresa?</a><br />
          <a href="#">¿Cómo registro empleados a mi empresa?</a><br />
          <a href="#">¿Cómo accedo a mi perfil?</a>
        </div>
        <div class="col-md-3 mb-3">
          <p class="h5">Contacto</p>
          <p><i class="pi pi-phone" style="color:#405D72;"></i>
            +506 2000-0000</p>
          <p><i class="pi pi-home" style="color:#405D72;"></i>
            San José, Montes de Oca, San Pedro</p>
        </div>
      </div>
    </div>
  </footer>
</template> 

<script>
import axios from "axios";
export default {
  data() {
    return {
      legalName: '',
      description: '',
      idNumber: '',
      phoneNumber: '',
      email: '',
      employerUsername: '',
      address: {
        province: '',
        canton: '',
        district: '',
        otherSigns: ''
      },
      benefits: 0,
      paymentType: '',
      creationDay: '',
      creationMonth: '',
      creationYear: '',
      months: [
        'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
        'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'
      ],
      years: this.generateYears()
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
    submitForm() {
      axios.post("https://localhost:7275/api/Company", {
        legalName: this.legalName,
        description: this.description,
        idNumber: this.idNumber,
        phoneNumber: this.phoneNumber,
        email: this.email,
        employerUsername: this.employerUsername,
        province: this.address.province,
        canton: this.address.canton,
        district: this.address.district,
        otherSigns: this.address.otherSigns,
        benefits: this.benefits,
        paymentType: this.paymentType,
        creationDay: this.creationDay,
        creationMonth: this.creationMonth,
        creationYear: this.creationYear,
      })
      .then(function(response) {
        console.log("Respuesta del servidor:", response.data);
        if (response.data === true) {
          alert('¡Empresa registrada exitosamente!');
          this.$router.push('/'); // De momento redirigir a la página principal
        } else {
          alert(
            "No se pudo registrar el empleador. Verifica los datos ingresados."
          );
        }
      }.bind(this))
      .catch(function(error) {
        console.error("Error:", error);
        if (error.response) {
          const message = error.response.data?.message || "Error desconocido";
          alert(message);
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