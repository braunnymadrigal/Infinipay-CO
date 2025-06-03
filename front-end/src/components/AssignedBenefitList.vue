<template>
  <HeaderCompany />

  <div v-if="showPopup" @click.stop
       class="d-flex justify-content-center my-5 py-5">
    <div class="display-1 text-danger" style="padding: 150px;">
      No tiene permisos para ver esta informacion.
    </div>
  </div>

  <div v-else>
    <div class="card p-4 mx-auto"
         style="max-width: 1000px; background-color: #fff8f3; border: none">
      <h1 class="text-center"
          style="color: #405d72">
        Beneficios asignados
      </h1>
    </div>

    <div class="container mt-5 mb-5">
      <div class="row justify-content-start">
        <div class="col-2">
          <button type="button"
                  class="btn btn-success"
                  style="background-color: #405d72;
                border: transparent"
                  @click="openAddAssignedBenefitListModal()">
            Agregar
          </button>
        </div>
      </div>

      <table class="table is-bordered table-striped
           is-narrow is-hoverable is-fullwidth">
        <thead>
          <tr>
            <th style="white-space: nowrap">Nombre</th>
            <th style="white-space: nowrap">Descripción</th>
            <th style="white-space: nowrap;">Tiempo minimo</th>
            <th style="white-space: nowrap">Deducción</th>
            <th style="white-space: nowrap; text-align: center;">Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="assignedBenefits.length === 0">
            <td colspan="5" class="text-center">No hay beneficios asignados.</td>
          </tr>
          <tr v-else v-for="benefit in assignedBenefits" :key="benefit.name">
            <td>{{ benefit.benefit.name }}</td>
            <td>{{ truncateString(benefit.benefit.description, 150) }}</td>
            <td>{{ benefit.benefit.minEmployeeTime + " meses" }}</td>
            <td>{{ benefit.formattedDeduction }}</td>
            <td>
              <div class="d-flex justify-content-center gap-2">
                <button class="btn btn-danger btn-sm"
                        style="width: 70px;
                      border: transparent;
                      width: 70px">
                  Eliminar
                </button>
                <button class="btn btn-success btn-sm"
                        style="background-color: #405d72;
                      border: transparent;
                      width: 70px"
                        @click="openAssignedBenefitListModal(benefit)">
                  Ver
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>

    </div>

    <AssignedBenefitListModal v-if="showAssignedBenefitListModal"
                              :benefit="selectedBenefit"
                              @close="showAssignedBenefitListModal = false" />

    <AddAssignedBenefitListModal v-if="showAddAssignedBenefitListModal"
                                 :availableBenefits =
                                  "availableBenefits"

                                 :maxBenefitsPerEmployee =
                                  "maxBenefitsPerEmployee"

                                 :numAssignedBenefits =
                                  "assignedBenefits.length"

                                 @close="showAddAssignedBenefitListModal
                                  = false" />
  </div>
  <MainFooter />

</template>

<script>
  import HeaderCompany from "./HeaderCompany.vue";
  import MainFooter from "./MainFooter.vue";
  import AssignedBenefitListModal from "./AssignedBenefitListModal";
  import AddAssignedBenefitListModal from "./AddAssignedBenefitListModal";

  export default {
    components: {
      HeaderCompany,
      MainFooter,
      AssignedBenefitListModal,
      AddAssignedBenefitListModal,
    },

    data() {
      return {
        showPopup: false,
        showAssignedBenefitListModal: false,
        selectedBenefit: null,
        showAddAssignedBenefitListModal: false,
        allBenefits: [],
        assignedBenefits: [],
        availableBenefits: [],
        maxBenefitsPerEmployee: 0
      };
    },

    created() {
      this.getAssignedBenefits();
    },

    methods: {
      async getAssignedBenefits() {
        try {
          const benefitsUser = await this.$api.getAssignedBenefits();
          this.showPopup = false;
          this.allBenefits = await Promise.all(
            benefitsUser.data.map(async (benefit) => {
              benefit.formattedDeduction = await this.formulaFormat(benefit);
              return benefit;
            })
          );

          if (this.allBenefits.length != 0) {
            this.assignedBenefits = this.allBenefits.filter(b => b.assigned);
            this.availableBenefits = this.allBenefits.filter(b => !b.assigned);
            this.maxBenefitsPerEmployee =
              this.allBenefits[0].benefit.benefitsPerEmployee;
          }

        } catch (error) {
          this.showPopup = true;
          console.error("Error:", error);
          if (error.response) {
            const message = error.response.data?.message || "Error desconocido";
            alert(message);
          }
        }
      },

      truncateString(str, maxLength) {
        if (str.length > maxLength) {
          return str.substring(0, maxLength) + "...";
        }
        return str;
      },

      openAssignedBenefitListModal(benefit) {
        this.selectedBenefit = benefit;
        this.showAssignedBenefitListModal = true;
      },

      openAddAssignedBenefitListModal() {
        this.showAddAssignedBenefitListModal = true;
      },

      async formulaFormat(benefit) {
        if (benefit.benefit.deductionType === 'montoFijo') {
          return benefit.benefit.paramOneAPI + " CRC";
        } else if (benefit.benefit.deductionType === 'porcentaje') {
          return benefit.benefit.paramOneAPI;
        } else if (benefit.benefit.deductionType === 'api') {
          try {
            const response = await this.$api.benefitAPI(benefit.benefit);
            return response.data.resultado ?? "Resultado no habilitado.";
          } catch (error) {
            return "Resultado no habilitado.";
          }
        }
      },

    }
  };
</script>

<style>
</style>
