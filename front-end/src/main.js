import { createApp } from 'vue'
import App from './App.vue'
import 'primeicons/primeicons.css';

import {createRouter, createWebHistory} from 'vue-router'
import HomePage from './components/HomePage.vue'
import AboutUs from './components/AboutUs.vue'
import RegisterEmployer from './components/RegisterEmployer.vue'
import RegisterCompany from './components/RegisterCompany.vue'
import EmployerProfile from './components/EmployerProfile.vue'
import RegisterEmployee from './components/RegisterEmployee.vue'
import EmployeesList from './components/EmployeesList.vue';

const router = createRouter({
  history: createWebHistory(),
  routes: [
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
    {path: '/EmployeesList', name: "EmployeesList"
      , component: EmployeesList},
  ]
});

createApp(App).use(router).mount('#app') 
