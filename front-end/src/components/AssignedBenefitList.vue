<template>
  <HeaderCompany />

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
                style="background-color: #405d72; border: transparent">
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
          <th style="white-space: nowrap">Tiempo minimo</th>
          <th style="white-space: nowrap">Califican</th>
          <th style="white-space: nowrap">Deducción</th>
          <th style="white-space: nowrap">Acciones</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="benefit in benefits" :key="benefit.name">
          <td>{{ benefit.name }}</td>
          <td>{{ truncateString(benefit.description, 150) }}</td>
          <td>{{ benefit.minTime + " meses" }}</td>
          <td>{{ benefit.elegible.join(', ') }}</td>
          <td>{{ benefit.formula.join(', ') }}</td>
          <td>
            <div class="d-flex justify-content-center gap-2">
              <button class="btn btn-danger btn-sm" style="width: 70px">
                Eliminar
              </button>
              <button class="btn btn-success btn-sm"
                      style="background-color: #405d72;
                      border: transparent;
                      width: 70px"
                      @click="openModal(benefit)">
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
  <MainFooter />

</template>

<script>
  import HeaderCompany from "./HeaderCompany.vue";
  import MainFooter from "./MainFooter.vue";
  import AssignedBenefitListModal from"./AssignedBenefitListModal";

  export default {
    components: {
      HeaderCompany,
      MainFooter,
      AssignedBenefitListModal
    },
    data() {
      return {
        showAssignedBenefitListModal: false,
        selectedBenefit: null,
        benefits: [
          {
            name: "Plan Dental",
            description: `Nuestro plan dental ofrece cobertura integral que
            incluye limpiezas semestrales, exámenes, radiografías
            y descuentos en tratamientos como empastes y ortodoncia,
            garantizando salud bucal y ahorro para toda la familia.`,
            minTime: 5,
            elegible: ['Semanal', 'Quincenal'],
            formula: ['32 0000 CRC'],
            creationDate: '2/05/2025',
            creatorUser: 'fodp3323',
            modificationDate: null,
            modificationUser: null,
          },
          {
            name: "Servicio Automotores",
            description: `Nuestro plan de servicios para autos incluye
            mantenimiento preventivo, cambio de aceite, revisiones generales y
            asistencia en carretera, brindando tranquilidad, seguridad y mayor
            duración para tu vehículo todos los días.`,
            minTime: 1,
            elegible: ['Mensual', 'Semanal', 'Quincenal'],
            formula: ['32 0000 CRC'],
            creationDate: '2/03/2025',
            creatorUser: 'pep123',
            modificationDate: '4/04/2025',
            modificationUser: 'pop123'
          }
        ]
      };
    },
    methods: {
      truncateString(str, maxLength) {
        if (str.length > maxLength) {
          return str.substring(0, maxLength) + "...";
        }
        return str;
      },
      openModal(benefit) {
        this.selectedBenefit = benefit;
        this.showAssignedBenefitListModal = true;
      }
    }
  };
</script>

<style scoped>
</style>
