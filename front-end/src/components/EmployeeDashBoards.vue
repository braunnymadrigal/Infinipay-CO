<template>
  <div>
    <HeaderCompany />

    <div
      v-if="report && selectedPayment"
      class="charts-container"
      style="
        display: flex;
        flex-wrap: wrap;
        gap: 2rem;
        justify-content: center;
        margin: 50px auto;
      "
    >
      <div style="max-width: 1000px; height: 400px; min-height: 300px">
        <Bar
          v-if="payrollChart.data && hasPayrollChartData"
          :data="payrollChart.data"
          :options="payrollChart.options"
        />
        <div v-else class="text-center" style="padding: 150px 0; color: #888">
          Datos no disponibles para gráfico de planilla
        </div>
      </div>

      <div style="min-width: 300px; max-width: 500px; min-height: 300px">
        <h3 style="text-align: center; margin-bottom: 1rem">
          Información de {{ this.profile?.PrimerNombre || "Empleado" }}
        </h3>
        <table class="table table-striped">
          <thead>
            <tr>
              <th>Llave</th>
              <th>Valor</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>Beneficios Asignados</td>
              <td>{{ assignedBenefits.length }}</td>
            </tr>
            <tr>
              <td>Salario Bruto</td>
              <td>{{ selectedPayment.computedGrossSalary }}₡</td>
            </tr>
            <tr>
              <td>Salario Neto</td>
              <td>{{ selectedPayment.netSalary }}₡</td>
            </tr>
            <tr>
              <td>Monto rebajado por cargas sociales</td>
              <td>{{ totalObligatorias }}₡</td>
            </tr>
            <tr>
              <td>Monto rebajado por beneficios</td>
              <td>{{ totalVoluntarias }}₡</td>
            </tr>
            <tr>
              <td>Provincia</td>
              <td>{{ profile.Provincia }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <MainFooter />
  </div>
</template>

<script>
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";
import { Bar } from "vue-chartjs";
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
} from "chart.js";

ChartJS.register(
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale
);

export default {
  name: "EmployeePayrollChart",
  components: {
    HeaderCompany,
    MainFooter,
    Bar,
  },
  data() {
    return {
      report: null,
      selectedPayment: null,
      payrollChart: { data: null, options: null },
      assignedBenefits: [],
      profile: null,
      totalObligatorias: 0,
      totalVoluntarias: 0,
    };
  },
  computed: {
    hasPayrollChartData() {
      return (
        this.payrollChart.data &&
        this.payrollChart.data.datasets[0].data.some((v) => v > 0)
      );
    },
  },
  methods: {
    async fetchPayrollReport() {
      try {
        const response = await this.$api.showEmployeeReport();
        this.report = response.data;
        if (
          this.report &&
          this.report.payments &&
          this.report.payments.length > 0
        ) {
          this.selectedPayment = this.report.payments[0];
          this.buildPayrollChart();
        }
      } catch (err) {
        console.error("Error al obtener datos del reporte:", err);
      } finally {
        this.loading = false;
      }
    },
    async getProfile() {
      try {
        const response = await this.$api.getProfile();
        this.showPopup = false;
        this.profile = response.data;
      } catch (err) {
        console.error("Error al obtener el perfil:", err);
      }
    },
    async getAssignedBenefits() {
      try {
        const response = await this.$api.getAssignedBenefits();
        this.assignedBenefits = response.data;
      } catch (err) {
        console.error("Error al obtener asignaciones de beneficios:", err);
      }
    },
    buildPayrollChart() {
      const p = this.selectedPayment;
      if (!p) return;

      this.totalObligatorias =
        p.deductions
          ?.filter((d) => this.isObligatoria(d.type))
          .reduce((sum, d) => sum + d.amount, 0) || 0;

      this.totalVoluntarias =
        p.deductions
          ?.filter((d) => !this.isObligatoria(d.type))
          .reduce((sum, d) => sum + d.amount, 0) || 0;

      const salarioBruto = Number(p.computedGrossSalary || 0);
      const pagoNeto = Number(p.netSalary || 0);

      const labels = [
        "Salario Bruto",
        "Deducciones obligatorias",
        "Deducciones voluntarias",
        "Pago Neto",
      ];
      const values = [
        salarioBruto,
        this.totalObligatorias,
        this.totalVoluntarias,
        pagoNeto,
      ];

      this.payrollChart = {
        data: {
          labels,
          datasets: [
            {
              label: "Monto (₡)",
              data: values,
              backgroundColor: ["#3498db", "#e74c3c", "#f39c12", "#2ecc71"],
              barThickness: 50,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            title: {
              display: true,
              text: "Distribución de pago salarial",
              font: {
                size: 26,
              },
            },
            legend: {
              display: false,
            },
            tooltip: {
              callbacks: {
                label: function (context) {
                  return `₡${context.raw.toLocaleString("es-CR")}`;
                },
              },
            },
          },
          scales: {
            x: {
              ticks: {
                font: {
                  size: 13,
                },
              },
            },
            y: {
              beginAtZero: true,
              ticks: {
                callback: (value) => `₡${value.toLocaleString("es-CR")}`,
              },
              title: {
                display: true,
                text: "Monto en colones (₡)",
              },
            },
          },
        },
      };
    },
    isObligatoria(type) {
      if (!type) return false;
      const obligatorias = [
        "empleado_ccss_ivm",
        "empleado_ccss_sem",
        "empleado_renta",
        "empleado_lpt_bpop",
      ];
      return obligatorias.includes(type.toLowerCase());
    },
  },
  mounted() {
    this.fetchPayrollReport();
    this.getProfile();
    this.getAssignedBenefits();
  },
};
</script>

<style scoped>
.table {
  width: 100%;
  border-collapse: collapse;
}
.table th,
.table td {
  padding: 0.5rem;
  border: 1px solid #ccc;
}
</style>
