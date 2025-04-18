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
          <div class="texto-sobre-imagen titulo">Sobre Nosotros</div>
          <div class="texto-sobre-imagen subtitulo">
            Simplificamos el proceso, vos solo tenés que enfocarte en hacer crecer tu empresa.
          </div>
        </div>
      </div>

    <!-- Más textos -->
    <div>
      <p style=" 
        font-size: 25px;
        text-align: justify;
        margin-top: 30px;
        margin-left: auto;
        margin-right: auto;
        max-width: 900px;
      ">
        Infinipay CO está dirigido a emprendedores y pymes en Costa Rica para automatizar la gestión de planillas, asegurando el cálculo preciso de salarios, deducciones y beneficios según la legislación vigente.
      </p>
      <h1 style="
        font-size: 35px;
        text-align: justify;
        margin-top: 50px;
        margin-left: 200px;
        color: #405D72;
        ">¿Por qué elegir Infinipay CO?</h1>
    </div>
  </div>
</template>

<style scoped>
@import '../assets/css/PaginaPrincipal.css';
</style>