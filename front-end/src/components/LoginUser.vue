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
            <router-link to="/login_user" class="botones_izquierda">Iniciar sesión</router-link>
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
  <div class="h-100 d-flex align-items-center justify-content-center bg-transparent">
    <div class="card p-4 bg-transparent border-0" style="max-width: 600px; width: 100%">
      <span class="material-symbols-outlined" style="font-size:200px">person</span>
    </div>
  </div>
  <div class="h-100 d-flex align-items-center justify-content-center bg-transparent">
    <div class="card p-4 bg-transparent border-0" style="max-width: 600px; width: 100%">
      <h1 class="font-weight-bold">Iniciar sesión</h1>
      <h6 class="text-secondary">¿Aún no has registrado tu empresa? ¡Empezá ya!</h6>
    </div>
  </div>
  <div>
    <div>
      <form @submit.prevent="startLogin" class="form">
        <div class="form-group text-start">
          <label for="userId">Correo electrónico o nombre de usuario</label>
          <input
            v-model="user.userId"
            type="text"
            id="userId"
            pattern="([^\s@]+@[^\s@]+\.[^\s@]+)|(^[A-Za-z._]{1,30}$)"
            minlength="1"
            maxlength="100"
            title="Ingrese un correo electrónico o nombre de usuario valido (No se permiten espacios al inicio ni al final). El nombre de usuario sólo acepta letras y los caracteres especiales '.' o '_'. "
            placeholder=""
            class="form-control bg-transparent"
            required
          />
        </div>
        <div class="form-group text-start mb-3">
          <label for="userPassword">Contraseña</label>
          <input
            v-model="user.userPassword"
            type="password"
            id="userPassword"
            pattern="^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@#$%&*!?]).+$"
            minlength="10"
            maxlength="100"
            title="La contraseña debe superar los 10 caracteres y no sobrepasar los 100. La contraseña debe contener al menos una mayúscula, una minúscula y un número. La contraseña debe contener al menos un carácter especial de la siguiente lista: '@', '#', '$', '%', '&', '*', '!', '?'."
            placeholder=""
            class="form-control bg-transparent"
            required
          />
        </div>
        <div>
          <button type="submit" class="btn btn-dark btn-round">
            Iniciar sesión
          </button>
        </div>
      </form>
    </div>
  </div>
  <div class="h-100 d-flex align-items-center justify-content-center bg-transparent">
    <div class="card p-4 bg-transparent border-0" style="max-width: 600px; width: 100%">
      <a href="#" class="text-start">¿Has olvidado tu contraseña?</a> 
    </div>
  </div>

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
