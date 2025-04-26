using System.Net;
using System.Net.Http.Json;

namespace TradeApiCaller.RegistrationNumber
{
    public static class RegNumberVerifier
    {
        public async static Task<bool> CheckRegNumber(HttpClient client, string regNumber)
        {
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
