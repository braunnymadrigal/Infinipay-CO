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
            <a href="#" class="btn btn-outline-primary me-2"
              style="background-color: #405D72; color: #FFFFFF;
                border: transparent;"> Iniciar sesión</a>
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
          <label for="legalName" class="form-label">Razón social</label>
          <textarea class="form-control" id="legalName" 
            style="background-color: #FFF8F3;" v-model="legalName"
            required maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s&]+$"
            placeholder="Sólo letras, acentos, espacios y '&'" rows="2">
          </textarea>
        </div>

        <div class="mb-3">
          <label for="description" class="form-label">
            Descripción de la empresa</label>
          <textarea class="form-control" style="background-color: #FFF8F3;"
            v-model="description" id="description" maxlength="300"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"
            placeholder=
            "Sólo se permiten espacios, letras y acentos del abecedario español."
            rows="4"></textarea>
        </div>

        <div class="mb-3">
          <label for="idNumber" class="form-label">Cédula jurídica</label>
          <input type="text" class="form-control"
            style="background-color: #FFF8F3;" v-model="idNumber"
            id="idNumber" required pattern="^\d{10}$"
            placeholder="10 dígitos, sin guiones">
        </div>

        <div class="mb-3">
          <label class="form-label">Teléfono</label>
          <div v-for="(phoneNumber, index) in phoneNumbers" :key="index"
            class="d-flex align-items-center mb-2">
            <span class="me-2">+506</span>
            <input type="text" class="form-control me-2" 
              style="background-color: #FFF8F3;" v-model="phoneNumbers[index]" 
              :id="'phoneNumbers-' + index" required pattern="\d{8}"
                placeholder="8 dígitos, sin guiones">
            <button type="button" class="btn btn-danger btn-sm"
              @click="deletePhoneNumber(index)">🗑️</button>
          </div>

          <div class="d-flex justify-content-center">
            <button type="button" class="btn btn-primary mt-2"
              style="background-color: #405D72; border: transparent;"
              @click="addPhoneNumber" :disabled="phoneNumbers.length >= 100">
              Añadir otro teléfono
            </button>
          </div>

        </div>

        <div class="mb-3">
          <label for="email" class="form-label">Correo electrónico</label>
          <input type="email" class="form-control"
            style="background-color: #FFF8F3;"
            v-model="email" id="email" required maxlength="100"
            pattern="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
            placeholder="xxx@xxxx.xxx">
        </div>

        <h3 class="fw-normal mb-3">Dirección</h3>
        <div v-for="(addr, index) in addresses" :key="index"
          class="border p-3 rounded mb-3">
          <div class="row g-3">
            <div class="col-md-6">
              <label :for="'province-' + index" class="form-label">
                Provincia</label>
              <input type="text" class="form-control"
                style="background-color: #FFF8F3;" v-model="addr.province"
                :id="'province-' + index" required maxlength="10"
                pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$">
            </div>
            <div class="col-md-6">
              <label :for="'canton-' + index" class="form-label">Cantón</label>
              <input type="text" class="form-control" 
                style="background-color: #FFF8F3;" v-model="addr.canton"
                :id="'canton-' + index" required maxlength="100"
                pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$">
            </div>
            <div class="col-md-6">
              <label :for="'district-' + index" class="form-label">
                Distrito
              </label>
              <input type="text" class="form-control"
                style="background-color: #FFF8F3;" v-model="addr.district"
                :id="'district-' + index" required maxlength="100"
                pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$">
            </div>
            <div class="col-md-6">
              <label :for="'otherSigns-' + index" class="form-label">
                Otras señas
              </label>
              <textarea class="form-control" style="background-color: #FFF8F3;
                height: 38px;" v-model="addr.otherSigns"
                :id="'otherSigns-' + index" required maxlength="300"
                pattern="^[a-zA-Z0-9áéíóúÁÉÍÓÚ\s]+$" rows="2"
                placeholder=
                  "Sólo se permiten letras, números y espacios en blanco"
              ></textarea>
            </div>
          </div>
          <div class="text-end mt-2">
            <button type="button" class="btn btn-danger btn-sm"
              @click="deleteAddress(index)">🗑️</button>
          </div>
        </div>

        <div class="d-flex justify-content-center">
          <button type="button" class="btn btn-primary mb-3"
            style="background-color: #405D72; border: transparent;"
            @click="addAddress" :disabled="addresses.length >= 100">
            Añadir otra dirección
          </button>
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
            <option value="weekly">Semanal</option>
            <option value="biweekly">Quincenal</option>
            <option value="monthly">Mensual</option>
          </select>
        </div>
        
        <div class="d-flex justify-content-center mt-4" style="gap: 10px;">
          <router-link to="/RegisterEmployer" class="btn btn-secondary"
            style="background-color: #405D72;"> Volver</router-link>
          <button type="submit" class="btn btn-success"
            style="background-color: #405D72; border: transparent;">
            Terminar registro
          </button>
        </div>
      </form>
    </div>
  </div>
</template> 

<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router';

export default {
  setup() {
    const router = useRouter();
    const legalName = ref('');
    const description = ref('');
    const idNumber = ref('');
    const phoneNumbers = ref(['']);
    const email = ref('');
    const addresses = ref([
      {
        province: '',
        canton: '',
        district: '',
        otherSigns: ''
      }
    ]);
    const benefits = ref(0);
    const paymentType = ref('');

    function addPhoneNumber() {
      if (phoneNumbers.value.length < 100) {
        phoneNumbers.value.push('');
      }
    }

    function deletePhoneNumber(index) {
      if (phoneNumbers.value.length > 1) {
        phoneNumbers.value.splice(index, 1);
      }
    }

    function addAddress() {
      if (addresses.value.length < 100) {
        addresses.value.push({
          province: '',
          canton: '',
          district: '',
          otherSigns: ''
        });
      }
    }

    function deleteAddress(index) {
      if (addresses.value.length > 1) {
        addresses.value.splice(index, 1);
      }
    }

    function submitForm() {
      alert('¡Empresa registrada exitosamente!');
      router.push('/');
    }

    return {
      description,
      legalName,
      idNumber,
      phoneNumbers,
      email,
      addresses,
      benefits,
      paymentType,
      addPhoneNumber,
      deletePhoneNumber,
      addAddress,
      deleteAddress,
      submitForm
    };
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