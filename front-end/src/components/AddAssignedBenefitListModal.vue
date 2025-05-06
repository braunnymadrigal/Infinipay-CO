<template>
  <div class="modal-backdrop" @click.self="$emit('close')">
    <div class="modal-content">
      <div v-if="numAssignedBenefits === maxBenefitsPerEmployee"
           class="alert alert-warning">
        {{ warningMaxBenefits }}
      </div>
      <table class="table is-bordered table-striped
       is-narrow is-hoverable is-fullwidth">
        <thead>
          <tr>
            <th style="white-space: nowrap">Nombre</th>
            <th style="white-space: nowrap">Descripción</th>
            <th style="white-space: nowrap">Tiempo minimo</th>
            <th style="white-space: nowrap">Deducción</th>
            <th style="white-space: nowrap">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="!availableBenefits || availableBenefits.length === 0">
            <td colspan="4" class="text-center">No hay beneficios para 
            asignar.</td>
          </tr>
          <tr v-else v-for="benefit in availableBenefits" :key="benefit.name">
            <td>{{ benefit.benefitName }}</td>
            <td>{{ benefit.benefitDescription }}</td>
            <td>{{ benefit.benefitMinTime + " meses" }}</td>
            <td>{{ benefit.formattedDeduction }}</td>
            <td>
              <div class="d-flex justify-content-center gap-2">
                <button class="btn btn-success btn-sm"
                  :disabled="numAssignedBenefits === maxBenefitsPerEmployee"
                  style="width: 70px;
                  border: transparent;
                  width: 70px"
                        @click="assignBenefit(benefit)">
                  Agregar
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
  import axios from "axios";
  export default {
    name: "AddAssignedBenefitListModal",
    props: {
      availableBenefits: {
        type: Array,
        required: true,
      },
      maxBenefitsPerEmployee: {
        type: Number,
        required: true,
      },
      numAssignedBenefits: {
        type: Number,
        required: true,
      }
    },
    data() {
      return {
        selectedAddBenefit: null,
        warningMaxBenefits:
          "Máxima cantidad de beneficios asignables alcanzado.",
      };
    },
    methods: {
      async assignBenefit(selectedAddBenefit) {
        try {
          const response = await axios
            .get("https://localhost:7275/api/Login/GetLoggedUser", {
              headers: {
                Authorization: `Bearer ${this.$cookies.get(`jwt`)}`
              }
            });

          const userPersonId = response.data.PersonaId;

          axios.post(
            "https://localhost:7275/api/AssignedBenefitList/AssignBenefit", {
            userPersonId: userPersonId,
            benefitId: selectedAddBenefit.benefitId
          }).then(function (response) {
            console.log(response);
            window.location.href = "/AssignedBenefitList";
          });
        } catch(error) {
          if (error.response?.config?.url.includes("GetLoggedUser")) {
            console.error("Error obteniendo nombre de usuario", error);
          } else {
          console.error("Error al intentar asignar un beneficio", error);
          }
        }
      }
    }
  };
</script>

<style scoped>
  .modal-backdrop {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.7);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 9999;
  }

  .modal-content {
    opacity: 1 !important;
    background-color: white;
    padding: 20px;
    border-radius: 10px;
    min-width: 600px;
    max-width: 900px;
  }
</style>