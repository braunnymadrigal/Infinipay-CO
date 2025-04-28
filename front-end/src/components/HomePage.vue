<template>
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
            <router-link to="/AboutUs" class="RightButton">
              Sobre nosotros</router-link>
            <router-link to="/" class="RightButton">
              ¿Necesitás ayuda?</router-link>
          </div>
        </nav>
      </div>
    </header>
  </div>

  <div class="ImageContainer">
    <img src="../assets/images/HomePage/image1.jpg"
      alt="Image" class="MainImage" />
    <div class="DarkFilter"></div>
      <div class="TextContainerOnImage">
        <div class="MainTitle">Simplificá la gestión de planillas de tus trabajadores</div>
      </div>
  </div>

  <h1 class ="Title">¿Por qué elegirnos?</h1>
  <div class="TextContainer">
    <div class="TextContainerColumns">
      <div class="TextBox Subtitle">
        Accesibilidad<br>
        <span class="Text">Brindamos una alternativa accesible a pequeñas empresas que no pueden pagar servicios contables</span>
      </div>
      <div class="TextBox Subtitle">
        Automatización<br>
        <span class="Text">Evitamos errores humanos en el cálculo de salarios y deducciones</span>
      </div>
      <div class="TextBox Subtitle">
        Cumplimiento legal<br>
        <span class="Text">Nos aseguramos de que los empleadores cumplan con todas las normativas laborales y fiscales</span>
      </div>
      <div class="TextBox Subtitle">
        Optimización<br>
        <span class="Text">Reducimos la carga administrativa para que los emprendedores se enfoquen en hacer crecer su negocio</span>
      </div>
    </div>

    <div class="SecondImageContainer">
        <img src="../assets/images/HomePage/image2.webp"
          alt="Image" class="SecondRightImage" />
      </div>
  </div>

  <h1 class ="Title">Reseñas</h1>
  <div class="ReviewContainer">
    <div class="Review">
      <img src="../assets/images/HomePage/Reviews/kirby_review.png" 
        alt="Persona 1" class="RoundImage" />
      <p class="ReviewText">“¡Infinipay CO hace que pagar a los Waddle Dees sea pan comido!”</p>
      <p class="ReviewPersonName">Kirby</p>
      <p class="ReviewCompanyName">Dream Land CO.</p>
    </div>

    <div class="Review">
      <img src="../assets/images/HomePage/Reviews/sylveon_review.jpg"
        alt="Persona 2" class="RoundImage" />
      <p class="ReviewText">“Gestionar pagos nunca fue tan encantador”</p>
      <p class="ReviewPersonName">Sylveon</p>
      <p class="ReviewCompanyName">Encantos Sylveon</p>
    </div>

    <div class="Review">
      <img src="../assets/images/HomePage/Reviews/fallguys_review.jpg"
        alt="Persona 3" class="RoundImage" />
      <p class="ReviewText">“¡Nada de tropezar con cálculos de planilla!”</p>
      <p class="ReviewPersonName">Fall Guys</p>
      <p class="ReviewCompanyName">Fall Corp. Eventos Extremos</p>
    </div>
  </div>

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
</script>

<style scoped>
  @import '../assets/css/HomePage.css';
  @import '../assets/css/HeaderFooter.css';
</style>