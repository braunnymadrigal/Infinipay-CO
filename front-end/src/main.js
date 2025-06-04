import { createApp } from "vue";
import ApiPlugin from "./plugins/api";
import App from "./App.vue";
import "primeicons/primeicons.css";

import { createRouter, createWebHistory } from "vue-router";
import VueCookies from "vue-cookies";

import HomePage from "./components/HomePage.vue";
import AboutUs from "./components/AboutUs.vue";
import RegisterEmployer from "./components/RegisterEmployer.vue";
import RegisterCompany from "./components/RegisterCompany.vue";
import RegisterEmployee from "./components/RegisterEmployee.vue";
import LoginUser from "./components/LoginUser.vue";
import RegisterBenefit from "./components/RegisterBenefit.vue";
import BenefitList from "./components/BenefitList.vue";
import MyCompany from "./components/MyCompany.vue";
import MyProfile from "./components/MyProfile.vue";
import AssignedBenefitList from "./components/AssignedBenefitList.vue";
import EmployeesList from "./components/EmployeesList.vue";
import CompanyList from "./components/CompanyList.vue";
import BenefitDetails from "./components/BenefitDetails.vue";
import GeneratePayroll from "./components/GeneratePayroll.vue";
import ShowPayrollResults from "./components/ShowPayrollResults.vue";
import EmployeeTimesheet from "./components/EmployeeTimesheet.vue";
import UpdateEmployeeForm from "./components/UpdateEmployeeForm.vue";
import UpdateBenefitForm from "./components/UpdateBenefitForm.vue";

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
    {
      path: "/BenefitDetails/:benefitId",
      name: "BenefitDetails",
      component: BenefitDetails,
    },
    { path: "/MyCompany", name: "MyCompany", component: MyCompany },
    { path: "/MyProfile", name: "MyProfile", component: MyProfile },
    {
      path: "/AssignedBenefitList",
      name: "AssignedBenefitList",
      component: AssignedBenefitList,
    },
    { path: "/EmployeesList", name: "EmployeesList", component: EmployeesList },
    { path: "/CompanyList", name: "CompanyList", component: CompanyList },
    
    { path: "/GeneratePayroll", name: "GeneratePayroll"
      , component: GeneratePayroll },
    
    { path: "/ShowPayrollResults", name: "ShowPayrollResults"
      , component: ShowPayrollResults },
    {
      path: "/EmployeeTimesheet",
      name: "EmployeeTimesheet",
      component: EmployeeTimesheet
    },
    {
      path: "/EmployeeUpdate/:id",
      name: "EmployeeUpdate",
      component: UpdateEmployeeForm,
    },
    {
      path: "/BenefitUpdate/:id",
      name: "BenefitUpdate",
      component: UpdateBenefitForm,
    },
  ],
});

const app = createApp(App);
app.use(router);
app.use(VueCookies, { expires: "7d" });
app.use(ApiPlugin);
app.mount("#app");
