using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using Serilog;

namespace TradeApiCaller.Authentication
{
    public static class IdentityService
    {
        private static readonly HttpClientHandler _handler = new()
        {
            UseCookies = true,
            AllowAutoRedirect = false
        };

        private static readonly HttpClient _client;
        private static readonly Regex AntiForgeryRegex = new(
            @"<input[^>]+name=""__RequestVerificationToken""[^>]+value=""([^""]+)""",
            RegexOptions.Compiled | RegexOptions.CultureInvariant
        );

        static IdentityService()
        {
            _client = new HttpClient(_handler);
            _client.DefaultRequestHeaders.Add("scheme", "https");
            _client.DefaultRequestHeaders.Add("user-agent", "API_tests");
            _client.DefaultRequestHeaders.Add("accept", "application/json, text/json, text/x-json, text/javascript, application/xml, text/xml, */*");
        }

        public static async Task<string> GetTokenValueAsync(string username, string password)
        {
            var (verifier, challenge) = GeneratePKCE();

            var loginResp = await GetLoginResponseAsync(challenge, username, password);
            if (loginResp.StatusCode != HttpStatusCode.Found)
                throw new InvalidOperationException($"Expected 302 but got {(int)loginResp.StatusCode}");

            var code = await GetAuthorizationCodeAsync(challenge);
            if (string.IsNullOrEmpty(code))
                throw new InvalidOperationException("Failed to retrieve authorization code.");

            var token = await ExchangeCodeForTokenAsync(code, verifier);
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Failed to retrieve access token.");

            return token;
        }

        private static (string Verifier, string Challenge) GeneratePKCE()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            var codeVerifier = Convert.ToBase64String(bytes)
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');

            var hash = SHA256.HashData(Encoding.ASCII.GetBytes(codeVerifier));
            var codeChallenge = Convert.ToBase64String(hash)
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');

            return (codeVerifier, codeChallenge);
        }

        private static async Task<HttpResponseMessage> GetLoginResponseAsync(
            string challenge, string username, string password)
        {
            // 1) GET login page
            string token = string.Empty;

            try
            {
                var page = await _client.GetStringAsync(Consts.LoginUrl);
                token = ExtractAntiForgeryToken(page);
                if (string.IsNullOrEmpty(token))
                    throw new InvalidOperationException("Anti-forgery token not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Log.Logger.Error(ex.Message);
                Log.Logger.Error("Check your VPN connection and run the app again.");

                throw new AccessViolationException();
            }

            // 2) POST credentials
            var form = new Dictionary<string, string>
            {
                ["ReturnUrl"] = BuildReturnUrl(challenge),
                ["CountryCode"] = "CZ",
                ["Username"] = username,
                ["Password"] = password,
                ["__RequestVerificationToken"] = token
            };
            var req = new HttpRequestMessage(HttpMethod.Post, Consts.LoginUrl)
            {
                Content = new FormUrlEncodedContent(form)
            };
            return await _client.SendAsync(req);
        }

        private static string ExtractAntiForgeryToken(string html) =>
            AntiForgeryRegex.Match(html) is var m && m.Success
                ? m.Groups[1].Value
                : throw new InvalidOperationException("Anti-forgery token not found.");

        private static string BuildReturnUrl(string challenge)
        {
            var qs = HttpUtility.ParseQueryString(string.Empty);
            qs["client_id"] = Consts.ClientId;
            qs["redirect_uri"] = Consts.ProjectCallBackUrl;
            qs["response_type"] = "code";
            qs["scope"] = "offline_access email profile openid partner";
            qs["code_challenge"] = challenge;
            qs["code_challenge_method"] = "S256";

            return Consts.BaseUrl + "/connect/authorize/callback?" + qs;
        }

        private static async Task<string> GetAuthorizationCodeAsync(string challenge)
        {
            var url = BuildReturnUrl(challenge);
            var resp = await _client.GetAsync(url);
            if (resp.StatusCode != HttpStatusCode.Found)
                throw new InvalidOperationException("Auth redirect failed.");

            var loc = resp.Headers.Location;
            return loc is null
                ? string.Empty
                : HttpUtility.ParseQueryString(loc.Query)["code"] ?? string.Empty;
        }

        private static async Task<string> ExchangeCodeForTokenAsync(
            string authCode, string verifier)
        {
            var form = new Dictionary<string, string>
            {
                ["client_id"] = Consts.ClientId,
                ["grant_type"] = "authorization_code",
                ["code"] = authCode,
                ["redirect_uri"] = Consts.ProjectCallBackUrl,
                ["code_verifier"] = verifier
            };
            var req = new HttpRequestMessage(HttpMethod.Post, Consts.TokenUrl)
            {
                Content = new FormUrlEncodedContent(form)
            };
            var resp = await _client.SendAsync(req);
            resp.EnsureSuccessStatusCode();

            var tokenResp = await JsonSerializer.DeserializeAsync<TokenResponse>(
                await resp.Content.ReadAsStreamAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            return tokenResp?.AccessToken ?? string.Empty;
        }

        
    }
}
