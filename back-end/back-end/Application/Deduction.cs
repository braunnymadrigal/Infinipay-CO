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

        private const string VORLAGENERSTELLER_API_URL = 
            "https://mediseguro-vorlagenersteller-d4hmbvf7frg7aqan.southcentralus-01.azurewebsites.net/api/MediSeguroMonto";
        private const string FRIENDS_API_URL =
            "https://poliza-friends-grg0h9g5crf2hwh8.southcentralus-01.azurewebsites.net/api/LifeInsurance";
        private const string GEEMS_API_URL =
            "https://asociacion-geems-c3dfavfsapguhxbp.southcentralus-01.azurewebsites.net/api/public/calculator/calculate";

        private const double PERCENTAGE_DIVISOR = 100.0;

        public async Task<List<PayrollEmployeeModel>> computeDeductions(List<PayrollEmployeeModel> payrollEmployees)
        {
            for (int i = 0; i < payrollEmployees.Count; ++i)
            {
                var payrollEmployee = payrollEmployees[i];
                var deductions = payrollEmployee.deductions;
                for (int j = 0; j < deductions.Count; ++j)
                {
                    var deduction = deductions[j];
                    switch (deduction.formulaType)
                    {
                        case DEDUCTION_BY_API:
                            payrollEmployees[i].deductions[j] = await deductionByApi(deduction, payrollEmployee);
                            break;
                        case DEDUCTION_BY_FIXED_AMOUNT:
                            payrollEmployees[i].deductions[j] = deductionByFixedAmount(deduction);
                            break;
                        case DEDUCTION_BY_PERCENTAGE:
                            payrollEmployees[i].deductions[j] = deductionByPercentage(deduction, payrollEmployees[i].computedGrossSalary);
                            break;
                        default:
                            throw new Exception("A type of deduction is not supported.");
                    }
                }
            }
            return payrollEmployees;
        }

        private PayrollDeductionModel deductionByFixedAmount(PayrollDeductionModel deduction)
        {
            deduction.resultAmount = convertStringToDouble(deduction.param1Value);
            return deduction;
        }

        private PayrollDeductionModel deductionByPercentage(PayrollDeductionModel deduction, double salary)
        {
            var percentage = convertStringToDouble(deduction.param1Value);
            percentage = percentage / PERCENTAGE_DIVISOR;
            deduction.resultAmount = salary * percentage;
            return deduction;
        }

        private async Task<PayrollDeductionModel> deductionByApi(PayrollDeductionModel deduction, PayrollEmployeeModel employee)
        {
            JsonDocument myJsonDoc;
            switch (deduction.apiUrl)
            {
                case VORLAGENERSTELLER_API_URL:
                    myJsonDoc = await ApiVorlagenersteller();
                    break;
                case FRIENDS_API_URL:
                    myJsonDoc = await ApiFriends();
                    break;
                case GEEMS_API_URL:
                    myJsonDoc = await ApiGeems();
                    break;
                default:
                    throw new Exception("Unknown API can not be used.");
            }
            var extractedValue = ExtractDoubleValue(myJsonDoc);
            deduction.resultAmount = extractedValue;
            return deduction;
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

        //[Authorize(Roles = "empleador")]
        //[HttpPost]
        //public async Task<IActionResult> ComputeMoreDeductions(int type)
        //{
        //    IActionResult iActionResult = BadRequest("Unknown error.");
        //    try
        //    {
        //        JsonDocument myJsonDoc;
        //        switch (type)
        //        {
        //            case 1:
        //                myJsonDoc = await ApiFriends();
        //                break;
        //            case 2:
        //                myJsonDoc = await ApiVorlagenersteller();
        //                break;
        //            case 3:
        //                myJsonDoc = await ApiGeems();
        //                break;
        //            default:
        //                throw new Exception("Bad type");
        //        }
        //        double extractedValue = ExtractDoubleValue(myJsonDoc);

        //        iActionResult = Ok(extractedValue);
        //    }
        //    catch (Exception e)
        //    {
        //        iActionResult = NotFound(e.Message);
        //    }
        //    return iActionResult;
        //}


        private async Task<JsonDocument> ApiFriends()
        {
            using var client = new HttpClient();

            // base URL
            var baseUrl = "https://poliza-friends-grg0h9g5crf2hwh8.southcentralus-01.azurewebsites.net/api/LifeInsurance";

            // query parameters
            var requestUrl = $"{baseUrl}?date%20of%20birth=2004-10-12&sex=male";

            // headers
            client.DefaultRequestHeaders.Add("FRIENDS-API-TOKEN", "1D0B194488C091852597B9AF7D1AA8F23D55C9784815489CF0A488B6F2C6D5C4C569AD51231FACB9920E5A763FE388247E03131D1601AC234E86BC0D266EB6A7");

            // GET
            var response = await client.GetAsync(requestUrl);

            var responseContent = await response.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(responseContent);
            return doc;
        }

        private async Task<JsonDocument> ApiVorlagenersteller()
        {
            using var client = new HttpClient();

            var url = "https://mediseguro-vorlagenersteller-d4hmbvf7frg7aqan.southcentralus-01.azurewebsites.net/api/MediSeguroMonto";

            client.DefaultRequestHeaders.Add("token", "TOKEN123");

            // Create the JSON body
            var requestBody = new
            {
                fechaNacimiento = "2000-10-31",
                genero = "masculino",
                cantidadDependientes = 0
            };

            // Serialize to JSON
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await client.PostAsync(url, content);

            // Read the response
            var responseContent = await response.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(responseContent);
            return doc;
        }

        private async Task<JsonDocument> ApiGeems()
        {
            using var client = new HttpClient();

            var url = "https://asociacion-geems-c3dfavfsapguhxbp.southcentralus-01.azurewebsites.net/api/public/calculator/calculate";

            client.DefaultRequestHeaders.Add("API-KEY", "Tralalerotralala");

            // Create the JSON body
            var requestBody = new
            {
                associationName = "GEEMS",
                employeeSalary = 500000.75
            };

            // Serialize to JSON
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Send the POST request
            var response = await client.PostAsync(url, content);

            // Read the response
            var responseContent = await response.Content.ReadAsStringAsync();

            var doc = JsonDocument.Parse(responseContent);
            return doc;
        }


        static double ExtractDoubleValue(JsonDocument doc)
        {
            var root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Number)
            {
                return root.GetDouble();
            }

            if (root.ValueKind == JsonValueKind.Object)
            {
                if (root.TryGetProperty("monthlyCost", out var monthlyCostProp) && monthlyCostProp.ValueKind == JsonValueKind.Number)
                {
                    return monthlyCostProp.GetDouble();
                }

                if (root.TryGetProperty("amountToCharge", out var amountToChargeProp) && amountToChargeProp.ValueKind == JsonValueKind.Number)
                {
                    return amountToChargeProp.GetDouble();
                }

                throw new Exception("No known numeric fields found in JSON object.");
            }

            throw new Exception("Unsupported JSON format.");
        }
    }
}
