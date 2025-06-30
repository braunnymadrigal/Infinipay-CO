<template>
  <HeaderCompany />
  <div
    style="
      height: 100%;
      margin: 50px 0px;
      display: flex;
      justify-content: center;
    "
  >
    <div style="max-width: 500px; max-height: 500px">
      <Pie
        v-if="chartData"
        :data="chartData"
        :options="chartOptions"
        class="w-full md:w-[30rem]"
      />
    </div>
  </div>
  <MainFooter />
</template>

<script>
import { Chart as ChartJS, Title, Tooltip, Legend, ArcElement } from "chart.js";
import { Pie } from "vue-chartjs";
import HeaderCompany from "./HeaderCompany.vue";
import MainFooter from "./MainFooter.vue";

ChartJS.register(Title, Tooltip, Legend, ArcElement);

export default {
  name: "DashBoardsEmpleador",
  components: {
    HeaderCompany,
    MainFooter,
    Pie,
  },
  data() {
    return {
      empleados: [],
      chartData: null,
      chartOptions: null,
    };
  },
  methods: {
    async getEmployees() {
      try {
        const response = await this.$api.getEmployeesData();
        this.empleados = response.data;
        this.buildChart();
      } catch (error) {
        console.error("Error obteniendo los empleados:", error);
      }
    },
    buildChart() {
      const colorMap = {
        Limón: "#4CAF50", // Verde
        Guanacaste: "#FFEB3B", // Amarillo
        Cartago: "#2196F3", // Azul
        "San Jose": "#9C27B0", // Morado
        Puntarenas: "#FF9800", // Naranja
        Alajuela: "#F44336", // Rojo
        Heredia: "#00BCD4", // Celeste
      };

      const conteoPorProvincia = this.empleados.reduce((acc, emp) => {
        const provincia = emp.province || "Desconocido";
        acc[provincia] = (acc[provincia] || 0) + 1;
        return acc;
      }, {});

      const provincias = Object.keys(conteoPorProvincia);

      this.chartData = {
        labels: provincias,
        datasets: [
          {
            data: Object.values(conteoPorProvincia),
            backgroundColor: provincias.map((p) => colorMap[p] || "#BDBDBD"), // Gris por defecto si no está en el mapa
            hoverBackgroundColor: provincias.map(
              (p) => colorMap[p] || "#9E9E9E"
            ),
          },
        ],
      };

      this.chartOptions = {
        responsive: true,
        plugins: {
          legend: {
            position: "top",
          },
          title: {
            display: true,
            text: "Empleados por Provincia",
          },
        },
      };
    },
  },
  mounted() {
    this.getEmployees();
  },
};
</script>

<style scoped lang="scss"></style>
