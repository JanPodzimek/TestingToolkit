namespace TradeApiCaller.HashId
{
    public static class HashIdProcessor
    {
        public async static Task<string> ProcessHashId(HttpClient client, string hashId)
        {
            bool validInt = int.TryParse(hashId, out var number);
            string suffix = validInt ? "encode" : "decode"; 
            string url = $"https://portalapi.dev.alza.cz/api/hash-ids/{hashId}/{suffix}";

            var response = await client.GetAsync(url);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex) when (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return $"The EntityModel with identifier '{hashId}' was not found.";
            }

            string result = await response.Content.ReadAsStringAsync();
            result = validInt ? $"HashId: {result.Trim('"')}" : $"Id: {result}";

            return result;
        }
    }
}
