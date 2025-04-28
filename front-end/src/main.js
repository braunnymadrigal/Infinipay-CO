import { createApp } from "vue";
import App from "./App.vue";

import { createRouter, createWebHistory } from "vue-router";
import PantallaPrincipal from "./components/PantallaPrincipal.vue";
import SobreNosotros from "./components/SobreNosotros.vue";
import RegistroEmpleador from "./components/RegistroEmpleador.vue";
import RegistroEmpresa from "./components/RegistroEmpresa.vue";
import RegistroBeneficio from "./components/RegistroBeneficio.vue";
import ListaBeneficios from "./components/ListaBeneficios.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", name: "Home", component: PantallaPrincipal },
    {
      path: "/sobre_nosotros",
      name: "SobreNosotros",
      component: SobreNosotros,
    },
    {
      path: "/registrar_empleador",
      name: "RegistroEmpleador",
      component: RegistroEmpleador,
    },
    {
      path: "/registrar_empresa",
      name: "RegistroEmpresa",
      component: RegistroEmpresa,
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
  ],
});

createApp(App).use(router).mount("#app");
