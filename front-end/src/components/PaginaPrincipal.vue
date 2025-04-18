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
            <a href="#" class="botones_izquierda">Personal</a>
            <a href="#" class="botones_izquierda">Empresa</a>
          </div>
          <!-- Contenedor de los botones de la derecha -->
          <div class="navbar-links-right">
            <a href="#" class="botones_derecha">Sobre nosotros</a>
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
          <div class="texto-sobre-imagen titulo_principal">Sobre Nosotros</div>
          <div class="texto-sobre-imagen subtitulo_principal">
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
      <div class="contenedor_cajas" style="gap: 30px;">
        <div class="cajas_texto texto">Daniel Gómez Solís
            <br><span class="texto" style="margin-right: 150px; color: #758694;">Scrum Master</span>
        </div>
        <div class="cajas_texto texto">María Fernanda Herrera Gutiérrez</div>
      </div>

      <div class="contenedor_cajas" style="gap: 30px;">
        <div class="cajas_texto texto">Braunny Madrigal Barrantes</div>
        <div class="cajas_texto texto">Fabián Mora Arias</div>
      </div>
    </div>
  </div>
</template>

<style scoped>
@import '../assets/css/PaginaPrincipal.css';
</style>