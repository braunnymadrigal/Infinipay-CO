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
    <h2 style="color: #758694;  margin-bottom: 50px;">Datos de la empresa</h2>
    <form @submit.prevent="submit_form">

      <div class="InputSection">
        <label class="Label" for="name">Nombre</label>
        <input class="InputBox"
          type="text" 
          v-model="name" 
          id="name"
          required 
          maxlength="30"
          pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ&.\s]+$"
          title="Sólo se permiten letras y acentos del abecedario español
            , '&', '.' y espacios" 
        />
      </div>

      <div class="InputSection">
        <label class="Label" for="description">Descripción de la empresa</label>
        <textarea class="TextArea"
          v-model="description" 
          id="description"
          maxlength="300"
          pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s]+$"
          placeholder="Sólo se permiten espacios, letras y acentos del abecedario español."
          rows="4"
          style="resize: vertical; width: 100%;"
        ></textarea>
      </div>

      <div class="InputSection">
        <label class="Label" for="idNumber">Cédula jurídica</label>
        <input class="InputBox"
          type="text" 
          v-model="idNumber" 
          id="idNumber"
          required
          pattern="^\d{10}$"
          title="Formato: X-XXX-XXXXXX"
          placeholder="10 dígitos, sin guiones"
        />
      </div>

      <div class="InputSection">
        <label class="Label" for="phoneNumbers">Teléfono</label>
        <div 
          v-for="(phoneNumber, index) in phoneNumbers"
          :key="index"
          style="display: flex; align-items: center; margin-bottom: 8px;">
          <span style="padding: 0 8px;">+506</span>
          <input class="InputBox" style="margin-top: 15px;"
            type="text" 
            v-model="phoneNumbers[index]"
            :id="'phoneNumbers-' + index"
            required
            pattern="\d{8}"
            title="Formato: XXXX-XXXX"
            placeholder="8 dígitos, sin guiones"
          />
          <button
            type="button" 
            class="DeleteButton" style="margin-left: 8px;"
            @click="deletePhoneNumber(index)">🗑️
          </button>
        </div>
        <button 
          type="button" class="AddButton"
          @click="addPhoneNumber"
          :disabled="phoneNumbers.length >= 100">Añadir otro teléfono
        </button>
      </div>

      <div class="InputSection">
        <label class="Label" for="email">Correo electrónico</label>
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

      <h3 style="text-align: left; font-weight: normal;
        margin-bottom: 20px;">Dirección </h3>

      <div class="CompanyAddressSection" 
        v-for="(addr, index) in addresses" :key="index">
        <div>
          <label class="Label" :for="'province-' + index">Provincia</label>
          <input class="InputBox"
            type="text" 
            v-model="addr.province" 
            :id="'province-' + index" 
            required 
            maxlength="10"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label class="Label" :for="'canton-' + index">Cantón</label>
          <input class="InputBox"
            type="text" 
            v-model="addr.canton" 
            :id="'canton-' + index" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label class="Label" :for="'district-' + index">Distrito</label>
          <input class="InputBox"
            type="text" 
            v-model="addr.district" 
            :id="'distric-' + index" 
            required 
            maxlength="100"
            pattern="^[a-zA-ZáéíóúÁÉÍÓÚ\s]+$"
            title="Sólo se permiten letras y acentos del abecedario español"
          />
        </div>

        <div>
          <label class="Label" :for="'otherSigns-' + index">Otras señas</label>
          <textarea class="TextArea"
            v-model="addr.otherSigns" 
            :id="'otherSigns-' + index" 
            required 
            maxlength="300"
            pattern="^[a-zA-Z0-9áéíóúÁÉÍÓÚ\s]+$"
            placeholder="Sólo se permiten letras, números y espacios en blanco"
            rows="1"
            style="resize: vertical;"
          ></textarea>
        </div>

        <button
          type="button"
          class="DeleteButton" style="margin-top: 8px;"
          @click="deleteAddress(index)">🗑️
        </button>
      </div>

      <button type="button"
        class="AddButton"
        @click="addAddress" :disabled="addresses.length >= 100">
        Añadir otra dirección
      </button>
      
      <div class="InputSection">
        <label class="Label" for="legalName">Razón social</label>
        <textarea class="TextArea"
          type="text" 
          id="legalName" 
          v-model="legalName"
          required 
          maxlength="100"
          pattern="^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s&]+$"
          placeholder="Sólo letras, acentos, espacios y '&'"
          title="Sólo se permiten letras, acentos, espacios y el símbolo '&'"
          rows="2"
        ></textarea>
      </div>
      
      <div class="InputSection">
        <label class="Label" for="benefits">Cantidad máxima de beneficios por
          empleado</label>
        <select id="benefits" class="SelectForm" v-model="benefits" required>
          <option disabled value="">Seleccione una cantidad</option>
          <option v-for="n in 100" :key="n" :value="n-1">{{ n-1 }}</option>
        </select>
      </div>

      <div class="InputSection">
        <label class="Label" for="paymentType">Tipo de pago</label>
        <select id="paymentType" class="SelectForm" v-model="paymentType"
          required>
          <option disabled value="">Seleccione un tipo de pago</option>
          <option value="weekly">Semanal</option>
          <option value="biweekly">Quincenal</option>
          <option value="monthly">Mensual</option>
        </select>
    </div>

    <div class="ButtonContainer">
      <router-link to="/RegisterEmployer" class="GoBackButton"
        @click="goBack">Volver</router-link>
      <button class="GoNextButton" type="submit">Terminar registro</button>
    </div>
    </form>
  </div>

</template>

<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router';

export default {
  setup() {
    const router = useRouter();
    const name = ref('');
    const description = ref('');
    const idNumber = ref('');
    const phoneNumbers = ref(['']);
    const email = ref('');
    const addresses = ref([
      {
        province: '',
        canton: '',
        district: '',
        otherSigns: ''
      }
    ]);
    const benefits = ref(0);
    const paymentType = ref('');

    function addPhoneNumber() {
      if (phoneNumbers.value.length < 100) {
        phoneNumbers.value.push('');
      }
    }

    function deletePhoneNumber(index) {
      if (phoneNumbers.value.length > 1) {
        phoneNumbers.value.splice(index, 1);
      }
    }

    function addAddress() {
      if (addresses.value.length < 100) {
        addresses.value.push({
          province: '',
          canton: '',
          district: '',
          otherSigns: ''
        });
      }
    }

    function deleteAddress(index) {
      if (addresses.value.length > 1) {
        addresses.value.splice(index, 1);
      }
    }

    function submitForm() {
      router.push('/');
    }

    return {
      name,
      description,
      idNumber,
      phoneNumbers,
      email,
      addresses,
      benefits,
      paymentType,
      addPhoneNumber,
      deletePhoneNumber,
      addAddress,
      deleteAddress,
      submitForm
    };
  }
};
</script>

<style scoped>
  @import '../assets/css/RegisterCompany.css';
  @import '../assets/css/RegisterEmployer.css';
</style>