using System.Net;
using System.Text;
using Bogus;

namespace UserProcessor
{
    public static class Creator
    {
        public static async Task<RegistrationResultModel> CreateUser()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Chrome/135.0.0.0");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var faker = new Faker();
            string login = $"{faker.Internet.UserName()}@testing.cz";
            string password = "ahoj123123";

            var idsUrl = @"https://dev.alza.cz/Services/EShopService.svc/SaveUserInf";
            var jsonBody = CreateUserRequestBody.GetRegistrationBody(login, password);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpStatusCode responseCode;

            try
            {
                var response = await client.PostAsync(idsUrl, content);
                responseCode = response.StatusCode;
            }
            catch (Exception ex)
            {
                throw new ($"Error: {ex.Message}");
            }

            return new RegistrationResultModel
            {
                Login = login,
                Password = password,
                ResponseCode = responseCode
            };
        }
    }
}
