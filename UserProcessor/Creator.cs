using System.Net;
using System.Text;
using System.Text.Json;
using Bogus;

namespace UserProcessor
{
    public static class Creator
    {
        public static async Task<RegistrationResultModel> CreateUser(string? login = default)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Chrome/135.0.0.0");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var faker = new Faker();

            if (string.IsNullOrEmpty(login))
            {
                login = $"{faker.Internet.UserName()}@TESTING.cz";
            }
            
            string password = "ahoj123123";

            var idsUrl = @"https://dev.alza.cz/Services/EShopService.svc/SaveUserInfo";
            var jsonBody = CreateUserRequestBody.GetRegistrationBody(login, password);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpStatusCode responseCode;
            string? message;

            try
            {
                var response = await client.PostAsync(idsUrl, content);
                var responseJson = await response.Content.ReadAsStringAsync();
                responseCode = response.StatusCode;

                using var doc = JsonDocument.Parse(responseJson);
                var d = doc.RootElement.GetProperty("d");

                if (d.TryGetProperty("Message", out var msgElem))
                {
                    message = msgElem.GetString();
                }
                else
                {
                    message = null;
                }
            }
            catch (Exception ex)
            {
                throw new ($"Error: {ex.Message}");
            }

            return new RegistrationResultModel
            {
                Login = login,
                Password = password,
                ResponseCode = responseCode,
                Message = message
            };
        }
    }
}
