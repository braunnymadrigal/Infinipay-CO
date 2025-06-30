using System.Text.Json;
using System.Text;
using back_end.Domain;

namespace back_end.Application
{
    public class Deduction : IDeduction
    {
        private const string DEDUCTION_BY_API = "api";
        private const string DEDUCTION_BY_FIXED_AMOUNT = "montoFijo";
        private const string DEDUCTION_BY_PERCENTAGE = "porcentaje";
        private const string JSON_ENCODING = "application/json";
        private const string MALE_IN_ENGLISH = "male";
        private const string MALE_IN_SPANISH = "masculino";
        private const string FEMALE_IN_ENGLISH = "female";
        private const string FEMALE_IN_SPANISH = "femenino";

        private const string FRIENDS_API_JSON_RETURN_KEY = "monthlyCost";
        private const string GEEMS_API_JSON_RETURN_KEY = "amountToCharge";

        private const string VORLAGENERSTELLER_API_URL =
            "https://mediseguro-vorlagenersteller-d4hmbvf7frg7aqan.southcentralus-01.azurewebsites.net/api/MediSeguroMonto";
        private const string FRIENDS_API_URL =
            "https://poliza-friends-grg0h9g5crf2hwh8.southcentralus-01.azurewebsites.net/api/LifeInsurance";
        private const string GEEMS_API_URL =
            "https://asociacion-geems-c3dfavfsapguhxbp.southcentralus-01.azurewebsites.net/api/public/calculator/calculate";

        private const double PERCENTAGE_DIVISOR = 100.0;

        private const string MONTHLY_PAYMENT_TYPE = "mensual";
        private const string BIWEEKLY_PAYMENT_TYPE = "quincenal";
        private const int DEDUCTION_DIVISOR = 2;

        public async Task<List<PayrollEmployeeModel>> calculateDeductions(List<PayrollEmployeeModel> 
            payrollEmployees, string paymentType)
        {
            for (var i = 0; i < payrollEmployees.Count; ++i)
            {
                payrollEmployees[i] = await calculateAllDeductionsForEmployee(payrollEmployees[i], 
                    paymentType);
            }
            return payrollEmployees;
        }

        private async Task<PayrollEmployeeModel> calculateAllDeductionsForEmployee(PayrollEmployeeModel 
            payrollEmployee, string paymentType)
        {
            for (var i = 0; i < payrollEmployee.deductions.Count; ++i)
            {
                payrollEmployee.deductions[i] =  await calculateSingleDeductionForEmployee(payrollEmployee, 
                    payrollEmployee.deductions[i], paymentType);
            }
            return payrollEmployee;
        }

        private async Task<PayrollDeductionModel> calculateSingleDeductionForEmployee(PayrollEmployeeModel 
            payrollEmployee, PayrollDeductionModel deduction, string paymentType)
        {
            switch (deduction.formulaType)
            {
                case DEDUCTION_BY_API:
                    deduction = await deductionByApi(deduction, payrollEmployee, paymentType);
                    break;
                case DEDUCTION_BY_FIXED_AMOUNT:
                    deduction = deductionByFixedAmount(deduction, paymentType);
                    break;
                case DEDUCTION_BY_PERCENTAGE:
                    deduction = deductionByPercentage(deduction, payrollEmployee.computedGrossSalary);
                    break;
                default:
                    throw new Exception("Deduction: type of deduction is not supported.");
            }
            return deduction;
        }

        private PayrollDeductionModel deductionByFixedAmount(PayrollDeductionModel deduction, 
            string paymentType)
        {
            var amount = convertStringToDouble(deduction.param1Value);
            deduction.resultAmount = calculateFixedAmountByPaymentType(amount, paymentType);
            return deduction;
        }

        private PayrollDeductionModel deductionByPercentage(PayrollDeductionModel deduction, double salary)
        {
            var percentage = convertStringToDouble(deduction.param1Value);
            percentage = percentage / PERCENTAGE_DIVISOR;
            deduction.resultAmount = salary * percentage;
            return deduction;
        }

        private async Task<PayrollDeductionModel> deductionByApi(PayrollDeductionModel deduction, 
            PayrollEmployeeModel employee, string paymentType)
        {
            JsonDocument myJsonDoc;
            switch (deduction.apiUrl)
            {
                case VORLAGENERSTELLER_API_URL:
                    myJsonDoc = await apiVorlagenersteller(deduction, employee);
                    break;
                case FRIENDS_API_URL:
                    myJsonDoc = await apiFriends(deduction, employee);
                    break;
                case GEEMS_API_URL:
                    myJsonDoc = await apiGeems(deduction, employee);
                    break;
                default:
                    throw new Exception("Unknown API can not be used.");
            }
            var extractedValue = extractDoubleValue(myJsonDoc.RootElement);
            deduction.resultAmount = calculateFixedAmountByPaymentType(extractedValue, paymentType);
            return deduction;
        }

