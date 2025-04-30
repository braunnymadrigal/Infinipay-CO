<template>
  <!-- HEADER -->
  <div>
    <div class="CompanyLogo">
      <img
        src="../assets/images/logo.png"
        alt="Company logo"
        class="LogoImg"
      />
    </div>
    <header class="MainHeader">
      <div class="MainHeaderNavigation">
        <nav class="MainHeaderNavigationLinks">
          <div class="NavigationSectionLeft">
            <router-link to="/LoginUser" class="LeftButton">
              Iniciar sesión</router-link>
            <router-link to="/RegisterEmployer" class="LeftButton">
              Registrá tu empresa
            </router-link>
          </div>
          <div class="NavigationSectionRight">
            <router-link to="/" class="RightButton">
              Página principal</router-link>
          </div>
        </nav>
      </div>
    </header>
  </div>

  <!-- CONTENT -->
  <div class="h-100 d-flex align-items-center justify-content-center 
  bg-transparent">
    <div class="card p-4 bg-transparent border-0" 
    style="max-width: 600px; width: 100%">
      <i class="pi pi-user" style="font-size: 8.5rem"></i>
    </div>
  </div>
  <div class="h-100 d-flex align-items-center justify-content-center 
  bg-transparent">
    <div class="card p-4 bg-transparent border-0" 
    style="max-width: 600px; width: 100%">
      <h1 class="font-weight-bold">Iniciar sesión</h1>
      <h6 class="text-secondary">¿Aún no has registrado tu empresa? 
        ¡Empezá ya!</h6>
    </div>
  </div>
  <div class="h-100 d-flex align-items-center justify-content-center 
  bg-transparent">
    <div class="card p-4 bg-transparent border-0" 
    style="max-width: 600px; width: 100%">
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
            title="Ingrese un correo electrónico o nombre de usuario valido 
            (No se permiten espacios al inicio ni al final). El nombre de 
            usuario sólo acepta letras y los caracteres especiales '.' o '_'. "
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
            title="La contraseña debe superar los 10 caracteres y no sobrepasar 
            los 100. La contraseña debe contener al menos una mayúscula, una 
            minúscula y un número. La contraseña debe contener al menos un 
            carácter especial de la siguiente lista: '@', '#', '$', '%', '&', 
            '*', '!', '?'."
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
  <div class="h-100 d-flex align-items-center justify-content-center 
  bg-transparent">
    <div class="card p-4 bg-transparent border-0" 
    style="max-width: 600px; width: 100%">
      <a href="#" class="text-start">¿Has olvidado tu contraseña?</a> 
    </div>
  </div>

  <!-- FOOTER -->
  <footer class="MainFooter">
    <div>
      <p class="MainFooterText">Contacto</p>
      <p class="MainFooterText">Infinipay CO.</p>
      <div class="SocialMediaContainer">
        <a href="#" class="fa fa-facebook"></a>
        <a href="#" class="fa fa-linkedin"></a>
        <a href="#" class="fa fa-youtube"></a>
        <a href="#" class="fa fa-instagram"></a>
      </div>
    </div>
    <div>
      <p class="MainFooterSubtitule">Empresa y equipo</p>
      <div class="FooterNavLinks">
        <a href="#">Sobre nosotros</a>
      </div>
    </div>
    <div >
      <p class="MainFooterSubtitule">Recursos</p>
      <div class="FooterNavLinks">
        <a href="#">¿Cómo registro mi empresa?</a>
        <a href="#">¿Cómo registro empleados a mi empresa?</a>
        <a href="#">¿Cómo accedo a mi perfil?</a>
      </div>
    </div>
    <div>
      <p class="MainFooterSubtitule">Contacto</p>
      <div class="FooterNavLinks">
        <a href="#"> <i class="pi pi-phone" style="color:#405D72;"></i>
            +506 2000-0000</a>
        <a href="#"> <i class="pi pi-home" style="color:#405D72;"></i>
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