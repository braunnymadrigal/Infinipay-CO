<template>
  <HeaderCompany />

  <div v-if="showPopup" class="d-flex justify-content-center my-5 py-5">
    <div class="display-6 text-danger">
      No tiene permisos para ver esta información.
    </div>
  </div>

  <div v-if="isLoading" class="text-center mt-5">
    <p>Cargando planilla...</p>
  </div>

  <div v-if="!isLoading && !showPopup" class="container mt-5 mb-5">
    <h2 class="text-center mb-4" style="color: #405d72">Planilla</h2>
    <h4 class="text-center mb-4" style="color: #758694">Juan Pérez Solano</h4>

    <table class="table table-bordered table-hover align-middle">
      <thead class="table-light">
        <tr>
          <th style="color: #405d72">Periodo</th>
          <th style="color: #405d72">Salario Bruto</th>
          <th style="color: #405d72">Deducciones</th>
          <th style="color: #405d72">Salario Neto</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>{{ payroll.period }}</td>
          <td>₡{{ payroll.grossSalary.toLocaleString() }}</td>
          <td>
            <div class="small">Impuesto de Renta: ₡{{
              payroll.incomeTax.toLocaleString() }}</div>

            <div class="small">CCSS: ₡{{
              payroll.ccss.toLocaleString() }}</div>

            <div class="small">Beneficios: ₡{{
              payroll.benefits.toLocaleString() }}</div>

            <div class="fw-bold mt-1">Total: ₡{{
              payroll.totalDeductions.toLocaleString() }}</div>
          </td>
          <td><strong>₡{{ payroll.netSalary.toLocaleString() }}</strong></td>
        </tr>
      </tbody>
    </table>
  </div>

  <MainFooter />
</template>

<script>
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";

export default {
  components: { HeaderCompany, MainFooter },
  data() {
    return {
      showPopup: false,
      isLoading: false,
      payroll: {
        period: "13-30 Abril, 2025",
        grossSalary: 650000,
        incomeTax: 25000,
        ccss: 31000,
        benefits: 40000,
        get totalDeductions() {
          return this.incomeTax + this.ccss + this.benefits;
        },
        get netSalary() {
          return this.grossSalary - this.totalDeductions;
        }
      }
    };
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
