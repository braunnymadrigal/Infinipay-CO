<template>
  <HeaderCompany rol="profile.Rol" />

  <div
    v-if="showPopup"
    @click.stop
    class="d-flex justify-content-center my-5 py-5"
  >
    <div class="display-1 text-danger" style="padding: 150px">
      No tienes permisos para registrar empleados.
    </div>
  </div>

  <div v-else>
    <div
      class="card p-4 mx-auto"
      style="max-width: 1000px; background-color: #fff8f3; border: none"
    >
      <h1 class="text-center" style="color: #405d72">¡Bienvenido!</h1>
      <h2 class="text-center" style="color: #758694">
        Editar datos de perfil para tu empleado
      </h2>

      <form @submit.prevent="submitForm">
        <div class="row mb-3 justify-content-center" style="margin-top: 30px">
          <div class="col-md-6 col-lg-6">
            <label for="firstName" class="form-label">Primer nombre</label>
            <input
              type="text"
              class="form-control"
              style="background-color: #fff8f3"
              v-model="employee.firstName"
              id="firstName"
              required
              maxlength="50"
              pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
              placeholder="Sólo se permiten letras y acentos del abecedario español"
              title="Sólo se permiten letras y acentos del abecedario español"
            />
          </div>

          <div class="col-md-6 col-lg-6">
            <label for="secondName" class="form-label">Segundo nombre</label>
            <input
              type="text"
              class="form-control"
              style="background-color: #fff8f3"
              v-model="employee.secondName"
              id="secondName"
              maxlength="50"
              pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
              placeholder="Sólo se permiten letras y acentos del abecedario español"
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
              v-model="employee.firstLastName"
              id="firstLastName"
              required
              maxlength="50"
              pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
              placeholder="Sólo se permiten letras y acentos del abecedario español"
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
              v-model="employee.secondLastName"
              id="secondLastName"
              required
              maxlength="50"
              pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
              placeholder="Sólo se permiten letras y acentos del abecedario español"
              title="Sólo se permiten letras y acentos del abecedario español"
            />
          </div>
        </div>

        <div class="mb-3">
          <label for="gender" class="form-label"> Género</label>
          <select
            id="gender"
            class="form-select"
            style="background-color: #fff8f3"
            v-model="employee.gender"
            required
          >
            <option disabled value="">Seleccione una opción</option>
            <option value="masculino">Masculino</option>
            <option value="femenino">Femenino</option>
          </select>
        </div>

        <div class="mb-3">
          <label for="idNumber" class="form-label">Cédula</label>
          <input
            type="text"
            class="form-control"
            style="background-color: #fff8f3"
            v-model="employee.idNumber"
            id="idNumber"
            required
            pattern="^(?!000000000)[1-79]\d{8}$"
            placeholder="9 dígitos, sin guiones"
            title="Debe tener 9 dígitos, empezar con 1-7 o 9 y no ser todo ceros"
          />
        </div>

        <div class="mb-3">
          <label class="form-label">Fecha de nacimiento</label>
          <div class="d-flex gap-2">
            <select
              id="birthDay"
              class="form-select"
              v-model="employee.birthDay"
              style="background-color: #fff8f3"
              required
            >
              <option value="">Día</option>
              <option v-for="day in 31" :key="day" :value="day">
                {{ day }}
              </option>
            </select>
            <select
              id="birthMonth"
              class="form-select"
              v-model="employee.birthMonth"
              style="background-color: #fff8f3"
              required
            >
              <option value="">Mes</option>
              <option
                v-for="(month, index) in months"
                :key="index"
                :value="index + 1"
              >
                {{ month }}
              </option>
            </select>
            <select
              id="birthYear"
              class="form-select"
              v-model="employee.birthYear"
              style="background-color: #fff8f3"
              required
            >
              <option value="">Año</option>
              <option v-for="year in years" :key="year" :value="year">
                {{ year }}
              </option>
            </select>
          </div>
        </div>

        <div class="mb-3">
          <label class="form-label">Fecha de contratación</label>
          <div class="d-flex gap-2">
            <select
              id="hireDay"
              class="form-select"
              v-model="employee.hireDay"
              style="background-color: #fff8f3"
              required
            >
              <option value="">Día</option>
              <option v-for="day in 31" :key="day" :value="day">
                {{ day }}
              </option>
            </select>
            <select
              id="hireMonth"
              class="form-select"
              v-model="employee.hireMonth"
              style="background-color: #fff8f3"
              required
            >
              <option value="">Mes</option>
              <option
                v-for="(month, index) in months"
                :key="index"
                :value="index + 1"
              >
                {{ month }}
              </option>
            </select>
            <select
              id="hireYear"
              class="form-select"
              v-model="employee.hireYear"
              style="background-color: #fff8f3"
              required
            >
              <option value="">Año</option>
              <option v-for="year in years" :key="year" :value="year">
                {{ year }}
              </option>
            </select>
          </div>
        </div>

        <div class="mb-3">
          <label class="form-label" for="role">Rol</label>
          <select
            id="role"
            class="form-select"
            v-model="employee.role"
            style="background-color: #fff8f3"
            required
          >
            <option disabled value="">Seleccione el rol del empleado</option>
            <option value="sinRol">Sin rol asignado</option>
            <option value="supervisor">Supervisor</option>
            <option value="administrador">Administrador</option>
          </select>
        </div>

        <div class="mb-3">
          <label for="username" class="form-label">Nombre de usuario</label>
          <input
            type="text"
            class="form-control"
            v-model="employee.username"
            style="background-color: #fff8f3"
            id="username"
            required
            maxlength="100"
            pattern="^[a-z_\.]+$"
            title="Sólo se permiten letras minúsculas, '_' y '.'"
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
              v-model="employee.phoneNumber"
              id="phoneNumber"
              required
              pattern="\d{8}"
              title="Formato: 8 dígitos, sin guiones"
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
            v-model="employee.email"
            id="email"
            required
            maxlength="100"
            placeholder="xxx@xxxx.xxx"
            @input="employee.email = $event.target.value.toLowerCase()"
            title="Formato: xxx@xxxx.xxx"
          />
        </div>

        <h5 class="fw-normal mb-3 text-start">Información de contrato</h5>
        <div class="border p-3 rounded mb-3">
          <div class="row g-3">
            <div class="col-md-6">
              <label for="reportsHours" class="form-label">
                ¿Reporta horas?</label
              >
              <select
                id="reportsHours"
                class="form-select"
                v-model="employee.reportsHours"
                required
                style="background-color: #fff8f3"
              >
                <option disabled value="">Seleccione una opción</option>
                <option value="1">Sí</option>
                <option value="0">No</option>
              </select>
            </div>
            <div class="col-md-6">
              <label for="salary" class="form-label">Salario bruto</label>
              <input
                type="text"
                id="salary"
                class="form-control"
                style="background-color: #fff8f3"
                v-model="employee.salary"
                pattern="^\d{1,8}(\.\d{1,2})?$"
                required
                placeholder="Ej: 45000.00"
                title="Formato: Máximo 8 dígitos antes del punto y 2 después"
              />
            </div>
            <div class="col-md-6">
              <label for="typeContract" class="form-label">
                Tipo de contrato</label
              >
              <select
                id="typeContract"
                class="form-select"
                v-model="employee.typeContract"
                required
                style="background-color: #fff8f3"
              >
                <option disabled value="">Seleccione una opción</option>
                <option value="semanal">Semanal</option>
                <option value="quincenal">Quincenal</option>
                <option value="mensual">Mensual</option>
                <option value="servicios">Servicios</option>
              </select>
            </div>
            <div class="col-md-6">
              <label class="form-label"> Fecha de creación</label>
              <div class="d-flex gap-2">
                <select
                  id="creationDay"
                  class="form-select"
                  v-model="employee.creationDay"
                  style="background-color: #fff8f3"
                  required
                >
                  <option value="">Día</option>
                  <option v-for="day in 31" :key="day" :value="day">
                    {{ day }}
                  </option>
                </select>
                <select
                  id="creationMonth"
                  class="form-select"
                  v-model="employee.creationMonth"
                  style="background-color: #fff8f3"
                  required
                >
                  <option value="">Mes</option>
                  <option
                    v-for="(month, index) in months"
                    :key="index"
                    :value="index + 1"
                  >
                    {{ month }}
                  </option>
                </select>
                <select
                  id="creationYear"
                  class="form-select"
                  v-model="employee.creationYear"
                  style="background-color: #fff8f3"
                  required
                >
                  <option value="">Año</option>
                  <option v-for="year in years" :key="year" :value="year">
                    {{ year }}
                  </option>
                </select>
              </div>
            </div>
          </div>
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
                v-model="employee.province"
                id="province"
                required
                maxlength="50"
                pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
                title="Sólo se permiten letras y acentos del abecedario español"
                placeholder="Sólo se permiten letras y acentos del abecedario español"
              />
            </div>
            <div class="col-md-6">
              <label for="canton" class="form-label">Cantón</label>
              <input
                type="text"
                class="form-control"
                style="background-color: #fff8f3"
                v-model="employee.canton"
                id="canton"
                required
                maxlength="50"
                title="Sólo se permiten letras y acentos del abecedario español"
                placeholder="Sólo se permiten letras y acentos del abecedario español"
                pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
              />
            </div>
            <div class="col-md-6">
              <label for="district" class="form-label">Distrito</label>
              <input
                type="text"
                class="form-control"
                style="background-color: #fff8f3"
                v-model="employee.district"
                id="district"
                required
                maxlength="50"
                pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
                title="Sólo se permiten letras y acentos del abecedario español"
                placeholder="Sólo se permiten letras y acentos del abecedario español"
              />
            </div>
            <div class="col-md-6">
              <label for="otherSigns" class="form-label">Otras señas</label>
              <textarea
                class="form-control"
                style="background-color: #fff8f3; height: 38px"
                v-model="employee.otherSigns"
                id="otherSigns"
                maxlength="256"
                pattern="^[a-zA-Z0-9áéíóúÁÉÍÓÚ\s]+$"
                rows="2"
                title="Sólo se permiten letras, números y espacios en blanco"
                placeholder="Sólo se permiten letras, números y espacios en blanco"
              >
              </textarea>
            </div>
          </div>
        </div>

        <div class="d-flex justify-content-center">
          <button
            type="submit"
            class="btn btn-secondary"
            style="background-color: #405d72; color: white; border: transparent"
          >
            Confirmar cambios
          </button>
        </div>
      </form>
    </div>
  </div>
  <MainFooter />
