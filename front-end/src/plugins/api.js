import axios from "axios";

export default {
  install(app) {
    const apiBaseURL = "https://localhost:7275/api";

    app.config.globalProperties.$apiBaseURL = apiBaseURL;

    function authHeader() {
      const token = app.config.globalProperties.$cookies.get("jwt");

      if (!token) {
        throw new Error("Authorization token missing.");
      }

      return {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      };
    }

    app.config.globalProperties.$api = {
      registerEmployer(employerData) {
        return axios.post(`${apiBaseURL}/Employer`, employerData);
      },

      registerEmployee(employeeData) {
        return axios.post(`${apiBaseURL}/Employee`, employeeData, authHeader());
      },

      registerCompany(companyData) {
        return axios.post(`${apiBaseURL}/Company`, companyData);
      },

      getProfile() {
        return axios.get(`${apiBaseURL}/Profile`, authHeader());
      },

      getCompany() {
        return axios.get(`${apiBaseURL}/MyCompany`, authHeader());
      },

      login(userCredential) {
        return axios.post(`${apiBaseURL}/Login`, {
          NicknameOrEmail: userCredential.userId,
          Password: userCredential.userPassword,
        });
      },

      getEmployees() {
        return axios.get(`${apiBaseURL}/EmployeeList`, authHeader());
      },

      getCompanyList() {
        return axios.get(`${apiBaseURL}/Company`);
      },

      getAssignedBenefits() {
        return axios.get(`${apiBaseURL}/AssignedBenefitList`, authHeader());
      },

      assignBenefit(benefitId) {
        return axios.post(
          `${apiBaseURL}/AssignedBenefitList`,
          {
            benefitId: benefitId,
          },
          authHeader()
        );
      },

      getEmployeeById(employeeId) {
        return axios.get(`${apiBaseURL}/Employee/${employeeId}`, authHeader());
      },

      updateEmployee(employeeData, employeeId) {
        return axios.put(
          `${apiBaseURL}/Employee/${employeeId}`,
          employeeData,
          authHeader()
        );
      },
    };
  },
};
