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
        return axios.get(`${apiBaseURL}/EmployeeBenefit`, authHeader());
      },

      assignBenefit(benefitId) {
        return axios.post(
          `${apiBaseURL}/EmployeeBenefit`,
          {
            id: benefitId,
          },
          authHeader()
        );
      },

      generateEmployeePayroll(payload = {}) {
        return axios.post(
          `${apiBaseURL}/PayrollOrchestrator`,
          payload,
          authHeader()
        );
      },

      showEmployeesPayroll() {
        return axios.get(`${apiBaseURL}/PayrollOrchestrator`, authHeader());
      },

      getEmployeeById(employeeId) {
        return axios.get(`${apiBaseURL}/Employee/${employeeId}`, authHeader());
      },

      benefitAPI(benefit) {
        return axios.get(benefit.urlAPI, {
          params: {
            param1: benefit.paramOneAPI,
            param2: benefit.paramTwoAPI,
            param3: benefit.paramThreeAPI,
          },
        });
      },

      getCompanyBenefits() {
        return axios.get(`${apiBaseURL}/CompanyBenefit`, authHeader());
      },

      createCompanyBenefit(benefitData) {
        return axios.post(
          `${apiBaseURL}/CompanyBenefit`,
          benefitData,
          authHeader()
        );
      },

      updateEmployee(employeeData, employeeId) {
        return axios.put(
          `${apiBaseURL}/Employee/${employeeId}`,
          employeeData,
          authHeader()
        );
      },

      benefitAPI(benefit) {
        return axios.get(benefit.urlAPI, {
          params: {
            param1: benefit.paramOneAPI,
            param2: benefit.paramTwoAPI,
            param3: benefit.paramThreeAPI,
          },
        });
      },

      getCompanyBenefits() {
        return axios.get(`${apiBaseURL}/CompanyBenefit`, authHeader());
      },

      createCompanyBenefit(benefitData) {
        return axios.post(
          `${apiBaseURL}/CompanyBenefit`,
          benefitData,
          authHeader()
        );
      },

      getCompanyBenefitById(benefitId) {
        return axios.get(
          `${apiBaseURL}/CompanyBenefit/${benefitId}`,
          authHeader()
        );
      },

      updateCompanyBenefit(benefitData, benefitId) {
        return axios.put(
          `${apiBaseURL}/CompanyBenefit/${benefitId}`,
          benefitData,
          authHeader()
        );
      },

      getEmployeeHoursContract() {
        return axios.get(`${apiBaseURL}/EmployeeHours`, authHeader());
      },

      getEmployeeHours(startDate, endDate) {
        return axios.get(`${apiBaseURL}/EmployeeHours/Hours`, {
          params: {
            startDate,
            endDate,
          },
          ...authHeader(),
        });
      },

      registerEmployeeHours(employeeHoursWorked) {
        return axios.post(
          `${apiBaseURL}/EmployeeHours`,
          employeeHoursWorked,
          authHeader()
        );
      },

      getCompanyBenefitById(benefitId) {
        return axios.get(
          `${apiBaseURL}/CompanyBenefit/${benefitId}`,
          authHeader()
        );
      },

      updateCompanyBenefit(benefitData, benefitId) {
        return axios.put(
          `${apiBaseURL}/CompanyBenefit/${benefitId}`,
          benefitData,
          authHeader()
        );
      },
    };
  },
};