</template>

<script>
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";
export default {
  components: {
    HeaderCompany,
    MainFooter,
  },
  data() {
    return {
      showPopup: false,
      employee: {
        firstName: "",
        secondName: "",
        firstLastName: "",
        secondLastName: "",
        idNumber: "",
        username: "",
        phoneNumber: "",
        email: "",
        role: "",
        gender: "",
        birthDay: "",
        birthMonth: "",
        birthYear: "",
        hireDay: "",
        hireMonth: "",
        hireYear: "",
        reportsHours: "",
        salary: "",
        typeContract: "",
        creationDay: "",
        creationMonth: "",
        creationYear: "",
        province: "",
        canton: "",
        district: "",
        otherSigns: "",
      },

      months: [
        "Enero",
        "Febrero",
        "Marzo",
        "Abril",
        "Mayo",
        "Junio",
        "Julio",
        "Agosto",
        "Septiembre",
        "Octubre",
        "Noviembre",
        "Diciembre",
      ],
      years: this.generateYears(),
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

    fetchEmployeeData() {
      this.$api
        .getEmployeeById(this.$route.params.id)
        .then((response) => {
          const data = response.data;
          this.employee.firstName = data.firstName;
          this.employee.secondName = data.secondName;
          this.employee.firstLastName = data.firstLastName;
          this.employee.secondLastName = data.secondLastName;
          this.employee.idNumber = data.idNumber;
          this.employee.username = data.username;
          this.employee.phoneNumber = data.phoneNumber;
          this.employee.email = data.email;
          this.employee.role = data.role;
          this.employee.gender = data.gender;
          this.employee.birthDay = data.birthDay;
          this.employee.birthMonth = data.birthMonth;
          this.employee.birthYear = data.birthYear;
          this.employee.hireDay = data.hireDay;
          this.employee.hireMonth = data.hireMonth;
          this.employee.hireYear = data.hireYear;
          this.employee.reportsHours = data.reportsHours;
          this.employee.salary = data.salary;
          this.employee.typeContract = data.typeContract;
          this.employee.creationDay = data.creationDay;
          this.employee.creationMonth = data.creationMonth;
          this.employee.creationYear = data.creationYear;
          this.employee.province = data.province;
          this.employee.canton = data.canton;
          this.employee.district = data.district;
          this.employee.otherSigns = data.otherSigns;
        })
        .catch((error) => {
          console.error("Error al obtener los datos del empleado:", error);
          this.showPopup = true;
          if (error.response) {
            const message = error.response.data?.message || "Error desconocido";
            alert(message);
            this.$router.push("MyProfile");
          }
        });
    },

    submitForm() {
      const employeeData = {
        firstName: this.employee.firstName,
        secondName: this.employee.secondName,
        firstLastName: this.employee.firstLastName,
        secondLastName: this.employee.secondLastName,
        idNumber: this.employee.idNumber,
        username: this.employee.username,
        phoneNumber: this.employee.phoneNumber,
        email: this.employee.email,
        role: this.employee.role,
        province: this.employee.province,
        canton: this.employee.canton,
        district: this.employee.district,
        otherSigns: this.employee.otherSigns,
        gender: this.employee.gender,
        birthDay: Number(this.employee.birthDay),
        birthMonth: Number(this.employee.birthMonth),
        birthYear: Number(this.employee.birthYear),
        hireDay: Number(this.employee.hireDay),
        hireMonth: Number(this.employee.hireMonth),
        hireYear: Number(this.employee.hireYear),
        reportsHours: Number(this.employee.reportsHours),
        salary: Number(this.employee.salary),
        typeContract: this.employee.typeContract,
        creationDay: Number(this.employee.creationDay),
        creationMonth: Number(this.employee.creationMonth),
        creationYear: Number(this.employee.creationYear),
      };

      this.$api
        .updateEmployee(employeeData, this.$route.params.id)
        .then(() => {
          this.showPopup = false;
          alert("¡Empleado actualizado exitosamente!");
          this.$router.push("../EmployeesList");
        })
        .catch((error) => {
          this.showPopup = true;
          console.error("Error:", error);
          if (error.response) {
            const message = error.response.data?.message || "Error desconocido";
            alert(message);
            this.$router.push("../EmployeesList");
          }
        });
    },
  },
  mounted() {
    this.fetchEmployeeData();
  },
};
</script>

<style>
label {
  display: block;
  text-align: left;
  margin-bottom: 0.5rem;
}
@import "../assets/css/HeaderFooter.css";
</style>
