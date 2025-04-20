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
            <router-link to="/" class="botones_derecha">Página principal</router-link>
            <a href="#" class="botones_derecha">¿Necesitás ayuda?</a>
          </div>
        </nav>
      </div>
    </header>

      <!-- Imagen de fondo -->
      <div class="contenedor_imagen">
        <img src="../assets/images/pagina_principal.jpeg" alt="Fondo de la página" class="imagen_principal" />
        <div class="filtro_oscuro"></div>

        <!-- Textos sobre imagen -->
        <div class="contenedor_textos_sobre_imagen">
          <div class="titulo_principal">Sobre Nosotros</div>
          <div class="subtitulo_principal">
            Simplificamos el proceso, vos solo tenés que enfocarte en hacer crecer tu empresa.
          </div>
        </div>
      </div>

    <!-- Más textos -->
    <div>
      <p class="texto_principal">
        Infinipay CO está dirigido a emprendedores y pymes en Costa Rica para automatizar la gestión de planillas, asegurando el cálculo preciso de salarios, deducciones y beneficios según la legislación vigente.
      </p>
      <h1 class ="titulo">¿Por qué elegir Infinipay CO?</h1>

      <div class="contenedor_cajas">
        <div class="cajas_texto subtitulo">Solución automatizada para Costa Rica 
            <br><span class="texto">Plataforma web que simplifica la gestión de planillas, asegurando el cálculo correcto de salarios, deducciones y beneficios según la legislación costarricense.</span>
        </div>
        <div class="cajas_texto subtitulo">Cumplimiento legal garantizado
          <br><span class="texto">Aplicamos las deducciones obligatorias de ley como el CCSS, impuesto sobre la renta y asociaciones solidaristas, con cálculos basados en fuentes oficiales.</span>
        </div>
      </div>

      <div class="contenedor_cajas">
        <div class="cajas_texto subtitulo">Optimización para emprendedores y pymes
          <br><span class="texto">Diseñada para dueños de negocios que necesitan gestionar pagos sin depender de terceros, reduciendo errores y ahorrando tiempo.</span>
        </div>
        <div class="cajas_texto subtitulo">Reportes claros y completos
          <br><span class="texto">Generamos informes detallados sobre salarios brutos, deducciones y pagos, brindando control total sobre la planilla y las obligaciones fiscales.</span>
        </div>
      </div>
    </div>

    <!-- Mas texto -->
    <div>
      <h1 class ="titulo">Nuestro equipo</h1>
      <div class="contenedor_cajas">
        <div class="cajas_texto texto" style="margin-left: 100px;">Daniel Gómez Solís
            <br><span class="texto" style="margin-right: 150px; color: #758694;">Scrum Master</span>
        </div>
        <div class="cajas_texto texto">María Fernanda Herrera Gutiérrez</div>
      </div>

      <div class="contenedor_cajas">
        <div class="cajas_texto texto" style="margin-left: 100px;">Braunny Madrigal Barrantes</div>
        <div class="cajas_texto texto">Fabián Mora Arias</div>
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
          <a href="#">¿Necesitás ayuda?</a>
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
             +506 8000-0000</a>
          <a href="#"> <i class="material-icons" style="color:#405D72;">phone</i>
             +506 2000-0000</a>
          <a href="#"> <i class="material-icons" style="color:#405D72;">home</i>
             San José, Montes de Oca, San Pedro</a>
        </div>
      </div>
    </footer>
  </div>
</template>

<style scoped>
@import '../assets/css/SobreNosotros.css';
</style>