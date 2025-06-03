<template>
  <HeaderCompany />

  <div v-if="!showPopup" @click.stop class="card p-4 mx-auto mb-5"
       style="max-width: 1920px; background-color: #fff8f3; border: none">


    <h1 class="text-center"
        style="color: #405d72">
      Registrar Horas
    </h1>
    <div class="d-flex flex-wrap gap-3 mb-3" style="margin-top: 20px;">
      <div class="d-flex align-items-center gap-2">
        <div style="width: 20px; height: 20px; background-color: #fff8f3;
          border: 1px solid #ccc;"></div>
        <span>No registrado</span>
      </div>
      <div class="d-flex align-items-center gap-2">
        <div style="width: 20px; height: 20px; background-color: #ff8667;">
        </div>
        <span>Pendiente</span>
      </div>
      <div class="d-flex align-items-center gap-2">
        <div style="width: 20px; height: 20px; background-color: #7fd1ae;">
        </div>
        <span>Aprobado</span>
      </div>
      <div class="d-flex align-items-center gap-2">
        <div style="width: 20px; height: 20px; background-color: #c35355;">
        </div>
        <span>Rechazado</span>
      </div>
    </div>

    <table class="table is-bordered is-fullwidth">
      <thead>
        <tr>
          <th class="weekHeader">Lunes</th>
          <th class="weekHeader">Martes</th>
          <th class="weekHeader">Miércoles</th>
          <th class="weekHeader">Jueves</th>
          <th class="weekHeader">Viernes</th>
          <th class="weekHeader">Sábado</th>
          <th class="weekHeader">Domingo</th>
        </tr>
      </thead>

      <tbody>
        <tr>
          <td class="weekHeader"
              v-for="(day, index) in workWeek" :key="formatDate(day)">
            <div class="d-flex justify-content-left gap-2"
                 style="color: #405d72">
              <span style="font-size: 12px">
                {{ day.toLocaleDateString('en-GB') }}
              </span>
            </div>
            <div class="d-flex justify-content-left gap-2 mt-3 mb-3">
              <span style="margin: 20px">
                Ingresar Horas:
              </span>
              <input v-if="!isLoading"
                     class="form-control"
                     type="number"
                     min="0"
                     max="9"
                     :value="alreadyRegistered(day)
                      ? monthlyHours[formatDate(day)].hoursWorked
                        : hours[index][1]"
                     :disabled="disableRegistration(day)
                      || alreadyRegistered(day)"
                     :style="{
                        width: '60px',
                        height: '40px',
                        marginTop: '10px',
                        backgroundColor: getBackgroundColor(day)
                      }"
                     @input="registerHour(index, $event.target.value)"
                     />
              <span v-else style="text-align: left">Cargando...</span>

            </div>
          </td>
        </tr>
      </tbody>
    </table>

    <div class="container mt-1 mb-5" style="max-width: 1920px;">
      <div class="d-flex justify-content-between"
           style="max-width: 1920px; margin: 0 auto;">
        <button type="button"
                class="btn btn-success"
                style="border: transparent;
                width: 110px"
                @click="getPreviousWeek">
          ← Anterior
        </button>

        <div class="d-flex justify-content-center">

          <button class="btn btn-danger btn-sm"
                  style="
                  border: transparent;
                  width: 80px;"
                  @click="resetHours">
            Cancelar
          </button>

          <button class="btn btn-success btn-sm"
                  style="
                  background-color: #405d72;
                  border: transparent;
                  width: 80px"
                  @click="registerEmployeeHours">
            Registrar
          </button>

        </div>

        <button type="button"
                class="btn btn-success"
                style="
                border: transparent;
                width: 110px"
                @click="getNextWeek">
          Siguiente →
        </button>
      </div>
    </div>
  </div>

  <div v-if="showPopup" @click.stop
    class="d-flex justify-content-center my-5 py-5">
    <div class="display-1 text-danger" style="padding: 150px;">
      No tiene permisos para ver esta información.
    </div>
   </div>
  <MainFooter />
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
        showPopup: false,
        week: [],
        baseDate: this.getMonday(new Date()),
        currentDate: new Date(),
        employeeType: "",
        isHourlyEmployee: false,
        hours: {},
        monthlyHours: {},
        isLoading: true,
      };
    },
    async created() {
      await this.getEmployeeHoursContract();

      if (this.isHourlyEmployee) {
        await this.getEmployeeHours();
      }
    },
    mounted() {
      this.resetHours();
    },
    methods: {
      async getEmployeeHoursContract() {
        try {
          const contract = await this.$api.getEmployeeHoursContract();
          if (contract == null) {
            throw new Error("No se puedo obtener información del contrato");
          }

          this.employeeType = contract.data.typeContract;
          this.isHourlyEmployee = contract.data.reportsHours;

        } catch (error) {
          this.showPopup = true;
          if (error.response) {
            const message = error.response.data?.message || "Error desconocido";
            console.error(message);
          }
        }
      },
      formatDate(date) {
        const d = new Date(date);
        const year = d.getFullYear();
        const month = String(d.getMonth() + 1).padStart(2, '0');
        const day = String(d.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
      },
      async getEmployeeHours() {
        this.isLoading = true;
        this.monthlyHours = {};
        try {
          if (this.baseDate <= this.getMonday(this.currentDate)) {
            const sunday = new Date(
              this.baseDate.getFullYear(),
              this.baseDate.getMonth(),
              this.baseDate.getDate()
            );

            sunday.setDate(sunday.getDate() + 6);
            const employeeHours = await this.$api.getEmployeeHours(
              this.formatDate(this.baseDate), this.formatDate(sunday));

            if (employeeHours == null) {
              throw new Error("No se recibio las horas del empleado");
            }

            for (let i = 0; i < employeeHours.data.length; i++) {
              this.monthlyHours[employeeHours.data[i].date]
                = employeeHours.data[i];
            }
          }

          } catch (error) {
            console.error("Error fetching employee hours:", error);
          } finally {
            this.isLoading = false;
        }
      },
      async registerEmployeeHours() {
        this.isLoading = true;

        try {
          const payload
            = this.hours.filter(h => h[1] > 0).map(([date, hoursWorked]) => ({
            date,
            hoursWorked
            }));
          console.log(payload);
          if (payload.length !== 0) {
            await this.$api.registerEmployeeHours(payload);
          }
        } catch (error) {
          console.error("Error registering employee hours:", error);
        } finally {
          this.isLoading = false;
          // window.location.href = "/EmployeeTimesheet";
        }
      },
      getMonday(date) {
        const day = date.getDay();
        const diff = (day === 0 ? -6 : 1) - day;
        const monday = new Date(
          date.getFullYear(),
          date.getMonth(),
          date.getDate()
        );

        monday.setDate(date.getDate() + diff);
        return monday;
      },
      getNextWeek() {
        const newDate = new Date(this.baseDate);
        newDate.setDate(this.baseDate.getDate() + 7);
        this.baseDate = this.getMonday(newDate);
        console.log(this.baseDate);
        this.resetHours();
        this.getEmployeeHours();
      },
      getPreviousWeek() {
        const newDate = new Date(this.baseDate);
        newDate.setDate(this.baseDate.getDate() - 7);
        this.baseDate = this.getMonday(newDate);
        console.log(this.baseDate);
        this.resetHours();
        this.getEmployeeHours();
      },
      resetHours() {
        const base = new Date(
          this.baseDate.getFullYear(),
          this.baseDate.getMonth(),
          this.baseDate.getDate()
        );

        this.hours = Array(7).fill(0).map((_, i) => {
          const date = new Date(base);
          date.setDate(date.getDate() + i);
          return [this.formatDate(date), 0];
        });
        console.log(this.hours);
      },
      alreadyRegistered(day) {
        const dateObj = day instanceof Date ? day : new Date(day);
        return Object.prototype.hasOwnProperty.call(
          this.monthlyHours, this.formatDate(dateObj));
      }
,
      disableRegistration(day) {
        let disable = true;
        if (this.isHourlyEmployee) {
          if (this.isSameMonthYear(day)) {
            if (!this.isPreviousDay(day)) {
              disable = true;
            } else if (this.employeeType === "servicios"
              || this.employeeType === "horas") {
              disable = !this.isSameWeek(day);
            } else if (this.employeeType == "medioTiempo") {
              disable = !this.isSameFortnight(day);
            } else if (this.employeeType == "tiempoCompleto") {
              disable = !this.isSameMonth(day);
            }
          }
        }
        return disable;
      },
      isSameMonthYear(day) {
        return this.currentDate.getYear() == day.getYear();
      },
      isPreviousDay(day) {
        const currentDay = this.currentDate;
        const target = new Date(day);

        currentDay.setHours(0, 0, 0, 0);
        target.setHours(0, 0, 0, 0);

        return currentDay >= target;
      },
      isSameWeek(day) {
        const monday = this.getMonday(this.currentDate);

        const sunday = new Date(monday);
        sunday.setDate(monday.getDate() + 6);

        const target = new Date(day);
        target.setHours(0, 0, 0, 0);

        return target >= monday && target <= sunday
      },
      isSameFortnight(day) {
        const ref = new Date(this.currentDate);
        const target = new Date(day);

        ref.setHours(0, 0, 0, 0);
        target.setHours(0, 0, 0, 0);

        const refFortnight = ref.getDate() <= 15;
        const targetFortnight = target.getDate() <= 15;

        return refFortnight === targetFortnight;
      },
      isSameMonth(day) {
        return day.getMonth() === this.currentDate.getMonth();
      },
      getBackgroundColor(day) {
        const formattedDay = this.formatDate(day);

        const entry = this.monthlyHours[formattedDay];
        if (!entry) {
          return "#fff8f3";
        }

        if (entry.approved == null) {
          return "#ff8667";
        } else if (entry.approved === true) {
          return "#7fd1ae";
        } else {
          return "#c35355";
        }
      },
      registerHour(index, value) {
        let numericValue = Number(value);
        if (numericValue < 0 || numericValue > 9 || isNaN(numericValue)) {
          numericValue = 0;
        }
        this.hours[index][1] = numericValue;
      },
    },
    computed: {
      workWeek() {
        const week = []
        for (let i = 0; i < 7; i++) {
          const date = new Date(this.baseDate);
          date.setDate(this.baseDate.getDate() + i);
          week.push(date);
        }
        return week;
      }
    }

  };
</script>

<style>
  .weekHeader {
    white-space: nowrap;
    border: 1px solid #405d72;
    text-align: center;
    
  }
</style>