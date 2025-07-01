<template>
  <div>
    <HeaderCompany />

    <div v-if="showPopup" class="d-flex justify-content-center my-5 py-5">
      <div class="display-1 text-danger" style="padding: 150px;">
        No tiene permisos para acceder a los reportes de empleador del pago de planilla.
      </div>
    </div>

      <div v-if="loading">
        <p class="text-center">Cargando reportes...</p>
      </div>

      <div v-else-if="report && selectedPeriod" class="container mt-4"
        style="max-width: 750px;" id="print">
        <div class="mb-3">
          <h1 class="text-center mb-4" style="color: #405D72;">
            Reporte de pagos del empleador
          </h1>
          <label class="form-label text-center">Seleccione un periodo:</label>
          <select class="form-select" v-model="selectedPeriodKey"
            @change="updateSelectedPeriod">
            <option v-for="p in report.periods" 
              :key="periodKey(p)" :value="periodKey(p)">
              {{ formatDate(p.startDate) }} - {{ formatDate(p.endDate) }}
            </option>
          </select>
        </div>

        <div class="shadow p-4 border rounded"
          style="background-color: #fffdfc; margin-bottom: 30px;">
          <p><strong>Empresa:</strong> {{ report.companyName }}</p>
          <p><strong>Nombre del empleador:</strong>
            {{ fixNameSpacing(report.fullName) }}</p>
          <p><strong>Periodo de pago:</strong> {{
            formatDate(selectedPeriod.startDate) }} - {{
            formatDate(selectedPeriod.endDate) }}</p>

          <h5 class="mt-4">Salarios por tipo de contrato</h5>
          <p class="label-value-line"><span>Horas:</span> <span>₡{{
            formatAmount(selectedPeriod.totalEmpHoursSalary) }}</span></p>
          <p class="label-value-line"><span>Tiempo completo:</span> <span>₡{{
            formatAmount(selectedPeriod.totalEmpFullTimeSalary) }}</span></p>
          <p class="label-value-line"><span>Medio tiempo:</span> <span>₡{{
            formatAmount(selectedPeriod.totalEmpHalfTimeSalary) }}</span></p>
          <p class="label-value-line"><span>Servicios profesionales:</span>
            <span>₡{{ formatAmount(selectedPeriod.totalEmpServicesSalary) }}
            </span></p>
          <p class="label-value-line fw-bold"><span>Total salarios:</span>
            <span>₡{{ formatAmount(selectedPeriod.totalSalaries) }}</span></p>
            
          <p v-if="selectedPeriod.totalEmployeeVoluntaryDeductions > 0" class="label-value-line fw-bold">
            <span>Total deducciones voluntarias:</span>
            <span>₡{{ formatAmount(selectedPeriod.totalEmployeeVoluntaryDeductions) }}</span>
          </p>
          <p v-else class="label-value-line">
            <em>Sin deducciones voluntarias</em>
          </p>

          <h5 class="mt-4">Pagos de ley del empleador</h5>
          <p v-for="tax in visibleEmployerTaxes" :key="tax.key"
            class="label-value-line">
            <span>{{ tax.label }}</span>
            <span>₡{{ formatAmount(tax.value) }}</span>
          </p>

          <p class="label-value-line fw-bold mt-2">
            <span>Total pagos de ley:</span>
            <span>₡{{ formatAmount(totalTaxes) }}</span>
          </p>

          <p class="label-value-line fw-bold mt-4">
            <span>Costo total del empleador:</span>
            <span>₡{{ formatAmount(selectedPeriod.totalSalaries + totalTaxes)
              }}</span>
          </p>
        </div>
      </div>
      <div v-if="report" class="d-flex justify-content-center"
        style="margin-bottom: 30px;">
        <button type="submit" class="btn btn-secondary"
          style="background-color: #405D72; color: white;
          border: transparent; margin-right: 30px;" @click="sendPDFFile">
          Enviar PDF a correo electrónico
        </button>

        <button type="submit" class="btn btn-secondary"
          style="background-color: #405D72; color: white;
          border: transparent;" @click="downloadPDFFile">
          Descargar PDF 
        </button>
    </div>
    <div class="d-flex justify-content-center">
      <div
        v-if="alertMessage"
        :class="['alert', alertType === 'success' ? 'alert-success'
        : 'alert-danger']"
        class="w-75"
        role="alert"
      >
        {{ alertMessage }}
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
      showPopup: false,
      alertMessage: "",
      alertType: "",
      selectedPeriodKey: "",
      selectedPeriod: null,
      employerTaxLabels: {
        totalEmployerCcssIvm: "IVM (Invalidez, Vejez y Muerte)",
        totalEmployerCcssSem: "SEM (Seguro Enfermedad/Maternidad)",
        totalEmployerLptBpop: "Cuota Patronal Banco Popular (0.25%)",
        totalEmployerOtrasFamiliares: "Asignaciones Familiares (5.00%)",
        totalEmployerOtrasImas: "IMAS (0.50%)",
        totalEmployerOtrasIna: "INA (1.50%)",
        totalEmployerOtrasBpop: "Aporte Banco Popular (0.25%)",
        totalEmployerLptFcl: "FCL - Fondo de Capitalización Laboral (3.00%)",
        totalEmployerLptOpc: "Fondo de Pensiones Complementarias (0.50%)",
        totalEmployerLptIns: "INS (1.00%)"
      }
    };
  },
  computed: {
    totalTaxes() {
      if (!this.selectedPeriod) return 0;
      return Object.keys(this.employerTaxLabels).reduce((sum, key) => {
        return sum + (this.selectedPeriod[key] || 0);
      }, 0);
    },
  visibleEmployerTaxes() {
    if (!this.selectedPeriod) return [];
    return Object.entries(this.employerTaxLabels)
      .filter(([key]) => Number(this.selectedPeriod[key]) > 0)
      .map(([key, label]) => ({
        key,
        label,
        value: Number(this.selectedPeriod[key]) || 0
      }));
  }

  },
  mounted() {
    this.fetchReport();
  },
  methods: {
    periodKey(p) {
      return `${new Date(p.startDate).getTime()}-${new Date(p.endDate).getTime()}`;
    },
    formatDate(date) {
      return new Date(date).toLocaleDateString("es-CR", {
        year: 'numeric',
        month: 'long',
        day: 'numeric'
      });
    },
    formatAmount(value) {
      const number = Number(value || 0);
      return number.toLocaleString("es-CR", {
        style: "decimal",
        useGrouping: true,
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
      });
    },
    updateSelectedPeriod() {
      this.selectedPeriod = this.report.periods.find(
        p => this.periodKey(p) === this.selectedPeriodKey
      );
    },
    async fetchReport() {
      this.loading = true;
      this.alertMessage = "";
      this.alertType = "";
      try {
        const response = await this.$api.showEmployerReport();
        const data = response.data;
        if (!data || !data.periodSummaries
          || data.periodSummaries.length === 0) {
          this.alertMessage = "No hay reportes disponibles.";
          this.alertType = "warning";
          return;
        }
        this.report = {
          fullName: data.employerFullName,
          companyName: data.companyName,
          periods: data.periodSummaries
        };
        this.selectedPeriodKey = this.periodKey(this.report.periods[0]);
        this.updateSelectedPeriod();
      } catch (err) {
          const statusCode = err.response.status;
          const errorMessage = err.response.data?.message || err.message;

          if (statusCode === 403) {
            this.alertMessage = "No tiene permisos para generar planillas.";
          } else if (statusCode === 500) {
            if (errorMessage.includes(
              "PayrollEmployer: Start date should be 1 day after latest end date."
            )) {
              this.alertMessage =
              "Ya existen registros de planilla para la fecha seleccionada.";           
            } else {
              this.alertMessage = "Error del servidor: " + errorMessage;
            }
          } else {
            this.alertMessage = errorMessage || "Ocurrió un error inesperado.";
          }
        this.alertType = "danger";
      } finally {
        this.loading = false;
      }
    },
    async sendPDFFile() {
      try {
        const pdfFile = await this.$utilities.generatePDF('print'
          , 'reporte_empleado', 'file');
        const emailData = new FormData();
        emailData.append("subject", "Reporte de pagos: empleador");

        emailData.append("message", `Estimado/a empleador/a,

        Le compartimos el reporte correspondiente a su pago de planilla.

        En el archivo adjunto encontrará el detalle del periodo, la información general de la empresa, los detalles de los salarios reportados y las contribuciones patronales.

        Atentamente,
        Equipo de Infinipay CO`);

        emailData.append("attachments", pdfFile);

        const response = await this.$api.sendEmail(emailData);
        if (typeof response?.data === "string"
          && response.data.includes("enviado")) {
          this.alertType = "success";
          this.alertMessage = response.data.message
          || "Correo enviado exitosamente.";
        } else {
          this.alertType = "danger";
          this.alertMessage = "Error al enviar el correo.";
        }
      } catch {
        this.alertType = "danger";
        this.alertMessage = "Ocurrió un error inesperado al enviar el correo.";
      }
    },
    async downloadPDFFile() {
      try {
        await this.$utilities.generatePDF("print", "reporte_empleador"
        , "download")
        this.alertType = "success";
        this.alertMessage = "PDF descargado correctamente.";
      } catch {
        this.alertType = "danger";
        this.alertMessage = "Ocurrió un error inesperado al descargar el pdf.";
      }
    },
    fixNameSpacing(fullName) {
      if (!fullName) return "";

      return fullName.replace(/([a-záéíóúñ])([A-ZÁÉÍÓÚÑ])/g, '$1 $2');
    }
  }
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