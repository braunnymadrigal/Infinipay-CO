<template>
  <div>
    <HeaderCompany />
    <div class="container mt-4 text-center">
      <h1 style="color: #405D72; margin-bottom: 40px;">
        Planillas de empleados</h1>

      <div v-if="alertMessage" :class="['alert', alertType === 'success' ?
        'alert-success' : 'alert-danger']" role="alert"
        style="margin-bottom: 20px;">
        {{ alertMessage }}
      </div>

      <table class="table table-bordered table-hover"
      v-if="!loading && payroll.length">
        <thead class="table-light">
          <tr>
            <th>Empleado</th>
            <th>Periodo</th>
            <th>Salario Bruto</th>
            <th>Deducciones</th>
            <th>Salario Neto</th>
          </tr>
        </thead>
        <tbody>
          <template v-for="planilla in paginatedPayroll" :key="planilla.id">
            <tr v-for="(employee, i) in planilla.payrollEmployees" :key="i">
              <td>{{ fixNameSpacing(employee.employeeName) }}</td>
              <td>{{ formatDate(planilla.payrollStartDate) }} -
                {{ formatDate(planilla.payrollEndDate) }}</td>
              <td>₡{{ employee.employeeGrossSalary }}</td>
              <td>
                <div class="small" v-for="(ded, j) in
                  employee.employeeDeductions" :key="j">
                  <strong>{{ formatDeductionType(ded.deductionType) }}:</strong>
                  ₡{{ formatAmount(ded.deductionAmount) }}
                </div>
                <div class="fw-bold mt-1">
                  Total: ₡{{ calculateDeductions(employee.employeeDeductions) }}
                </div>
              </td>
              <td><strong>₡{{ employee.employeeNetSalary }}</strong></td>
            </tr>
          </template>
        </tbody>
      </table>

      <div class="d-flex justify-content-center align-items-center mt-3 mb-4"
      v-if="totalPages > 1" style="gap: 15px;">
        <button
          class="btn btn-primary"
          style="background-color: #758694; border: transparent;"
          :disabled="currentPage === 1"
          @click="currentPage--"
        >
          Anterior
        </button>

        <span>Página {{ currentPage }} de {{ totalPages }}</span>

        <button
          class="btn btn-primary"
          style="background-color: #758694; border: transparent;"
          :disabled="currentPage === totalPages"
          @click="currentPage++"
        >
          Siguiente
        </button>
      </div>

      <div v-else-if="loading">
        <p>Cargando planillas...</p>
      </div>
    </div>

    <MainFooter />
  </div>
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
      payroll: [],
      error: null,
      currentPage: 1,
      itemsPerPage: 2,
      loading: true,
      alertMessage: "",
      alertType: "",
    };
  },
  computed: {
    paginatedPayroll() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.payroll.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.payroll.length / this.itemsPerPage);
    }
  },
  mounted() {
    this.fetchPayroll();
  },
  methods: {
    async fetchPayroll() {
      this.loading = true;
      this.alertMessage = "";
      this.alertType = "";
      try {
        const response = await this.$api.showEmployeesPayroll();
        this.payroll = response.data;
        if (!this.payroll || this.payroll.length === 0) {
          this.alertMessage = "No hay datos de planillas disponibles.";
          this.alertType = "warning";
        }
      } catch (err) {
        console.log('Error:', err);
        console.log('Response:', err.response);
        console.log('Status:', err.response?.status);
        console.log('Data:', err.response?.data);

        if (!err.response) {
          this.alertMessage = "No tiene permisos para acceder a las planillas.";
        } else {
          const statusCode = err.response.status;
          const errorMessage = err.response.data?.message || err.message;

          if (statusCode === 403) {
            this.alertMessage =
              "No tiene permisos para acceder a las planillas.";
          } else if (statusCode === 500) {
            this.alertMessage = "Error del servidor: " + errorMessage;
          } else {
            this.alertMessage = errorMessage || "Ocurrió un error inesperado.";
          }
        }
        this.alertType = "danger";
      } finally {
        this.loading = false;
      }
    },
    formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleDateString('es-CR');
    },
    calculateDeductions(employeeDeductions) {
      if (!employeeDeductions) return '0.00';
      const total = employeeDeductions.reduce((sum, d) => sum +
        d.deductionAmount, 0);
      return total.toFixed(2);
    },
    formatDeductionType(deductionType) {
      if (!deductionType) return '';
      const t = deductionType.toLowerCase();
      if (t.includes('ccss')) return 'CCSS';
      if (t.includes('renta')) return 'Renta';
      if (t.includes('beneficio')) return 'Beneficios';
      return deductionType.charAt(0).toUpperCase() + deductionType.slice(1);
    },
    formatAmount(deductionAmount) {
      if (typeof deductionAmount !== 'number') return deductionAmount;
      return deductionAmount.toLocaleString('es-CR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      });
    },
    fixNameSpacing(name) {
      if (!name) return "";
      return name.replace(/([a-záéíóúñ])([A-ZÁÉÍÓÚÑ])/g, '$1 $2');
    }
  }
};
</script>

<style scoped>
.small {
  font-size: 0.85rem;
  color: #555;
}
.fw-bold {
  font-weight: 600;
}
</style>
