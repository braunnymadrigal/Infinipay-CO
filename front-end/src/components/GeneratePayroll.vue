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
      alertType: ""
    };
  },

  methods: {
    generatePayroll() {
      this.isLoading = true;
      this.payroll = null;
      this.alertMessage = "";
      this.alertType = "";

      const start = new Date(this.startDate.year, this.startDate.month - 1
        , this.startDate.day)
        .toISOString().split('T')[0];

      const end = new Date(this.endDate.year, this.endDate.month - 1
        , this.endDate.day)
        .toISOString().split('T')[0];

      const payload = {
        startDate: start,
        endDate: end
      };

      this.$api.generateEmployeePayroll(payload)
        .then(resp => {
          this.isLoading = false;
          this.payroll = resp.data.map(payroll => ({
            ...payroll,
            startDate: payload.startDate,
            endDate: payload.endDate,
          }));
          this.alertMessage = "Planilla generada correctamente.";
          this.alertType = "success";
        })
        .catch(err => {
          this.isLoading = false;
          this.alertMessage = err.response?.data?.message ||
           "No tiene permisos para generar planillas.";
          this.alertType = "error";
        });
      }
    }
};
</script>