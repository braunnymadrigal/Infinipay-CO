import { createApp } from 'vue'
import App from './App.vue'

import {createRouter, createWebHistory} from 'vue-router'
import PantallaPrincipal from './components/PantallaPrincipal.vue'
import SobreNosotros from './components/SobreNosotros.vue'
import RegistroEmpleador from './components/RegistroEmpleador.vue'
import RegistroEmpresa from './components/RegistroEmpresa.vue'
import LoginUser from './components/LoginUser.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {path: '/', name: "Home", component: PantallaPrincipal},
    {path: '/sobre_nosotros', name: "SobreNosotros", component: SobreNosotros},
    {path: '/registrar_empleador', name: "RegistroEmpleador", component: RegistroEmpleador},
    {path: '/registrar_empresa', name: "RegistroEmpresa", component: RegistroEmpresa},
    {path: '/login_user', name: "LoginUser", component: LoginUser},
  ]
});

createApp(App).use(router).mount('#app')
