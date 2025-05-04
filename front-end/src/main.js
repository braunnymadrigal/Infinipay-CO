import { createApp } from "vue";
import App from "./App.vue";
import "primeicons/primeicons.css";

import { createRouter, createWebHistory } from "vue-router";

import HomePage from "./components/HomePage.vue";
import AboutUs from "./components/AboutUs.vue";
import RegisterEmployer from "./components/RegisterEmployer.vue";
import RegisterCompany from "./components/RegisterCompany.vue";
import EmployerProfile from "./components/EmployerProfile.vue";
import RegisterEmployee from "./components/RegisterEmployee.vue";
import LoginUser from "./components/LoginUser.vue";
import RegisterBenefit from "./components/RegisterBenefit.vue";
import BenefitList from "./components/BenefitList.vue";
import MyProfile from "./components/MyProfile.vue";
import AssignedBenefitList from "./components/AssignedBenefitList.vue";

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/", name: "HomePage", component: HomePage },
    { path: "/AboutUs", name: "AboutUs", component: AboutUs },
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
      path: "/EmployerProfile",
      name: "EmployerProfile",
      component: EmployerProfile,
    },
    {
      path: "/RegisterEmployee",
      name: "RegisterEmployee",
      component: RegisterEmployee,
    },
    { path: "/LoginUser", name: "LoginUser", component: LoginUser },
    {
      path: "/RegisterBenefit",
      name: "RegisterBenefit",
      component: RegisterBenefit,
    },
    { path: "/BenefitList", name: "BenefitList", component: BenefitList },
    { path: "/MyProfile", name: "MyProfile", component: MyProfile },
    {
      path: "/AssignedBenefitList",
      name: "AssignedBenefitList",
      component: AssignedBenefitList
    },
  ],
});

createApp(App).use(router).mount("#app");
