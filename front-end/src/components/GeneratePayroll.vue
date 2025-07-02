<template>
  <div>
    <HeaderCompany />

    <div class="container mt-4 text-center">

      <h1 style="color: #405D72">Generar Planilla</h1>

      <h5 style="color: #758694">Empresa {{ companyType }} </h5>

      <h5 class="mb-3" style="color: #758694; margin-top: 30px;
        margin-bottom: 30px;">Fecha de inicio</h5>

      <div class="row justify-content-center mb-3"
        v-if="companyType === 'mensual'" style="margin-top: 30px;">
        <div class="col-md-3">
          <label class="form-label">Año</label>
          <select v-model.number="startDate.year"
            class="form-select text-center">
            <option v-for="y in availableYears" :key="y" :value="y">{{ y }}
            </option>
          </select>
        </div>

        <div class="col-md-3">
          <label class="form-label">Mes</label>
          <select v-model.number="startDate.month"
            class="form-select text-center">
            <option v-for="(m, i) in months" :key="i" :value="i + 1">{{ m }}
            </option>
          </select>
        </div>
      </div>

      <div class="row justify-content-center mb-3"
        v-else-if="companyType === 'quincenal'" style="margin-top: 30px;">
        <div class="col-md-3">
          <label class="form-label">Año</label>
          <select v-model.number="startDate.year"
            class="form-select text-center">
            <option v-for="y in availableYears" :key="y" :value="y">{{ y }}
            </option>
          </select>
        </div>

        <div class="col-md-3">
          <label class="form-label">Mes</label>
          <select v-model.number="startDate.month"
            class="form-select text-center">
            <option v-for="(m, i) in months" :key="i" :value="i + 1">{{ m }}
            </option>
          </select>
        </div>

        <div class="col-md-3">
          <label class="form-label">Quincena</label>
          <select v-model.number="selectedQuincena"
            class="form-select text-center">
            <option :value="1">Primera</option>
            <option :value="2">Segunda</option>
          </select>
        </div>

        <p class="mb-3" style="color: #758694; margin-top: 50px;">
          Nota: Para la primera quincena se toman en cuenta los días del 1
          al 15 del mes; para la segunda quincena, los días del 16 al último
          día del mes.</p>
      </div>
      
      <div v-else class="alert alert-warning mt-4">
        Este tipo de planilla no está habilitado.
      </div>

      <div class="mt-4">
        <button
          class="btn btn-primary"
          style="background-color: #758694; border: transparent;
          margin-bottom: 30px; margin-top: 30px;"
          :disabled="isLoading"
          @click="generatePayroll"
        >
          {{ isLoading ? "Generando..." : "Generar planilla" }}
        </button>
      </div>

      <div v-if="alertMessage" :class="['alert', alertType === 'success'
        ? 'alert-success' : 'alert-danger']" role="alert">
        {{ alertMessage }}
      </div>

      <div v-if="dateAlertMessage" class="alert alert-warning" role="alert">
        {{ dateAlertMessage }}
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
    const currentYear = new Date().getFullYear();
    return {
      months: [
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
        "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre",
      ],
      availableYears:
        Array.from({ length: currentYear - 2009 }, (_, i) => 2010 + i),
      startDate: {
        day: 1,
        month: 1,
        year: currentYear
      },
      isLoading: false,
      payroll: null,
      alertMessage: "",
      alertType: "",
      dateAlertMessage: "",
      companyType: null,
      selectedQuincena: 1,
    };
  },

  mounted() {
    this.getCompanyType();
  },

  methods: {
    getLastDayOfMonth(year, month) {
      return new Date(year, month, 0).getDate();
    },

    async getCompanyType() {
      try {
        const response = await this.$api.getCompanyType();
        this.companyType = response.data?.toLowerCase();
      } catch (err) {
        if (!err.response) {
          console.error("Network error o sin respuesta");
        } else {
          const statusCode = err.response.status;
          const errorMessage = err.response.data?.message || err.message;
          console.error(`Error ${statusCode}: ${errorMessage}`);
        }
      }
    },

    clearTime(date) {
      return new Date(date.getFullYear(), date.getMonth(), date.getDate());
    },

    async generatePayroll() {
      this.isLoading = true;
      this.payroll = null;
      this.alertMessage = "";
      this.alertType = "";
      this.dateAlertMessage = "";

      let start = new Date(this.startDate.year, this.startDate.month - 1, 1);
      let end;

      if (this.companyType === "mensual") {
        end = new Date(
          this.startDate.year,
          this.startDate.month - 1,
          this.getLastDayOfMonth(this.startDate.year, this.startDate.month)
        );
      } else if (this.companyType === "quincenal") {
        if (this.selectedQuincena === 1) {
          start.setDate(1);
          end = new Date(this.startDate.year, this.startDate.month - 1, 15);
        } else {
          start.setDate(16);
          const lastDay = this.getLastDayOfMonth(this.startDate.year
          , this.startDate.month);
          end = new Date(this.startDate.year, this.startDate.month - 1
          , lastDay);
        }
      } else {
        this.alertMessage =
        "Las planillas para compañías con pagos semanales no están soportadas.";
        this.alertType = "danger";
        this.isLoading = false;
        return;
      }

      const payload = {
        startDate: start.toISOString().split("T")[0],
        endDate: end.toISOString().split("T")[0]
      };

      try {
        await this.$api.generateEmployeePayroll(payload);
        this.payroll = payload;
        this.alertMessage = "Planilla generada correctamente.";
        this.alertType = "success";
      } catch (err) {
        if (!err.response) {
          this.alertMessage = "No tiene permisos para generar planillas.";
        } else {
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
            } else if (statusCode === 500) {
              "PayrollEmployer: Date range month is in the future."
              this.alertMessage =
              "No se pueden seleccionar fechas futuras para la generación de planilla";  
            }
            else {
              this.alertMessage = "Error del servidor: " + errorMessage;
            }
          } else {
            this.alertMessage = errorMessage || "Ocurrió un error inesperado.";
          }
        }
        this.alertType = "danger";
      } finally {
        this.isLoading = false;
      }
    }
  }
};
</script>