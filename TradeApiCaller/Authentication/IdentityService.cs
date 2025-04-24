using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Alza.API.Core.Http;
using Alza.API.Core.Phoenix; // TODO: Remove dependency on Phoenix

namespace TradeApiCaller.Authentication
{
    public static class IdentityService
    {
        private static readonly HttpClientHandler _handler = new()
        {
            UseCookies = true,
            AllowAutoRedirect = false
        };

        private static readonly HttpCoreClient _client;
        private static readonly Regex AntiForgeryRegex = new(
            @"<input[^>]+name=""__RequestVerificationToken""[^>]+value=""([^""]+)""",
            RegexOptions.Compiled | RegexOptions.CultureInvariant
        );

        // static ctor to set headers once
        static IdentityService()
        {
            _client = new HttpCoreClient(_handler);
            var headers = _client.DefaultRequestHeaders;
            if (!headers.Contains("scheme")) headers.Add("scheme", "https");
            if (!headers.Contains("user-agent")) headers.Add("user-agent", "API_tests");
            if (!headers.Contains("accept"))
                headers.Add("accept", "application/json, text/json, text/x-json, text/javascript, application/xml, text/xml, */*");
        }

        /// <summary>
        /// Asynchronously retrieves the access token.
        /// </summary>
        public static async Task<string> GetTokenValueAsync(string username, string password)
        {
            // 1) Generate PKCE
            (var codeVerifier, var codeChallenge) = GeneratePKCE();

            // 2) Log in and get redirect
            var loginResp = await GetLoginResponseAsync(codeChallenge, username, password)
                              .ConfigureAwait(false);

            if (loginResp.StatusCode != HttpStatusCode.Found)
                throw new InvalidOperationException(
                    $"Expected 302 but got {(int)loginResp.StatusCode}");

            // 3) Exchange for code + token
            var authCode = await GetAuthorizationCodeAsync(codeChallenge)
                              .ConfigureAwait(false);
            if (string.IsNullOrEmpty(authCode))
                throw new InvalidOperationException("Failed to retrieve authorization code.");

            var token = await ExchangeCodeForTokenAsync(authCode, codeVerifier)
                             .ConfigureAwait(false);

            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Failed to retrieve access token.");

            return token;
        }

        private static (string CodeVerifier, string CodeChallenge) GeneratePKCE()
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

        private static async Task<HttpResponse> GetLoginResponseAsync(
            string codeChallenge, string username, string password)
        {
            // GET login page
            using var req1 = HttpFactory.CreateRequest(HttpMethod.Get, Consts.LoginUrl);
            var html = await _client.SendRequestAsync(req1)
                             .ConfigureAwait(false);

            // extract anti-forgery
            var token = ExtractAntiForgeryToken(html.Content);
            if (string.IsNullOrEmpty(token))
                throw new InvalidOperationException("Anti-forgery token not found.");

            // POST credentials
            var data = new RequestData
            {
                ReturnUrl = BuildReturnUrl(codeChallenge),
                CountryCode = "CZ",
                Username = username,
                Password = password,
                RequestVerificationToken = token
            };
            using var req2 = new HttpRequestMessage(HttpMethod.Post, Consts.LoginUrl)
            {
                Content = HttpFactory.CreateFormUrlEncodedContent(data)
            };
            return await _client.SendRequestAsync(req2)
                         .ConfigureAwait(false);
        }

        private static string ExtractAntiForgeryToken(string html)
            => AntiForgeryRegex.Match(html) is var m && m.Success
               ? m.Groups[1].Value
               : string.Empty;

        private static string BuildReturnUrl(string codeChallenge)
        {
            var qs = HttpUtility.ParseQueryString(string.Empty);
            qs["client_id"] = Consts.ClientId;
            qs["redirect_uri"] = Consts.ProjectCallBackUrl;
            qs["response_type"] = "code";
            qs["scope"] = "offline_access email profile openid partner";
            qs["code_challenge"] = codeChallenge;
            qs["code_challenge_method"] = "S256";

            return Consts.BaseUrl + "/connect/authorize/callback?" + qs;
        }

        private static async Task<string> GetAuthorizationCodeAsync(string codeChallenge)
        {
            var url = BuildReturnUrl(codeChallenge);
            using var req = HttpFactory.CreateRequest(HttpMethod.Get, url);
            var resp = await _client.SendRequestAsync(req)
                             .ConfigureAwait(false);

            if (resp.StatusCode != HttpStatusCode.Found)
                throw new InvalidOperationException("Auth redirect failed.");

            var loc = resp.Headers.Location;
            return loc is null
                ? string.Empty
                : HttpUtility.ParseQueryString(loc.Query)["code"] ?? string.Empty;
        }

        private static async Task<string> ExchangeCodeForTokenAsync(
            string authCode, string codeVerifier)
        {
            var data = new RequestData
            {
                ClientId = Consts.ClientId,
                GrantType = "authorization_code",
                Code = authCode,
                RedirectUri = Consts.ProjectCallBackUrl,
                CodeVerifier = codeVerifier
            };
            using var req = new HttpRequestMessage(HttpMethod.Post, Consts.TokenUrl)
            {
                Content = HttpFactory.CreateFormUrlEncodedContent(data)
            };
            var resp = await _client.SendRequestAsync<TokenResponse>(req)
                             .ConfigureAwait(false);
            return resp.Data?.AccessToken ?? string.Empty;
        }
    }
}
