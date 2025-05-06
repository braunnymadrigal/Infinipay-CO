<template>
  <HeaderCompany/>

  <div v-if="showPopup" @click.stop 
  class="d-flex justify-content-center my-5 py-5">
    <div class="display-1 text-danger" style="padding: 150px;">
      No tiene permisos para ver esta información.
    </div>
  </div> 
  
  <div v-if="isLoading" class="text-center mt-5">
    <p>Cargando información...</p>
  </div>

  <div v-if="!isLoading" class="container mt-5 mb-5">
    <h1 class="text-center mb-4" style="color: #405D72">
      Nombre de Empresa
    </h1>
    <h2 class="text-center mb-4" style="color: #758694">
      Lista de Empleados
    </h2>
    <table class=
    "table is-bordered table-striped is-narrow is-hoverable is-fullwidth">
      <thead>
        <tr>
          <th style="white-space: nowrap">Nombre</th>
          <th style="white-space: nowrap">Cédula</th>
          <th style="white-space: nowrap">Correo</th>
          <th style="white-space: nowrap">Teléfono</th>
          <th style="white-space: nowrap">Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="employee in employees" :key="employee.id">
          <td>{{ employee.completeName }}</td>
          <td>{{ employee.identification }}</td>
          <td>{{ employee.email }}</td>
          <td>{{ employee.phoneNumber }}</td>
          <td>
            <button class="btn btn-link" @click="viewEmployeeDetails(employee)">
              Ver detalles
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>

  <div v-if="showEmployeeDetailsModal" class="modal fade show d-block"
    tabindex="-1" style="background: rgba(0,0,0,0.5);">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Ficha del Empleado</h5>
          <button type="button" class="btn-close"
          @click="closeEmployeeDetailsModal"></button>
        </div>
        <div class="modal-body">
          <p><strong>Nombre completo:</strong>
            {{ selectedEmployee.completeName || 'Nombre no disponible' }}</p>
          <p><strong>Cédula:</strong>
            {{ selectedEmployee.identification || 'Cédula no disponible' }}</p>
          <p><strong>Correo electrónico:</strong>
            {{ selectedEmployee.email || 'Correo no disponible' }}</p>
          <p><strong>Teléfono:</strong>
            {{ selectedEmployee.phoneNumber || 'Teléfono no disponible' }}</p>
          <p><strong>Rol:</strong>
            {{ selectedEmployee.role || 'Rol no disponible' }}</p>
          <p><strong>Fecha de ingreso:</strong>
            {{ selectedEmployee.hireDate || 'Fecha no disponible' }}</p>
          <p><strong>Supervisor asignado:</strong>
            {{ selectedEmployee.supervisor || 'No asignado' }}</p>
          <p><strong>Observaciones:</strong>
            {{ selectedEmployee.observations ? selectedEmployee.observations
            : 'Sin observaciones' }}</p>

          <p><strong>Dirección:</strong></p>
          <div class="border p-3 rounded mb-3">
            <div class="row g-3">
              <div class="col-md-6">
                <label for="province" class="form-label">Provincia</label>
                <input type="text" class="form-control"
                v-model="selectedEmployee.province" id="province" readonly>
              </div>
              <div class="col-md-6">
                <label for="canton" class="form-label">Cantón</label>
                <input type="text" class="form-control"
                v-model="selectedEmployee.canton" id="canton" readonly>
              </div>
              <div class="col-md-6">
                <label for="district" class="form-label">Distrito</label>
                <input type="text" class="form-control"
                v-model="selectedEmployee.district" id="district" readonly>
              </div>
              <div class="col-md-6">
                <label for="otherSigns" class="form-label">Otras señas</label>
                <textarea class="form-control" style="height: 38px;"
                v-model="selectedEmployee.otherSigns" id="otherSigns"
                rows="2" readonly></textarea>
              </div>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button class="btn btn-secondary"
          @click="closeEmployeeDetailsModal">Cerrar</button>
        </div>
      </div>
    </div>
  </div>

  <MainFooter/>
</template>

<script>
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";
import axios from "axios";

export default {
  components: {
    HeaderCompany,
    MainFooter
  },
  data() {
    return {
      showPopup: false,
      employees: [],
      isLoading: true,
      showEmployeeDetailsModal: false,
      selectedEmployee: {
      }
    };
  },
  methods: {
    fetchEmployees() {
      let jwtCookie = this.$cookies.get('jwt');
      axios.get("https://localhost:7275/api/EmployeeList", 
      { headers: { "Authorization" :  `Bearer ${jwtCookie}` }})
      .then(response => {
        this.showPopup = false;
        this.employees = response.data;
        this.isLoading = false;
      })
      .catch(error => {
        this.showPopup = true;
        console.error("Error al obtener empleados:", error);
        this.isLoading = false;
      });
    },
    viewEmployeeDetails(employee) {
      this.selectedEmployee = employee;
      this.showEmployeeDetailsModal = true;
    },
    closeEmployeeDetailsModal() {
      this.showEmployeeDetailsModal = false;
    }
  },
  mounted() {
    this.fetchEmployees();
  }
};
</script>

<style>
@import '../assets/css/HeaderFooter.css';

.modal.fade.show {
  display: block;
}

.modal-backdrop {
  opacity: 0.5;
}
</style>
