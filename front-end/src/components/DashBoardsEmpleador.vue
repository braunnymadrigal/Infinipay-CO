<template>
  <HeaderCompany />
  <div
    class="charts-container"
    style="
      display: flex;
      flex-wrap: wrap;
      gap: 2rem;
      justify-content: center;
      margin: 50px auto;
    "
  >
    <div style="max-width: 400px; min-height: 300px">
      <Pie
        v-if="contractTypeChart.data && hasContractTypeData"
        :data="contractTypeChart.data"
        :options="contractTypeChart.options"
      />
      <div v-else class="text-center" style="padding: 100px 0; color: #888">
        Datos no disponibles para tipo de contrato
      </div>
    </div>

    <div style="max-width: 1000px; height: 400px; min-height: 300px">
      <Bar
        v-if="payrollChart.data && hasPayrollData"
        :data="payrollChart.data"
        :options="payrollChart.options"
      />
      <div v-else class="text-center" style="padding: 150px 0; color: #888">
        Datos no disponibles para pagos de planilla
      </div>
    </div>

    <div style="min-width: 300px; max-width: 500px; min-height: 300px">
      <h3
        v-if="benefitAssignments.length > 0"
        style="text-align: center; margin-bottom: 1rem"
      >
        Beneficios Asignados a Empleados
      </h3>
      <table v-if="benefitAssignments.length > 0" class="table table-striped">
        <thead>
          <tr>
            <th>Beneficio</th>
            <th>Cantidad</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in benefitAssignments" :key="item.Key">
            <td>{{ item.Key }}</td>
            <td>{{ item.Value }}</td>
          </tr>
        </tbody>
      </table>
      <div v-else class="text-center" style="padding: 100px 0; color: #888">
        Datos no disponibles para asignación de beneficios
      </div>
    </div>
  </div>
  <MainFooter />
</template>

<script>
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  ArcElement,
  BarElement,
  CategoryScale,
  LinearScale,
} from "chart.js";

import { Pie, Bar } from "vue-chartjs";
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";

ChartJS.register(
  Title,
  Tooltip,
  Legend,
  ArcElement,
  BarElement,
  CategoryScale,
  LinearScale
);

export default {
  name: "EmployerDashboard",
  components: {
    HeaderCompany,
    MainFooter,
    Pie,
    Bar,
  },
  data() {
    return {
      employees: [],
      contractTypeChart: { data: null, options: null },
      payrollChart: { data: null, options: null },
      benefitAssignments: [],
      selectedPayroll: null,
    };
  },
  computed: {
    hasContractTypeData() {
      return (
        this.contractTypeChart.data &&
        this.contractTypeChart.data.datasets[0].data.some((v) => v > 0)
      );
    },
    hasPayrollData() {
      return (
        this.payrollChart.data &&
        this.payrollChart.data.datasets[0].data.some((v) => v > 0)
      );
    },
  },
  methods: {
    async getEmployees() {
      try {
        const response = await this.$api.getEmployeesData();
        this.employees = response.data;
        this.buildContractChart();
      } catch (error) {
        console.error("Error fetching employees:", error);
      }
    },
    async getBenefitsPerEmployee() {
      try {
        const response = await this.$api.getBenefitAssignments();
        this.benefitAssignments = response.data;
      } catch (error) {
        console.error("Error fetching benefit assignments:", error);
      }
    },
    async getPayrollReport() {
      try {
        const response = await this.$api.showEmployerReport();
        const report = response.data;
        if (report?.periodSummaries?.length) {
          this.selectedPayroll = report.periodSummaries[0];
          this.buildPayrollChart();
        }
      } catch (error) {
        console.error("Error fetching payroll report:", error);
      }
    },
    buildContractChart() {
      const contractData = this.countByField("typeContract");
      const labels = Object.keys(contractData);
      const values = Object.values(contractData);
      const colorPalette = [
        "#4CAF50",
        "#2196F3",
        "#FF9800",
        "#9C27B0",
        "#F44336",
        "#00BCD4",
      ];
      this.contractTypeChart = {
        data: {
          labels,
          datasets: [
            {
              data: values,
              backgroundColor: labels.map(
                (_, i) => colorPalette[i % colorPalette.length]
              ),
            },
          ],
        },
        options: {
          responsive: true,
          plugins: {
            title: {
              display: true,
              text: "Employeados Por Tipo de Contrato",
              font: {
                size: 26,
                color: "black",
              },
            },
            legend: {
              position: "top",
            },
          },
        },
      };
    },
    buildPayrollChart() {
      const p = this.selectedPayroll;
      if (!p) return;

      const labels = [
        "Deducciones voluntarias",
        "Cargas sociales",
        "Salarios a empleados",
      ];

      const deduccionesVoluntarias = Number(
        p.totalEmployeeVoluntaryDeductions || 0
      );

      const cargasSociales =
        Number(p.totalEmployerCcssIvm || 0) +
        Number(p.totalEmployerCcssSem || 0) +
        Number(p.totalEmployerFclBpop || 0) +
        Number(p.totalEmployerOtrasFamiliares || 0) +
        Number(p.totalEmployerOtrasImas || 0) +
        Number(p.totalEmployerOtrasIna || 0) +
        Number(p.totalEmployerPensionBpop || 0) +
        Number(p.totalEmployerLptFcl || 0) +
        Number(p.totalEmployerLptOpc || 0) +
        Number(p.totalEmployerLptIns || 0);

      const salarios = Number(p.totalSalaries || 0);

      const values = [deduccionesVoluntarias, cargasSociales, salarios];

      this.payrollChart = {
        data: {
          labels,
          datasets: [
            {
              label: "Monto (₡)",
              data: values,
              backgroundColor: ["#e67e22", "#2ecc71", "#3498db"],
              barThickness: 50,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          indexAxis: "x",
          layout: {
            padding: 5,
          },
          plugins: {
            title: {
              display: true,
              text: "Distribución de pagos de planilla",
              font: {
                size: 26,
                color: "black",
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
                precision: 0,
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
    countByField(field) {
      return this.employees.reduce((acc, emp) => {
        const value = emp[field] || "no especificado";
        acc[value] = (acc[value] || 0) + 1;
        return acc;
      }, {});
    },
  },
  async mounted() {
    await this.getEmployees();
    await this.getBenefitsPerEmployee();
    await this.getPayrollReport();
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
