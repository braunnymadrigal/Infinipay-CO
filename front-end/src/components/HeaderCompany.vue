<template>
  <div class="container my-5">
    <div class="text-center mb-4">
      <img src="../assets/images/logo.png"
           alt="Company logo"
           class="img-fluid"
           style="max-width: 350px" />
    </div>

    <header class="mb-5 custom-header">
      <nav class="navbar navbar-expand-lg rounded custom-navbar">
        <div class="container-fluid">
          <div class="d-flex">
          
            <router-link to="/MyProfile"
                         class="mx-2"
                         style="color: #405d72">Perfil</router-link>
                         
            <router-link to="/MyCompany"
                         class="mx-2"
                         style="color: #405d72">Empresa</router-link>             

            <router-link 
                         to="/BenefitList"
                         class="mx-2"
                         style="color: #405d72">Beneficios Empresa
            </router-link>

            <router-link
                         to="/AssignedBenefitList"
                         class="mx-2"
                         style="color: #405d72">Beneficios Empleados

            </router-link>
            
            <router-link to="/EmployeesList" class="mx-2"
                style="color: #405d72">Empleados</router-link>
            
            <router-link to="/CompanyList" class="mx-2"
              style="color: #405d72;">Empresas
            </router-link>

            <router-link to="/GeneratePayroll" class="mx-2"
              style="color: #405d72;">Generar Planilla
            </router-link>

            <router-link to="/ShowPayrollResults" class="mx-2"
              style="color: #405d72;">Resultados Planilla
            </router-link>

            <a @click="logout" class="mx-2" style="color: #405d72; cursor: pointer;">
              Salir
            </a>

            </div>
          </div>
        </nav>
      </header>
    </div>
</template>

<script>
  export default {
    name: 'CompanyHeader',
 
    methods: {
      logout() {
        this.$cookies.set('jwt', "");
        this.$router.push('/');
      }
    }
  }
</script>

<style>
  @import '../assets/css/HeaderFooter.css';
</style>