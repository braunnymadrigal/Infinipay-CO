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
              Página principal</router-link>
          </div>
        </nav>
      </div>
    </header>
  </div>

  <div class="RegisterCompanyForm">
    <h1 style="color: #405D72;">Registrá tu empresa</h1>
    <h2 style="color: #758694; margin-bottom: 50px;">
      Datos del dueño de la empresa</h2>
    <form @submit.prevent="submitForm">

      <div class="InputSectionForNames">
        <div>
          <label class="Label" for="firstName">Primer nombre</label>
          <input class="InputBox"
            type="text" 
            v-model="firstName" 
            id="firstName" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label class="Label" for="secondName">Segundo nombre</label>
          <input class="InputBox"
            type="text" 
            v-model="secondName" 
            id="secondName" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>
      </div>

      <div class="InputSectionForNames">
        <div>
          <label class="Label" for="firstLastName">Primer apellido</label>
          <input class="InputBox"
            type="text" 
            v-model="firstLastName" 
            id="firstLastName" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label class="Label" for="SecondLastName">Segundo apellido</label>
          <input class="InputBox"
            type="text" 
            v-model="SecondLastName" 
            id="SecondLastName" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>
      </div>

      <div class="InputSection">
        <label class="Label" for="idNumber">Cédula</label>
        <input class="InputBox"
          type="text" 
          v-model="idNumber" 
          id="idNumber"
          required
          pattern="^\d{9}$"
          title="Formato: X-XXXX-XXXX"
          placeholder="9 dígitos, sin guiones"
        />
      </div>

      <div class="InputSection">
        <label class="Label" for="username">Nombre de usuario</label>
        <input class="InputBox"
          type="text" 
          v-model="username" 
          id="username"
          required 
          maxlength="30"
          pattern="^[a-z_\.]+$" 
          title="ejemplo_usuario" 
          placeholder="Sólo se permiten letras minúsculas, '_' y '.'"
        />
      </div>

      <div class="InputSection">
        <label class="Label" for="password">Contraseña</label>
        <input class="InputBox"
          type="password" 
          v-model="password" 
          id="password"
          required
          minlength="10"
          maxlength="100"
          pattern="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*?])[A-Za-z\d!@#$%^&*?]{10,100}$"
          title="La contraseña debe contener al menos una mayúscula, una minúscula, un número y un carácter especial"
          placeholder="Al menos 10 caracteres"
        />
      </div>

      <div class="InputSection">
        <label class="Label" for="phone">Teléfono</label>
        <div style="display: flex; align-items: center;">
          <span style="padding: 0 8px;">+506</span>
          <input class="InputBox" style="margin-top: 15px;"
            type="text" 
            v-model="phone" 
            id="phone"
            required
            pattern="\d{8}"
            title="Formato: XXXX-XXXX"
            placeholder="8 dígitos, sin guión"
          />
        </div>
      </div>

      <div class="InputSection">
        <label class="Label" for="email">Correo</label>
        <input class="InputBox"
          type="email" 
          v-model="email" 
          id="email"
          required
          maxlength="100"
          pattern="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
          placeholder="xxx@xxxx.xxx"
        />
      </div>

      <h3 style="text-align: left; font-weight: normal; margin-bottom: 20px;">
        Dirección</h3>

      <div class="AddressSection">
        <div>
          <label class="Label" for="province">Provincia</label>
          <input class="InputBox"
            type="text" 
            v-model="province" 
            id="province" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label class="Label" for="canton">Cantón</label>
          <input class="InputBox"
            type="text" 
            v-model="canton" 
            id="canton" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label class="Label" for="district">Distrito</label>
          <input class="InputBox"
            type="text" 
            v-model="district" 
            id="district" 
            required 
            maxlength="100"
            pattern="^[a-zA-Z0-9áéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label class="Label" for="otherSigns">Otras señas</label>
          <textarea class="TextArea"
            v-model="otherSigns" 
            id="otherSigns" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
            placeholder="Sólo se permiten números, letras y acentos del abecedario español"
            rows="1"
            style="resize: vertical;"
          ></textarea>
        </div>
      </div>

      <button class="NextPageButton" type="submit">Siguiente</button>
    </form>
  </div>

</template>

<script>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

export default {
  setup() {
    const router = useRouter()

    const firstName = ref('')
    const secondName = ref('')
    const firstLastName = ref('')
    const secondLastName = ref('')
    const idNumber = ref('')
    const username = ref('')
    const password = ref('')
    const phone = ref('')
    const email = ref('')
    const province = ref('')
    const canton = ref('')
    const district = ref('')
    const otherSigns = ref('')

    function submitForm() {
      router.push('/RegisterCompany')
    }

    return {
      firstName,
      secondName,
      firstLastName,
      secondLastName,
      idNumber,
      username,
      password,
      phone,
      email,
      province,
      canton,
      district,
      otherSigns,
      submitForm
    }
  }
}
</script>

<style scoped>
  @import '../assets/css/RegisterEmployer.css';
</style>