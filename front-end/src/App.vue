<template>
  <div id="app">
    <!-- Iterar sobre los "pasos" -->
    <div v-for="(step, index) in steps" :key="index">
      <component
        :is="step.component"
        v-if="index === currentStep"
        @next="goToNextStep"
        @previous="goToPreviousStep"
      />
    </div>
  </div>
</template>

<script>
import RegistroEmpleador from './components/RegistroEmpleador.vue'
import RegistroEmpresa from './components/RegistroEmpresa.vue'

export default {
  name: 'App',
  components: {
    RegistroEmpleador,
    RegistroEmpresa,
  },
  data() {
    return {
      currentStep: 0,
      steps: [
        { title: 'Registro Empleador', component: 'RegistroEmpleador' },
        { title: 'Registro Empresa', component: 'RegistroEmpresa' },
      ],
    }
  },
  methods: {
    goToNextStep() {
      if (this.currentStep < this.steps.length - 1) {
        this.currentStep++
      }
    },
    goToPreviousStep() {
      if (this.currentStep > 0) {
        this.currentStep--
      }
    }
  }
}
</script>

<style>
#app {
  font-family: 'Inter', sans-serif;
  text-align: center;
  margin-top: 60px;
}
</style>
