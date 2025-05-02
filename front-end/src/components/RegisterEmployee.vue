<template>
  <div class="container my-5">
    <div class="text-center mb-4">
      <img
        src="../assets/images/logo.png"
        alt="Company logo"
        class="img-fluid"
        style="max-width: 350px"
      />
    </div>

    <header class="mb-5 custom-header">
      <nav class="navbar navbar-expand-lg rounded custom-navbar">
        <div class="container-fluid">
          <div class="d-flex">
            <router-link
              to="/EmployerProfile"
              class="mx-2"
              style="color: #405d72"
              >Perfil</router-link
            >
            <a href="#" class="mx-2" style="color: #405d72">Empresa</a>
            <router-link to="/BenefitList" class="mx-2" style="color: #405d72"
              >Beneficios</router-link
            >
            <a href="#" class="mx-2" style="color: #405d72">Empleados</a>
          </div>
        </div>
      </nav>
    </header>

    <div
      class="card p-4 mx-auto"
      style="max-width: 1000px; background-color: #fff8f3; border: none"
    >
      <h1 class="text-center" style="color: #405d72">¡Bienvenido!</h1>
      <h2 class="text-center" style="color: #758694">
        Creá un perfil para tu empleado
      </h2>

      <form @submit.prevent="submitForm">
        <div class="row mb-3 justify-content-center" style="margin-top: 30px">
          <div class="col-md-6 col-lg-6">
            <label for="firstName" class="form-label">Primer nombre</label>
            <input
              type="text"
              class="form-control"
              style="background-color: #fff8f3"
              v-model="firstName"
              id="firstName"
              required
              maxlength="100"
              pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
              title="Sólo se permiten letras y acentos del abecedario español"
            />
          </div>

          <div class="col-md-6 col-lg-6">
            <label for="secondName" class="form-label">Segundo nombre</label>
            <input
              type="text"
              class="form-control"
              style="background-color: #fff8f3"
              v-model="secondName"
              id="secondName"
              required
              maxlength="100"
              pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
              title="Sólo se permiten letras y acentos del abecedario español"
            />
          </div>
        </div>

        <div class="row mb-3 justify-content-center">
          <div class="col-md-6 col-lg-6">
            <label for="firstLastName" class="form-label"
              >Primer apellido
            </label>
            <input
              type="text"
              class="form-control"
              style="background-color: #fff8f3"
              v-model="firstLastName"
              id="firstLastName"
              required
              maxlength="100"
              pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
              title="Sólo se permiten letras y acentos del abecedario español"
            />
          </div>

          <div class="col-md-6 col-lg-6">
            <label for="secondLastName" class="form-label">
              Segundo apellido</label
            >
            <input
              type="text"
              class="form-control"
              style="background-color: #fff8f3"
              v-model="secondLastName"
              id="secondLastName"
              required
              maxlength="100"
              pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
              title="Sólo se permiten letras y acentos del abecedario español"
            />
          </div>
        </div>

        <div class="mb-3">
          <label for="gender" class="form-label">
            Género</label>
          <select id="gender" class="form-select" 
           style="background-color: #FFF8F3;" v-model="gender" required>
            <option disabled value="">Seleccione una opción</option>
            <option value="weekly">Masculino</option>
            <option value="biweekly">Femenino</option>
          </select>
        </div>

        <div class="mb-3">
          <label for="idNumber" class="form-label">Cédula</label>
          <input
            type="text"
            class="form-control"
            style="background-color: #fff8f3"
            v-model="idNumber"
            id="idNumber"
            required
            pattern="^\d{9}$"
            placeholder="9 dígitos, sin guiones"
          />
        </div>

        <div class="mb-3">
          <label class="form-label" for="role">Rol</label>
          <select
            id="role"
            class="form-select"
            v-model="role"
            required
            style="background-color: #fff8f3"
          >
            <option disabled value="">Seleccione el rol del empleado</option>
            <option value="supervisor">Supervisor</option>
            <option value="employee">Empleado</option>
            <option value="administrator">Administrador</option>
          </select>
        </div>

        <div class="mb-3">
          <label for="username" class="form-label">Nombre de usuario</label>
          <input
            type="text"
            class="form-control"
            v-model="username"
            style="background-color: #fff8f3"
            id="username"
            required
            maxlength="30"
            pattern="^[a-z_\.]+$"
            title="ejemplo_usuario"
            placeholder="Sólo se permiten letras minúsculas, '_' y '.'"
          />
        </div>

        <div class="mb-3">
          <label for="phoneNumber" class="form-label">Teléfono</label>
          <div class="d-flex align-items-center mb-2">
            <span class="me-2">+506</span>
            <input
              type="text"
              class="form-control"
              style="background-color: #fff8f3"
              v-model="phoneNumber"
              id="phoneNumber"
              required
              pattern="\d{8}"
              placeholder="8 dígitos, sin guiones"
            />
          </div>
        </div>

        <div class="mb-3">
          <label for="email" class="form-label">Correo electrónico</label>
          <input
            type="email"
            class="form-control"
            style="background-color: #fff8f3"
            v-model="email"
            id="email"
            required
            maxlength="100"
            pattern="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
            placeholder="xxx@xxxx.xxx"
          />
        </div>

        <h5 class="fw-normal mb-3 text-start">Dirección</h5>
        <div class="border p-3 rounded mb-3">
          <div class="row g-3">
            <div class="col-md-6">
              <label for="province" class="form-label">Provincia</label>
              <input
                type="text"
                class="form-control"
                style="background-color: #fff8f3"
                v-model="address.province"
                id="province"
                required
                maxlength="10"
                pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
              />
            </div>
            <div class="col-md-6">
              <label for="canton" class="form-label">Cantón</label>
              <input
                type="text"
                class="form-control"
                style="background-color: #fff8f3"
                v-model="address.canton"
                id="canton"
                required
                maxlength="100"
                pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
              />
            </div>
            <div class="col-md-6">
              <label for="district" class="form-label">Distrito</label>
              <input
                type="text"
                class="form-control"
                style="background-color: #fff8f3"
                v-model="address.district"
                id="district"
                required
                maxlength="100"
                pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
              />
            </div>
            <div class="col-md-6">
              <label for="otherSigns" class="form-label">Otras señas</label>
              <textarea
                class="form-control"
                style="background-color: #fff8f3; height: 38px"
                v-model="address.otherSigns"
                id="otherSigns"
                required
                maxlength="300"
                pattern="^[a-zA-Z0-9áéíóúÁÉÍÓÚ\s]+$"
                rows="2"
                placeholder="Sólo se permiten letras, números y espacios en blanco"
              >
              </textarea>
            </div>
          </div>
        </div>

        <div class="d-flex justify-content-center">
          <button type="submit" class="btn btn-success" 
            style="background-color: #405D72; color: white;
            border: transparent;">
            Crear
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
          <p>
            <i class="pi pi-phone" style="color: #405d72"></i> +506 2000-0000
          </p>
          <p>
            <i class="pi pi-home" style="color: #405d72"></i> San José, Montes
            de Oca, San Pedro
          </p>
        </div>
      </div>
    </div>
  </footer>
</template>

<script>
import { ref } from "vue";
import { useRouter } from "vue-router";

export default {
  setup() {
    const router = useRouter()
    const firstName = ref('')
    const secondName = ref('')
    const firstLastName = ref('')
    const secondLastName = ref('')
    const idNumber = ref('')
    const username = ref('')
    const password = ref('')
    const phone = ref('')
    const email = ref('')
    const role = ref('')
    const gender = ref('')

    const address = ref({
      province: "",
      canton: "",
      district: "",
      otherSigns: "",
    });
    function submitForm() {
      alert("¡Empleado registrado exitosamente!");
      router.push("/EmployerProfile");
    }

    return {
      firstName,
      secondName,
      firstLastName,
      secondLastName,
      idNumber,
      username,
      password,
      phone,
      email,
      address,
      role,
      gender,
      submitForm
    }
  }
}
</script>

<style>
label {
  display: block;
  text-align: left;
  margin-bottom: 0.5rem;
}
@import "../assets/css/HeaderFooter.css";
</style>
