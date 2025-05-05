<template>
  <div class="modal-backdrop" @click.self="$emit('close')">
    <div class="modal-content">
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
          <tr v-for="benefit in allBenefits" :key="benefit.name">
            <td>{{ benefit.benefitName }}</td>
            <td>{{ benefit.benefitDescription }}</td>
            <td>{{ benefit.benefitMinTime + " meses" }}</td>
            <td>{{ benefit.formattedDeduction }}</td>
            <td>
              <div class="d-flex justify-content-center gap-2">
                <button class="btn btn-success btn-sm"
                        style="width: 70px;
                  border: transparent;
                  width: 70px">
                  Agregar
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
  </div>
</template>

<script>
  export default {
    name: "AddAssignedBenefitListModal",
    props: {
      allBenefits: {
        type: Array,
        required: true
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