using System.Text;
using System.Text.Json;
using back_end.Application;
using back_end.Domain;
using back_end.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace back_end.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeductionsController : GeneralController
    {
        public DeductionsController()
        {

        }

        //[Authorize(Roles = "empleador")]
        [HttpPost]
        public async Task<IActionResult> ComputeDeductions(int type)
        {
            IActionResult iActionResult = BadRequest("Unknown error.");
            try
            {
                JsonDocument myJsonDoc;
                switch (type)
                {
                    case 1:
                        myJsonDoc = await ApiFriends();
                        break;
                    case 2:
                        myJsonDoc = await ApiVorlagenersteller();
                        break;
                    case 3:
                        myJsonDoc = await ApiGeems();
                        break;
                    default:
                        throw new Exception("Bad type");
                }
                double extractedValue = ExtractDoubleValue(myJsonDoc);

                iActionResult = Ok(extractedValue);
            }
            catch (Exception e)
            {
                iActionResult = NotFound(e.Message);
            }
            return iActionResult;
        }


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
