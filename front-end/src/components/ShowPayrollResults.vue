<template>
  <div>
    <HeaderCompany />
    <div class="container mt-4 text-center">
    <h1 style="color: #405D72; margin-bottom: 40px;">Planillas de empleados</h1>
      <table class="table table-bordered table-hover">
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
          <tr v-for="(payroll, index) in payroll" :key="index">
            <td>{{ payroll.EmployeeName }}</td>
            <td>{{ payroll.startDate + ' - ' + payroll.endDate }}</td>
            <td>₡{{ payroll.ComputedGrossSalary }}</td>
            <td>
              <div class="small">Impuesto de Renta: ₡{{ payroll.RentTax }}</div>
              <div class="small">CCSS: ₡{{ payroll.ccss || 0 }}</div>
              <div class="small">Beneficios: ₡{{ payroll.benefits || 0 }}</div>
              <div class="fw-bold mt-1">Total: ₡{{ payroll.deductionsSum }}</div>
            </td>
            <td><strong>₡{{ payroll.finalSalary }}</strong></td>
          </tr>
        </tbody>
      </table>
    </div>
    <MainFooter />
  </div>
</template>

<script>
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";
export default {
  props: {
    payroll: Object,
    months: Array
  },
  components: {
    HeaderCompany,
    MainFooter,
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