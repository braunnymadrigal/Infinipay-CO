<template>
  <div class="modal-backdrop" @click.self="$emit('close')">
      <div class="modal-content" v-if="benefit">
        <h3>{{ benefit.name }}</h3>
        <p>{{ benefit.description }}</p>
        <p><strong>Tiempo mínimo:</strong> {{ benefit.minTime }} meses</p>
        <p><strong>Califican:</strong> {{ benefit.elegible.join(', ') }}</p>
        <p><strong>Deducción:</strong> {{ benefit.formula.join(', ') }}</p>
        <div class="d-flex flex-column align-items-center"
             style="border: none">
          <p style="font-size: 15px">Creado por <a :href="'/'">
            {{'@' + benefit.creatorUser}}</a> el {{benefit.creationDate}}</p>
          <p style="font-size: 15px" v-if="benefit.modificationUser">
            Modificado por
            <a :href="'/'">
            {{'@' + benefit.modificationUser}}
            </a> el {{benefit.modificationDate}}
          </p>
        </div>
      <button class="btn btn-secondary mt-3" @click="$emit('close')">
        Cerrar
      </button>
    </div>
  </div>
</template>


<script>
  export default {
    name: "AssignedBenefitListModal",
    props: {
      benefit: {
        type: Object,
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
