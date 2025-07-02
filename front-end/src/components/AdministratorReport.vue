<template>
  <div>
    <HeaderCompany />
    <div class="container mt-4 text-center">
      <h1 style="color: #405D72; margin-bottom: 40px;">
        Histórico de Pago de Planilla
      </h1>

      <h5 class="mb-3" style="color: #758694; margin-top: 30px;
        margin-bottom: 30px;">
        Fecha de inicio
      </h5>

      <div class="row justify-content-center mb-3" style="margin-top: 30px;">
        <div class="col-md-3">
          <label class="form-label">Año</label>
          <select v-model.number="startDate.year"
                  class="form-select text-center">
            <option v-for="y in availableYears" :key="y" :value="y">
              {{ y }}
            </option>
          </select>
        </div>

        <div class="col-md-3">
          <label class="form-label">Mes</label>
          <select v-model.number="startDate.month"
                  class="form-select text-center">
            <option v-for="index in availableStartMonths" :key="index" :value="index">
              {{ months[index - 1] }}
            </option>
          </select>
        </div>

        <div class="col-md-3">
          <label class="form-label">Día</label>
          <select v-model.number="startDate.day"
                  class="form-select text-center">
            <option v-for="day in availableStartDays" :key="day" :value="day">
              {{ day }}
            </option>
          </select>
        </div>
      </div>

      <h5 class="mb-3" style="color: #758694; margin-top: 30px;
         margin-bottom: 30px;">
        Fecha Fin
      </h5>

      <div class="row justify-content-center mb-3" style="margin-top: 30px;">
        <div class="col-md-3">
          <label class="form-label">Año</label>
          <select v-model.number="endDate.year"
                  class="form-select text-center">
            <option v-for="y in availableYears" :key="y" :value="y">
              {{ y }}
            </option>
          </select>
        </div>

        <div class="col-md-3">
          <label class="form-label">Mes</label>
          <select v-model.number="endDate.month"
                  class="form-select text-center">
            <option v-for="index in availableEndMonths" :key="index" :value="index">
              {{ months[index - 1] }}
            </option>
          </select>
        </div>

        <div class="col-md-3">
          <label class="form-label">Día</label>
          <select v-model.number="endDate.day"
                  class="form-select text-center">
            <option v-for="day in availableEndDays" :key="day" :value="day">
              {{ day }}
            </option>
          </select>
        </div>
      </div>

      <div v-if="alertMessage" :class="['alert', alertType === 'success' ?
        'alert-success' : 'alert-danger']" role="alert"
           style="margin-bottom: 20px;">
        {{ alertMessage }}
      </div>

      <div class="mt-4">
        <button class="btn btn-primary"
                style="background-color: #758694; border: transparent;
                margin-bottom: 30px; margin-top: 30px;"
                :disabled="loading"
                @click="getAllCompaniesPayroll">
          {{ loading ? "Generando..." : "Generar historial" }}
        </button>
      </div>

      <table class="table table-bordered table-hover"
             v-if="!loading && companiesPayroll.length">
        <thead class="table-light">
          <tr>
            <th>Nombre de Empresa</th>
            <th>Frecuencia de Pago</th>
            <th>Periodo</th>
            <th>Fecha de Pago</th>
            <th>Salario Bruto</th>
            <th>Cargas Sociales Empleador</th>
            <th>Deducciones voluntarias</th>
            <th>Costo empleador</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(company, index) in companiesPayroll" :key="index">
            <td>{{ company.companyName }}</td>
            <td>{{ company.hiringType }}</td>
            <td>
              Del {{ company.startDate }}
              al {{ company.endDate }}
            </td>
            <td>{{ company.endDate }}</td>
            <td>₡{{ company.grossSalary }}</td>
            <td>₡{{ company.grossEmployerTax }}</td>
            <td>₡{{ company.voluntaryDeductionsTotal }}</td>
            <td>₡{{ company.totalEmployerCost }}</td>

          </tr>
        </tbody>
      </table>

      <p v-if="!loading && !companiesPayroll.length">
        No hay resultados para el rango seleccionado.
      </p>

      <p class="text-end text-muted">
        Total de compañias en esta página: {{ companiesPayroll.length }}
      </p>

      <div class="d-flex justify-content-center align-items-center mt-3 mb-4"
           v-if="false" style="gap: 15px;">
        <button class="btn btn-primary"
                style="background-color: #758694; border: transparent;">
          Anterior
        </button>

        <span>Página { currentPage } de { totalPages }</span>

        <button class="btn btn-primary"
                style="background-color: #758694; border: transparent;">
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
      const currentDate = new Date();
      return {
      currentDate,
      companiesPayroll: [],
      error: null,
      loading: false,
      alertMessage: "",
      alertType: "",
      months: [
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
        "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre",
      ],
        availableYears: Array.from({ length: currentDate.getFullYear() - 2009 }
          , (_, i) => 2010 + i),
      startDate: {
        day: 1,
        month: currentDate.getMonth() + 1,
        year: currentDate.getFullYear(),
      },
      endDate: {
        day: 1,
        month: currentDate.getMonth() + 1,
        year: currentDate.getFullYear(),
      },
    };
  },
    computed: {
      availableStartMonths() {
        const max = (this.startDate.year === this.currentDate.getFullYear())
          ? this.currentDate.getMonth() + 1
          : 12;
        return Array.from({ length: max }, (_, i) => i + 1);
      },

      availableStartDays() {
        const { year, month } = this.startDate;
        const today = this.currentDate;

        const isCurrentMonth =
          year === today.getFullYear() &&
          month === today.getMonth() + 1;

        const limit = isCurrentMonth
          ? today.getDate()
          : new Date(year, month, 0).getDate();

        return Array.from({ length: limit }, (_, i) => i + 1);
      },

      availableEndMonths() {
        const max = (this.endDate.year === this.currentDate.getFullYear())
          ? this.currentDate.getMonth() + 1
          : 12;
        return Array.from({ length: max }, (_, i) => i + 1);
      },

      availableEndDays() {
        const { year, month } = this.endDate;
        const today = this.currentDate;

        const isCurrentMonth =
          year === today.getFullYear() &&
          month === today.getMonth() + 1;

        const limit = isCurrentMonth
          ? today.getDate()
          : new Date(year, month, 0).getDate();

        return Array.from({ length: limit }, (_, i) => i + 1);
      },

      availableMonths() {
        const now = new Date();
        return (this.startDate.year === now.getFullYear())
          ? now.getMonth() + 1
          : 12;
      },

      availableDays() {
        const total = new Date(this.startDate.year, this.startDate.month
          , 0).getDate();
        return Array.from({ length: total }, (_, i) => i + 1);
      },
  },
    watch: {
      startDate: {
        handler() {
          this.clampDay('startDate');
          this.enforceRange('start');
        },
        deep: true,
      },

      endDate: {
        handler() {
          this.clampDay('endDate');
          this.enforceRange('end');
        },
        deep: true,
      },
    },
   methods: {

     daysArray(year, month) {
       const total = new Date(year, month, 0).getDate();
       return Array.from({ length: total }, (_, i) => i + 1);
     },

     clampDay(key) {
       const d = this[key];
       const max = new Date(d.year, d.month, 0).getDate();
       if (d.day > max) d.day = max;
     },

     async getAllCompaniesPayroll() {
       const start = new Date(this.startDate.year, this.startDate.month - 1,
         this.startDate.day);

       const end = new Date(this.endDate.year, this.endDate.month - 1
         , this.endDate.day);

       try {
         this.companiesPayroll = await this.$api.getAllCompaniesPayroll(
           start.toISOString().split("T")[0],
           end.toISOString().split("T")[0]
         );
         console.log(this.companiesPayroll);
       } catch (error) {
         const statusCode = error.response.status;
         const errorMessage = error.response.date?.message || error.message;

         if (statusCode === 403) {
           this.alertMessage = "No tiene permisos para generar planillas.";
         } else if (statusCode === 500) {
           this.alertMessage = "Error. Por favor recargar página.";
         } else {
           this.alertMessage = errorMessage;
         }
       }
     },

     enforceRange(source) {
       const start = new Date(this.startDate.year, this.startDate.month - 1
         , this.startDate.day);
       const end = new Date(this.endDate.year, this.endDate.month - 1
         , this.endDate.day);
       const today = new Date(this.currentDate.getFullYear()
         , this.currentDate.getMonth(), this.currentDate.getDate());

       if (start > today) this.copyDate('startDate', today);
       if (end > today) this.copyDate('endDate', today);

       if (start > end) {
         if (source === 'start') this.copyDate('endDate', start);
         else this.copyDate('startDate', end);
       }
     },

     copyDate(targetKey, dateObj) {
       this[targetKey].year = dateObj.getFullYear();
       this[targetKey].month = dateObj.getMonth() + 1;
       this[targetKey].day = dateObj.getDate();
     },
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