<template>
  <div class="modal-backdrop" @click.self="$emit('close')">
      <div class="modal-content" v-if="benefit">
        <h3>{{ benefit.benefit.name }}</h3>
        <p>{{ benefit.benefit.description }}</p>
        <p><strong>Tiempo mínimo:</strong> {{ benefit.benefit.minEmployeeTime }} 
          meses</p>
        <p><strong>Deducción:</strong> {{ benefit.formattedDeduction }}</p>
        <div class="d-flex flex-column align-items-center"
             style="border: none">
          <p style="font-size: 15px">Creado por 
          <a :href="'/AssignedBenefitList'">
            {{'@' + benefit.benefit.userCreator}}
          </a>
            el {{formatDate(benefit.benefit.creationDate)}}</p>
          <p style="font-size: 15px" v-if="benefit.userModifier">
            Modificado por
            <a :href="'/AssignedBenefitList'">
              {{'@' + benefit.benefit.userModifier}}
            </a> el {{formatDate(benefit.benefit.modifiedDate)}}
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
    },
    methods: {
      formatDate(date) {
        const formattedDate = new Date(date);
        const day = String(formattedDate.getDate()).padStart(2, '0');
        const month = String(formattedDate.getMonth() + 1).padStart(2, '0');
        const year = formattedDate.getFullYear();
        return `${day}/${month}/${year}`;
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
    opacity: 1;
  }

  .modal-content {
    opacity: 100 !important;
    background: white !important;
    color: black !important;
    box-shadow: 0 0 20px rgba(0,0,0,0.7);
    z-index: 10000;
    padding: 20px;
    border-radius: 10px;
    min-width: 300px;
    max-width: 600px;
  }
</style>
