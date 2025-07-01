<template>
  <div class="modal-backdrop" @click.self="$emit('close')">
    <div class="modal-content" v-if="benefit">
      <h3 class="mb-3">¿Está seguro de eliminar este beneficio?</h3>
      <p>
        <strong>{{ benefit.benefit.name }}</strong>
      </p>
      <div class="modal-buttons">
        <button class="btn btn-sm btn-secondary" @click="$emit('close')">
          Cancelar
        </button>
        <button
          id="confirmDeleteButton"
          class="btn btn-sm btn-danger"
          @click="confirmDelete"
        >
          Confirmar
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "DeleteBenefitModal",
  props: {
    benefit: {
      type: Object,
      required: true,
    },
  },
  methods: {
    async deleteBenefit(benefitId) {
      try {
        await this.$api.deleteCompanyBenefit(benefitId);
        this.$emit("deleted");
        this.$emit("close");
      } catch (error) {
        console.error("Error borrando el beneficio", error);
      }
    },
    confirmDelete() {
      this.deleteBenefit(this.benefit.benefit.id);
    },
  },
};
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
  opacity: 1;
}

.modal-content {
  opacity: 100 !important;
  background: white !important;
  color: black !important;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.7);
  z-index: 10000;
  padding: 20px;
  border-radius: 10px;
  min-width: 300px;
  max-width: 600px;
}

.modal-buttons {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}
</style>
