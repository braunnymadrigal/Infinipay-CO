import { createApp } from "vue";
import App from "./App.vue";
import "primeicons/primeicons.css";
import { createApp } from "vue";
import App from "./App.vue";
import "primeicons/primeicons.css";

<<<<<<< HEAD
import { createRouter, createWebHistory } from "vue-router";
import HomePage from "./components/HomePage.vue";
import AboutUs from "./components/AboutUs.vue";
import RegisterEmployer from "./components/RegisterEmployer.vue";
import RegisterCompany from "./components/RegisterCompany.vue";
import RegistroBeneficio from "./components/RegistroBeneficio.vue";
import ListaBeneficios from "./components/ListaBeneficios.vue";
import { createRouter, createWebHistory } from "vue-router";
import HomePage from "./components/HomePage.vue";
import AboutUs from "./components/AboutUs.vue";
import RegisterEmployer from "./components/RegisterEmployer.vue";
import RegisterCompany from "./components/RegisterCompany.vue";
import RegistroBeneficio from "./components/RegistroBeneficio.vue";
import ListaBeneficios from "./components/ListaBeneficios.vue";
=======
import {createRouter, createWebHistory} from 'vue-router'

import HomePage from './components/HomePage.vue'
import AboutUs from './components/AboutUs.vue'
import RegisterEmployer from './components/RegisterEmployer.vue'
import RegisterCompany from './components/RegisterCompany.vue'
import EmployerProfile from './components/EmployerProfile.vue'
import RegisterEmployee from './components/RegisterEmployee.vue'
import LoginUser from './components/LoginUser.vue'
>>>>>>> 8ff70bd4debea73798629aed034c53898528d04b

const router = createRouter({
  history: createWebHistory(),
  routes: [
<<<<<<< HEAD
    { path: "/", name: "HomePage", component: HomePage },
    {
      path: "/AboutUs",
      name: "AboutUs",
      component: AboutUs,
    },
    {
      path: "/RegisterEmployer",
      name: "RegisterEmployer",
      component: RegisterEmployer,
    },
    {
      path: "/RegisterCompany",
      name: "RegisterCompany",
      component: RegisterCompany,
    },
    {
      path: "/registrar_beneficio",
      name: "RegistroBeneficio",
      component: RegistroBeneficio,
    },
    {
      path: "/lista_beneficios",
      name: "ListaBeneficios",
      component: ListaBeneficios,
    },
    { path: "/LoginUser", name: "LoginUser", component: LoginUser },
  ],
=======
    {path: '/', name: "HomePage", component: HomePage},
    {path: '/AboutUs', name: "AboutUs", component: AboutUs},
    {path: '/RegisterEmployer', name: "RegisterEmployer"
      , component: RegisterEmployer},
    {path: '/RegisterCompany', name: "RegisterCompany"
      , component: RegisterCompany},
    {path: '/EmployerProfile', name: "EmployerProfile"
      , component: EmployerProfile},
    {path: '/RegisterEmployee', name: "RegisterEmployee"
      , component: RegisterEmployee},
    {path: '/LoginUser', name: "LoginUser", component: LoginUser},
  ]
>>>>>>> 8ff70bd4debea73798629aed034c53898528d04b
});

createApp(App).use(router).mount("#app");
