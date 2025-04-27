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

  <!-- FOOTER -->
  <footer class="footer">
    <div>
      <p class="footer_text">Contacto</p>
      <p class="footer_text">Infinipay CO.</p>
      <div class="contenedor_redes">
        <a href="#" class="fa fa-facebook"></a>
        <a href="#" class="fa fa-linkedin"></a>
        <a href="#" class="fa fa-youtube"></a>
        <a href="#" class="fa fa-instagram"></a>
      </div>
    </div>

    <div>
      <p class="footer_subtitulo">Empresa y equipo</p>
      <div class="contenedor_enlaces">
        <a href="#">Sobre nosotros</a>
      </div>
    </div>

    <div >
      <p class="footer_subtitulo">Recursos</p>
      <div class="contenedor_enlaces">
        <a href="#">¿Cómo registro mi empresa?</a>
        <a href="#">¿Cómo registro empleados a mi empresa?</a>
        <a href="#">¿Cómo accedo a mi perfil?</a>
      </div>
    </div>

    <div>
      <p class="footer_subtitulo">Contacto</p>
      <div class="contenedor_enlaces">
        <!-- Iconos obtenidos de https://www.w3schools.com/css/css_icons.asp -->
        <a href="#"> <i class="material-icons" style="color:#405D72;">phone</i>
            +506 2000-0000</a>
        <a href="#"> <i class="material-icons" style="color:#405D72;">home</i>
            San José, Montes de Oca, San Pedro</a>
      </div>
    </div>
  </footer>
</template>

<script>
  export default {
    data() {
      return {
        user: { userId: "", userPassword: "" },
      };
    },
    methods: {
      startLogin() {
        console.log("Saved:", this.user);
      },
    },
  };
</script>

<style lang="scss" scoped>

</style>
