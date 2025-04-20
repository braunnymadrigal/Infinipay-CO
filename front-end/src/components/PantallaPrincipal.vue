<template>
  <div>
    <!-- Logo de la empresa -->
    <div class="logo_empresa">
      <img src="../assets/images/logo.png" alt="Logo de la empresa" class="logo_img"/>
    </div>
    <!-- Header -->
    <header class="header">
      <div class="navbar">
        <nav class="navbar-links">
          <!-- Contenedor de los botones de la izquierda -->
          <div class="navbar-links-left">
            <a href="#" class="botones_izquierda">Iniciar sesión</a>
            <router-link to="/registrar_empleador" class="botones_izquierda">Registrá tu empresa</router-link>
          </div>
          <!-- Contenedor de los botones de la derecha -->
          <div class="navbar-links-right">
            <router-link to="/sobre_nosotros" class="botones_derecha">Sobre nosotros</router-link>
            <a href="#" class="botones_derecha">¿Necesitás ayuda?</a>
          </div>
        </nav>
      </div>
    </header>
  </div>

  <!-- Imagen de fondo -->
  <div class="contenedor_imagen">
    <img src="../assets/images/PantallaPrincipal/imagen1.jpg"
      alt="Imagen" class="imagen_principal" />
    <div class="filtro_oscuro"></div>
      <!-- Textos sobre imagen -->
      <div class="contenedor_textos_sobre_imagen">
        <div class="titulo_principal">Simplificá la gestión de planillas de tus trabajadores</div>
      </div>
  </div>

  <!-- Texto -->
  <h1 class ="titulo">¿Por qué elegirnos?</h1>
  <div class="contenedor_texto">
    <div class="contenedor_columnas_texto">
      <div class="cajas_texto subtitulo">
        Accesibilidad<br>
        <span class="texto">Brindamos una alternativa accesible a pequeñas empresas que no pueden pagar servicios contables</span>
      </div>
      <div class="cajas_texto subtitulo">
        Automatización<br>
        <span class="texto">Evitamos errores humanos en el cálculo de salarios y deducciones</span>
      </div>
      <div class="cajas_texto subtitulo">
        Cumplimiento legal<br>
        <span class="texto">Nos aseguramos de que los empleadores cumplan con todas las normativas laborales y fiscales</span>
      </div>
      <div class="cajas_texto subtitulo">
        Optimización<br>
        <span class="texto">Reducimos la carga administrativa para que los emprendedores se enfoquen en hacer crecer su negocio</span>
      </div>
    </div>

    <div class="contenedor_imagen_derecha">
        <img src="../assets/images/PantallaPrincipal/imagen2.jpg"
          alt="Imagen" class="imagen_derecha" />
      </div>
  </div>

  <!-- Reseñas -->
  <h1 class ="titulo">Reseñas</h1>
  <div class="contenedor_review">
    <div class="review">
      <img src="../assets/images/PantallaPrincipal/Reviews/kirby_review.png" 
        alt="Persona 1" class="imagen_redonda" />
      <p class="texto_review">“¡Infinipay CO hace que pagar a los Waddle Dees sea pan comido!”</p>
      <p class="nombre_persona">Kirby</p>
      <p class="empresa_persona">Dream Land CO.</p>
    </div>

    <div class="review">
      <img src="../assets/images/PantallaPrincipal/Reviews/sylveon_review.jpg"
        alt="Persona 2" class="imagen_redonda" />
      <p class="texto_review">“Gestionar pagos nunca fue tan encantador”</p>
      <p class="nombre_persona">Sylveon</p>
      <p class="empresa_persona">Encantos Sylveon</p>
    </div>

    <div class="review">
      <img src="../assets/images/PantallaPrincipal/Reviews/fallguys_review.jpg"
        alt="Persona 3" class="imagen_redonda" />
      <p class="texto_review">“¡Nada de tropezar con cálculos de planilla!”</p>
      <p class="nombre_persona">Fall Guys</p>
      <p class="empresa_persona">Fall Corp. Eventos Extremos</p>
    </div>
  </div>

  <!-- Footer -->
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
</script>

<style scoped>
@import '../assets/css/PantallaPrincipal.css';
@import '../assets/css/HeaderFooter.css';
</style>