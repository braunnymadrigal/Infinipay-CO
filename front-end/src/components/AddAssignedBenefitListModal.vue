<template>
  <div class="modal-backdrop" @click.self="$emit('close')">
    <div class="modal-content">
      <h3>Agregar beneficio</h3>
    </div>
  </div>
</template>

<script>
  export default {
    name: "AddAssignedBenefitListModal",
    props: {
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
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 9999;
  }

  .modal-content {
    background: white;
    padding: 20px;
    border-radius: 10px;
    min-width: 300px;
    max-width: 600px;
  }
</style>