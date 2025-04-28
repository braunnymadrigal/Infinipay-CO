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
            <router-link to="/" class="RightButton">
              Página Principal</router-link>
            <router-link to="/" class="RightButton">
              ¿Necesitás ayuda?</router-link>
          </div>
        </nav>
      </div>
    </header>
  </div>

  <div class="ImageContainer">
    <img src="../assets/images/AboutUs/image1.jpeg"
      alt="Image" class="MainImage" />
    <div class="DarkFilter"></div>
      <div class="TextContainerOnImage">
        <div class="MainTitle">Sobre Nosotros</div>
          <div class="MainSubtitle">
            Simplificamos el proceso, vos solo tenés que enfocarte en hacer crecer tu empresa.
          </div>
      </div>
  </div>

  <div>
    <p class="MainText">
      Infinipay CO está dirigido a emprendedores y pymes en Costa Rica para automatizar la gestión de planillas, asegurando el cálculo preciso de salarios, deducciones y beneficios según la legislación vigente.
    </p>
    <h1 class ="Title">¿Por qué elegir Infinipay CO?</h1>

    <div class="TextContainer">
      <div class="TextBoxAboutUs Subtitle">Solución automatizada para Costa Rica 
          <br><span class="Text">Plataforma web que simplifica la gestión de planillas, asegurando el cálculo correcto de salarios, deducciones y beneficios según la legislación costarricense.</span>
      </div>
      <div class="TextBoxAboutUs Subtitle">Cumplimiento legal garantizado
        <br><span class="Text">Aplicamos las deducciones obligatorias de ley como el CCSS, impuesto sobre la renta y asociaciones solidaristas, con cálculos basados en fuentes oficiales.</span>
      </div>
    </div>

    <div class="TextContainer">
      <div class="TextBoxAboutUs Subtitle">Optimización para emprendedores y pymes
        <br><span class="Text">Diseñada para dueños de negocios que necesitan gestionar pagos sin depender de terceros, reduciendo errores y ahorrando tiempo.</span>
      </div>
      <div class="TextBoxAboutUs Subtitle">Reportes claros y completos
        <br><span class="Text">Generamos informes detallados sobre salarios brutos, deducciones y pagos, brindando control total sobre la planilla y las obligaciones fiscales.</span>
      </div>
    </div>
  </div>

  <h1 class ="Title">Nuestro equipo</h1>
  <div class="TeamMembersContainer">
    <div class="TeamMembers">
      <img src="../assets/images/HomePage/Reviews/kirby_review.png" 
        alt="Persona 1" class="RoundImage" />
      <p class="ReviewPersonName">Daniel Gómez Solís</p>
    </div>

    <div class="TeamMembers">
      <img src="../assets/images/AboutUs/TeamMembers/mafer.jpg"
      alt="Persona 2" class="RoundImage" />
      <p class="ReviewPersonName">María Fernanda Herrera Gutiérrez</p>
    </div>

    <div class="TeamMembers">
      <img src="../assets/images/HomePage/Reviews/fallguys_review.jpg"
      alt="Persona 3" class="RoundImage" />
      <p class="ReviewPersonName">Braunny Madrigal Barrantes</p>
    </div>

    <div class="TeamMembers">
      <img src="../assets/images/HomePage/Reviews/fallguys_review.jpg"
      alt="Persona 4" class="RoundImage" />
      <p class="ReviewPersonName">Fabián Mora Arias</p>
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

<style scoped>
  @import '../assets/css/AboutUs.css';
  @import '../assets/css/HomePage.css';
  @import '../assets/css/HeaderFooter.css';
</style>