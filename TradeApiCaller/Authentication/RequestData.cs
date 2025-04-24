using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TradeApiCaller.Authentication
{
    /// <summary>
    /// Represents the data for the Identity Management request.
    /// </summary>
    public sealed record RequestData
    {
        #region Exchange Code For Token

        /// <summary>
        /// Gets or sets the code for the request.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the client ID for the request.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("client_id")]
        public string? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the grant type for the request.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("grant_type")]
        public string? GrantType { get; set; }

        /// <summary>
        /// Gets or sets the redirect URI for the request.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("redirect_uri")]
        public string? RedirectUri { get; set; }

        /// <summary>
        /// Gets or sets the code verifier for the request.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("code_verifier")]
        public string? CodeVerifier { get; set; }

        #endregion

        #region Account Login

        /// <summary>
        /// Gets or sets the return URL for the request.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("ReturnUrl")]
        public string? ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets the request verification token for the request.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("__RequestVerificationToken")]
        public string? RequestVerificationToken { get; set; }

        /// <summary>
        /// Gets or sets the country code for the request.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("CountryCode")]
        public string? CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the username for the request.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Username")]
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the password for the request.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("Password")]
        public string? Password { get; set; }

        #endregion
    }
}
