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
            <router-link to="/EmployerProfile" class="mx-2"
              style="color: #405D72;">Perfil</router-link>
            <a href="#" class="mx-2" style="color: #405D72;">Empresa</a>
            <a href="#" class="mx-2" style="color: #405D72;">Beneficios</a>
            <router-link to="/EmployeesList" class="mx-2"
              style="color: #405D72;">Empleados</router-link>
          </div>
          <div class="ms-auto">
            <router-link to="/RegisterEmployee" class="btn btn-primary me-2"
              style="background-color: #F7E7DC; color: #2b3f4e;
                border: 2px solid transparent;">Registrar empleado
            </router-link>
          </div>
        </div>
      </nav>
    </header>

    <div v-if="!canSeeEmployeesInfo" class="text-center">
      <h4>No tiene permisos para ver esta información.</h4>
    </div>

    <div v-else>
      <h1 class="text-center mb-4" style="color: #405D72">
        Nombre de Empresa</h1>
      <h2 class="text-center mb-4" style="color: #758694">
        Lista de Empleados</h2>
      <table class="table table-striped">
        <thead>
          <tr>
            <th>Nombre</th>
            <th>Cédula</th>
            <th>Correo</th>
            <th>Teléfono</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="employee in employees" :key="employee.id">
            <td>{{ employee.completeName }}</td>
            <td>{{ employee.id }}</td>
            <td>{{ employee.email }}</td>
            <td>{{ employee.phoneNumber }}</td>
            <td>
              <button class="btn btn-link"
                @click="viewEmployeeDetails(employee)">
                Ver detalles</button>
            </td>
          </tr>
        </tbody>
      </table>

      <div v-if="showEmployeeDetailsModal" class="modal fade show d-block"
        tabindex="-1"
        style="background: rgba(0,0,0,0.5);">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title">Ficha del Empleado</h5>
              <button type="button" class="btn-close"
                @click="closeEmployeeDetailsModal"></button>
            </div>
            <div class="modal-body">
              <p><strong>Nombre completo:</strong> {{
                selectedEmployee.completeName || 'Nombre no disponible' }}</p>
              <p><strong>Cédula:</strong> {{ selectedEmployee.id ||
                'Cédula no disponible' }}</p>
              <p><strong>Correo electrónico:</strong> {{ selectedEmployee.email
                || 'Correo no disponible' }}</p>
              <p><strong>Teléfono:</strong> {{ selectedEmployee.phoneNumber ||
                'Teléfono no disponible' }}</p>
              <p><strong>Rol:</strong> {{ selectedEmployee.role ||
                'Rol no disponible' }}</p>
              <p><strong>Fecha de ingreso:</strong> {{
                selectedEmployee.hireDate || 'Fecha no disponible' }}</p>
              <p><strong>Supervisor asignado:</strong> {{
                selectedEmployee.supervisor || 'No asignado' }}</p>
              <p><strong>Observaciones:</strong> {{
                selectedEmployee.observations ? selectedEmployee.observations
                : 'Sin observaciones' }}</p>

              <p><strong>Dirección:</strong></p>
              <div class="border p-3 rounded mb-3">
                <div class="row g-3">
                  <div class="col-md-6">
                    <label for="province" class="form-label">Provincia</label>
                    <input type="text" class="form-control"
                      v-model="selectedEmployee.address.province"
                      id="province" required maxlength="10"
                      pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$" :readonly="true">
                  </div>
                  <div class="col-md-6">
                    <label for="canton" class="form-label">Cantón</label>
                    <input type="text" class="form-control"
                      v-model="selectedEmployee.address.canton"
                      id="canton" required maxlength="100"
                      pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$" :readonly="true">
                  </div>
                  <div class="col-md-6">
                    <label for="district" class="form-label">Distrito</label>
                    <input type="text" class="form-control"
                      v-model="selectedEmployee.address.district"
                      id="district" required maxlength="100"
                      pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$" :readonly="true">
                  </div>
                  <div class="col-md-6">
                    <label for="otherSigns" class="form-label">
                      Otras señas</label>
                    <textarea class="form-control" style="height: 38px;"
                      v-model="selectedEmployee.address.otherSigns"
                      id="otherSigns" required maxlength="300"
                      pattern="^[a-zA-Z0-9áéíóúÁÉÍÓÚ\s]+$" rows="2"
                      placeholder="Sólo se permiten letras, números y espacios en blanco"
                      :readonly="true">
                    </textarea>
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
    </div>
  </div>
  
  <footer class="py-5 custom-footer">
    <div class="container">
      <div class="row">
        <div class="col-md-3 mb-3">
          <p class="h5">Infinipay CO.</p>
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
export default {
  data() {
    return {
      actualUser: {
        role: 'Empleador'
      },
      employees: [
        {
          identifier: 1,
          completeName: "Juan David Perez Hidalgo",
          id: "111111111",
          email: "juanperez@mail.com",
          phoneNumber: "88888888",
          role: "Empleado",
          hireDate: "2025-01-15",
          supervisor: "Rosalina Guimaraes Azofeifa",
          observations: "",
          address: {
            province: "San José",
            canton: "Montes de Oca",
            district: "San Pedro",
            otherSigns: "UCR, facultad de Computación"
          }
        },
        {
          identifier: 2,
          completeName: "Rosalina Guimaraes Azofeifa",
          id: "211111111",
          email: "rosalina.guimaraes@mail.com",
          phoneNumber: "89998999",
          role: "Supervisor",
          hireDate: "2024-05-23",
          supervisor: "",
          observations: "",
          address: {
            province: "Heredia",
            canton: "Barva",
            district: "San José de la Montaña",
            otherSigns: "Cerca de la iglesia"
          }
        }
      ],
      showEmployeeDetailsModal: false,
      selectedEmployee: {}
    }
  },
  computed: {
    canSeeEmployeesInfo() {
      return ["Administrador", "Supervisor"
        , "Empleador"].includes(this.actualUser.role);
    }
  },
  methods: {
    viewEmployeeDetails(employee) {
      this.selectedEmployee = employee;
      this.showEmployeeDetailsModal = true;
    },
    closeEmployeeDetailsModal() {
      this.showEmployeeDetailsModal = false;
    }
  }
}
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