        private double calculateFixedAmountByPaymentType(double amount, string paymentType)
        {
            switch (paymentType)
            {
                case MONTHLY_PAYMENT_TYPE:
                    break;
                case BIWEEKLY_PAYMENT_TYPE:
                    amount = amount / DEDUCTION_DIVISOR;
                    break;
                default:
                    throw new Exception("Deduction: Payment type not supported.");
            }
            return amount;
        }

        private double convertStringToDouble(string value)
        {
            var convertedValue = 0.0;
            if (!double.TryParse(value, out convertedValue)
                || Double.IsNaN(convertedValue) || Double.IsInfinity(convertedValue)
                || Double.IsNegative(convertedValue))
            {
                throw new Exception("The string does not represent a positive proper double number.");
            }
            return convertedValue;
        }

        private async Task<JsonDocument> apiFriends(PayrollDeductionModel deduction
            , PayrollEmployeeModel employee)
        {
            using var client = new HttpClient();
            var birthDate = Convert.ToString(employee.birthDate.ToString("yyyy-MM-dd"));
            var gender = convertGenderToEnglish(employee.gender);
            var url = $"{FRIENDS_API_URL}?{deduction.param1Key}={birthDate}&{deduction.param2Key}={gender}";
            client.DefaultRequestHeaders.Add(deduction.header1Key, deduction.header1Value);
            var response = await client.GetAsync(url);
            return await readApiResponse(response);
        }

        private async Task<JsonDocument> apiVorlagenersteller(PayrollDeductionModel deduction
            , PayrollEmployeeModel employee)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add(deduction.header1Key, deduction.header1Value);
            var requestBody = new
            {
                fechaNacimiento = employee.birthDate,
                genero = employee.gender,
                cantidadDependientes = deduction.dependantNumber
            };
            var content = serializeToJson(requestBody);
            var response = await client.PostAsync(VORLAGENERSTELLER_API_URL, content);
            return await readApiResponse(response);
        }

        private async Task<JsonDocument> apiGeems(PayrollDeductionModel deduction
            , PayrollEmployeeModel employee)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add(deduction.header1Key, deduction.header1Value);
            var requestBody = new
            {
                associationName = employee.companyAssociation,
                employeeSalary = employee.computedGrossSalary
            };
            var content = serializeToJson(requestBody);
            var response = await client.PostAsync(GEEMS_API_URL, content);
            return await readApiResponse(response);
        }

        private StringContent serializeToJson(Object csharpObject)
        {
            var json = JsonSerializer.Serialize(csharpObject);
            var content = new StringContent(json, Encoding.UTF8, JSON_ENCODING);
            return content;
        }

        private async Task<JsonDocument> readApiResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            return jsonDoc;
        }

        private string convertGenderToEnglish(string gender)
        {
            var translatedGender = "";
            switch (gender)
            {
                case MALE_IN_SPANISH:
                    translatedGender = MALE_IN_ENGLISH;
                    break;
                case FEMALE_IN_SPANISH:
                    translatedGender = FEMALE_IN_ENGLISH;
                    break;
                default:
                    throw new Exception("Not supported gender");
            }
            return translatedGender;
        }

        private double extractDoubleValue(JsonElement rootElement)
        {
            switch (rootElement.ValueKind)
            {
                case JsonValueKind.Number:
                    return rootElement.GetDouble();
                case JsonValueKind.Object:
                    if (rootElement.TryGetProperty(FRIENDS_API_JSON_RETURN_KEY, out var monthlyCostProp)
                        && monthlyCostProp.ValueKind == JsonValueKind.Number)
                    {
                        return monthlyCostProp.GetDouble();
                    }
                    if (rootElement.TryGetProperty(GEEMS_API_JSON_RETURN_KEY, out var amountToChargeProp)
                        && amountToChargeProp.ValueKind == JsonValueKind.Number)
                    {
                        return amountToChargeProp.GetDouble();
                    }
                    throw new Exception("No numeric fields found in Json object.");
                default:
                    throw new Exception("Unsupported JsonElement format.");
            }
        }
    }
}
