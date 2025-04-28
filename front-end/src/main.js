import { createApp } from "vue";
import App from "./App.vue";
import "primeicons/primeicons.css";
import { createApp } from "vue";
import App from "./App.vue";
import "primeicons/primeicons.css";

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

const router = createRouter({
  history: createWebHistory(),
  routes: [
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
});

createApp(App).use(router).mount("#app");
