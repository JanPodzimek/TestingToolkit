using System.Net;
using System.Net.Http.Json;

namespace TradeApiCaller.RegistrationNumber
{
    public class RegNumberVerifier
    {
        public async static Task<bool> CheckRegNumber(string bearerToken, int regNumber)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Chrome/135.0.0.0");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", bearerToken);

            var url = $"https://portalapi.dev.alza.cz/api/partner/exists-by-registration-number?registrationNumber={regNumber}";
            string partnerExists;
            HttpStatusCode responseStatus;

            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var dto = await response.Content
                .ReadFromJsonAsync<ExistByRegNumberResponseModel>()
                .ConfigureAwait(false);
            return dto?.Value ?? throw new InvalidOperationException("Failed to parse JSON");
        }
    }
}
