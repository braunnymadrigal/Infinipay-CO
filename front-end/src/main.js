import { createApp } from 'vue'
import App from './App.vue'
import 'primeicons/primeicons.css';

import {createRouter, createWebHistory} from 'vue-router'

import HomePage from './components/HomePage.vue'
import AboutUs from './components/AboutUs.vue'
import RegisterEmployer from './components/RegisterEmployer.vue'
import RegisterCompany from './components/RegisterCompany.vue'
import LoginUser from './components/LoginUser.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {path: '/', name: "HomePage", component: HomePage},
    {path: '/AboutUs', name: "AboutUs", component: AboutUs},
    {path: '/RegisterEmployer', name: "RegisterEmployer"
      , component: RegisterEmployer},
    {path: '/RegisterCompany', name: "RegisterCompany"
      , component: RegisterCompany},
    {path: '/LoginUser', name: "LoginUser", component: LoginUser},
  ]
});

createApp(App).use(router).mount('#app') 
