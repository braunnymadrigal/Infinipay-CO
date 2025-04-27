<template>
  <!-- HEADER -->
  <div>
    <div class="logo_empresa">
      <img src="../assets/images/logo.png" alt="Logo de la empresa"
        class="logo_img"/>
    </div>
    <header class="header">
      <div class="navbar">
        <nav class="navbar-links">
          <div class="navbar-links-left">
            <a href="/login_user" class="botones_izquierda">Iniciar sesión</a>
            <router-link to="/registrar_empleador" class="botones_izquierda">Registrá tu empresa</router-link>
          </div>
          <div class="navbar-links-right">
            <router-link to="/" class="botones_derecha">Página principal</router-link>
          </div>
        </nav>
      </div>
    </header>
  </div>

  <!-- CONTENT -->
  <div> Hola. </div>
</template>

<script>
  export default {
    
  }
</script>

<style lang="scss" scoped>

</style>
