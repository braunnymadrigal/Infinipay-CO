import { createApp } from 'vue'
import App from './App.vue'

import {createRouter, createWebHistory} from 'vue-router'
import PaginaPrincipal from './components/PaginaPrincipal.vue'
import RegistroEmpleador from './components/RegistroEmpleador.vue'
import RegistroEmpresa from './components/RegistroEmpresa.vue'  

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {path: '/', name: "Home", component: PaginaPrincipal},
    {path: '/registrar_empleador', name: "RegistroEmpleador", component: RegistroEmpleador},
    {path: '/registrar_empresa', name: "RegistroEmpresa", component: RegistroEmpresa},
  ]
});

createApp(App).use(router).mount('#app')
