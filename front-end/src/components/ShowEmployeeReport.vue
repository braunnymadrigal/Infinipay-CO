<template>
  <div>
    <HeaderCompany />
    <div class="container mt-4">
      <h1 class="text-center mb-4" style="color: #405D72;">
        Reportes de pagos de planilla
      </h1>

      <div
        v-if="alertMessage"
        :class="['alert', alertType === 'success' ? 'alert-success'
        : 'alert-danger']"
        role="alert"
      >
        {{ alertMessage }}
      </div>

      <div v-if="loading">
        <p class="text-center">Cargando reportes...</p>
      </div>

      <div v-else-if="report" class="mx-auto" style="max-width: 750px;">
        <div class="mb-3">
          <label for="period" class="text-center form-label">
            Seleccione un periodo:
          </label>
          <select class="form-select" v-model="selectedPeriodKey"
            @change="updateSelectedPayment">
            <option
              v-for="payment in report.payments"
              :key="paymentKey(payment)"
              :value="paymentKey(payment)"
            >
              {{ formatDate(payment.startDate) }} -
              {{ formatDate(payment.endDate) }}
            </option>
          </select>
        </div>

        <div class="shadow p-4 border rounded" style="background-color: #fffdfc;
          margin-bottom: 30px;">
          <p><span><strong>Empresa: </strong></span><span>
            {{ report.companyName }}</span></p>
          <p><span><strong>Nombre completo: </strong></span><span>
            {{ report.fullName }}</span></p>
          <p><span><strong>Tipo de contrato: </strong></span>
            <span>{{ formatContractType(report.contractType) }}</span>
          </p>
          <p><span><strong>Fecha de pago: </strong></span>
            <span>{{ formatDate(selectedPayment.endDate) }}</span>
          </p>
          
          <div v-if="selectedPayment" class="mt-4">
            <p class="label-value-line fw-bold">
              <span><strong>Salario Bruto</strong></span>
              <span>₡{{ formatAmount(selectedPayment.grossSalary) }}</span>
            </p>

            <div v-if="selectedPayment.deductions &&
              selectedPayment.deductions.length > 0">
              <h5 class="mt-3">Deducciones obligatorias</h5>
              <div v-if="hasDeducciones('obligatoria')">
                <p
                  v-for="(d, index) in
                  selectedPayment.deductions.filter(d => isObligatoria(d.type))"
                  :key="index"
                  class="label-value-line"
                >
                  <span>{{ d.deductionName }}</span>
                  <span>-₡{{ formatAmount(d.amount) }}</span>
                </p>
                <p class="label-value-line fw-bold">
                  <span>Total deducciones obligatorias</span>
                  <span>-₡{{ totalDeducciones('obligatoria') }}</span>
                </p>
              </div>
              <p v-else>Sin deducciones obligatorias</p>

              <h5 class="mt-3">Deducciones voluntarias</h5>
              <div v-if="hasDeducciones('voluntaria')">
                <p
                  v-for="(d, index) in
                  selectedPayment.deductions.filter(d => !isObligatoria(d.type))"
                  :key="index"
                  class="label-value-line"
                >
                  <span>{{ d.deductionName }}</span>
                  <span>-₡{{ formatAmount(d.amount) }}</span>
                </p>
                <p class="label-value-line fw-bold">
                  <span>Total deducciones voluntarias</span>
                  <span>-₡{{ totalDeducciones('voluntaria') }}</span>
                </p>
              </div>
              <p v-else>Sin deducciones voluntarias</p>
            </div>

            <p class="label-value-line mt-3 fw-bold">
              <span>Pago Neto:</span>
              <span>₡{{ formatAmount(selectedPayment.netSalary) }}</span>
            </p>
          </div>
        </div>
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
      report: null,
      loading: true,
      alertMessage: "",
      alertType: "",
      selectedPeriodKey: "",
      selectedPayment: null,
    };
  },
  mounted() {
    this.fetchPayroll();
  },
  methods: {
    paymentKey(payment) {
      return `${new Date(payment.startDate).getTime()}-${new Date(payment.endDate).getTime()}`;
    },
    formatDate(dateString) {
      const date = new Date(dateString);
      return date.toLocaleDateString("es-CR", { year: 'numeric', month: 'long', day: 'numeric' });
    },
    formatAmount(value) {
      if (typeof value !== 'number') return value;
      return value.toLocaleString("es-CR", {
        style: "decimal",
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      });
    },
    isObligatoria(type) {
      if (!type) return false;
      const obligatorias = ['renta', 'ccssempleado'];
      return obligatorias.includes(type.toLowerCase());
    },
    totalDeducciones(tipo) {
      if (!this.selectedPayment || !this.selectedPayment.deductions) return 0;

      const filtered = this.selectedPayment.deductions.filter(d =>
        tipo === 'obligatoria' ? this.isObligatoria(d.type)
        : !this.isObligatoria(d.type)
      );

      const total = filtered.reduce((sum, d) => sum + d.amount, 0);
      return total.toLocaleString("es-CR", {
        style: "decimal",
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      });
    },
    hasDeducciones(tipo) {
      return this.selectedPayment?.deductions?.some(d =>
        tipo === 'obligatoria' ? this.isObligatoria(d.type)
        : !this.isObligatoria(d.type)
      );
    },
    updateSelectedPayment() {
      if (!this.report || !this.report.payments) return;

      this.selectedPayment = this.report.payments.find(
        (p) => this.paymentKey(p) === this.selectedPeriodKey
      );
    },
    formatContractType(type) {
      const map = {
        medioTiempo: 'Medio Tiempo',
        tiempoCompleto: 'Tiempo Completo',
        servicios: 'Servicios Profesionales',
        horas: 'Horas'
      };
      return map[type] || type;
    },
    async fetchPayroll() {
      this.loading = true;
      this.alertMessage = "";
      this.alertType = "";
      try {
        const response = await this.$api.showEmployeeReport();
        this.report = response.data;

        if (!this.report || !this.report.payments
          || this.report.payments.length === 0) {
          this.alertMessage = "No hay reportes disponibles.";
          this.alertType = "warning";
          this.report = null;
        } else {
          this.selectedPeriodKey = this.paymentKey(this.report.payments[0]);
          this.updateSelectedPayment();
        }
      } catch (err) {
        this.alertType = "danger";
        this.alertMessage = err.response?.status === 403
          ? "No tiene permisos para acceder a los reportes del empleado de pagos de planilla."
          : `Error del servidor: ${err.response?.data?.message || err.message}`;
      } finally {
        this.loading = false;
      }
    },
  },
};
</script>

<style scoped>
.form-select {
  max-width: 400px;
  margin: 0 auto;
  margin-bottom: 20px;
}

.label-value-line {
  display: flex;
  justify-content: space-between;
  white-space: nowrap;
  margin-bottom: 5px;
}

.label-value-line span:first-child {
  flex: 1;
}

.label-value-line span:last-child {
  text-align: right;
  min-width: 180px;
  display: inline-block;
}
</style>
