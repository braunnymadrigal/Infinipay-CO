<template>
  <div>
    <HeaderCompany />

    <div class="container mt-4 text-center">
      <h1 style="color: #405D72">Generar Planilla</h1>

      <div class="row justify-content-center mb-3">
        <h5 style="color: #758694; margin-top: 20px; margin-bottom: 20px;"
        class="mb-3">Fecha de inicio</h5>
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
          <label class="form-label">Día</label>
          <select v-model.number="startDate.day"
            class="form-select text-center">
            <option v-for="d in 31" :key="d" :value="d">{{ d }}</option>
          </select>
        </div>
      </div>

      <div class="row justify-content-center mb-3">
        <h5 style="color: #758694; margin-top: 20px; margin-bottom: 20px;"
        class="mb-3">Fecha de finalización</h5>
        <div class="col-md-3">
          <label class="form-label">Año</label>
          <select v-model.number="endDate.year" class="form-select text-center">
            <option v-for="y in availableYears" :key="y" :value="y">{{ y }}
            </option>
          </select>
        </div>

        <div class="col-md-3">
          <label class="form-label">Mes</label>
          <select v-model.number="endDate.month"
            class="form-select text-center">
            <option v-for="(m, i) in months" :key="i" :value="i + 1">{{ m }}
            </option>
          </select>
        </div>
        <div class="col-md-3">
          <label class="form-label">Día</label>
          <select v-model.number="endDate.day" class="form-select text-center">
            <option v-for="d in 31" :key="d" :value="d">{{ d }}</option>
          </select>
        </div>
      </div>

      <div class="mt-4">
        <button
          class="btn btn-primary"
          style="background-color: #758694; border: transparent;
            margin-bottom: 40px;" :disabled="isLoading" @click="generatePayroll"
        >
          {{ isLoading ? "Generando..." : "Generar planilla" }}
        </button>
      </div>

      <div v-if="alertMessage" :class="['alert', alertType === 'success'
        ? 'alert-success' : 'alert-danger']" role="alert"
        style="margin-bottom: 20px;">
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
      availableYears: Array.from({ length: 5 }, (_, i) => currentYear - 2 + i),
      startDate: {
        day: 1,
        month: 1,
        year: currentYear
      },
      endDate: {
        day: 1,
        month: 1,
        year: currentYear
      },
      isLoading: false,
      payroll: null,
      alertMessage: "",
      alertType: "",
      dateAlertMessage: ""
    };
  },

  methods: {
    clearTime(date) {
      return new Date(date.getFullYear(), date.getMonth(), date.getDate());
    },

    async generatePayroll() {
      this.isLoading = true;
      this.payroll = null;
      this.alertMessage = "";
      this.alertType = "";
      this.dateAlertMessage = "";

      const start = new Date(this.startDate.year, this.startDate.month - 1
        , this.startDate.day);
      const end = new Date(this.endDate.year, this.endDate.month - 1
        , this.endDate.day);
      const today = this.clearTime(new Date());

      const startClean = this.clearTime(start);
      const endClean = this.clearTime(end);

      if (startClean > today || endClean > today) {
        this.dateAlertMessage = "No se pueden seleccionar fechas futuras.";
        this.isLoading = false;
        return;
      }

      const payload = {
        startDate: start.toISOString().split('T')[0],
        endDate: end.toISOString().split('T')[0]
      };

      try {
        await this.$api.generateEmployeePayroll(payload);
        this.payroll = {
          startDate: payload.startDate,
          endDate: payload.endDate
        };
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
            if (errorMessage.includes("Payroll employee table could not be extracted")) {
              this.alertMessage ="No hay datos para calcular planilla o ya existen registros para la fecha seleccionada.";
            } else if (errorMessage.includes("The computed gross salary can not be less than zero")) {
              this.alertMessage = "Validar datos de empleados.";
            } else {
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
