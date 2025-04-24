namespace TradeApiCaller.Authentication
{
    /// <summary>
    /// Constants used for trade identity management service.
    /// </summary>
    public static class Consts
    {
        /// <summary>
        /// Base URL for the Identity Server.
        /// </summary>
        public const string BaseUrl = "https://idserver.dev-dmz.alza.cz";

        /// <summary>
        /// Login URL for the Identity Server.
        /// </summary>
        public const string LoginUrl = $"{BaseUrl}/account/login";

        /// <summary>
        /// Token URL for the Identity Server.
        /// </summary>
        public const string TokenUrl = $"{BaseUrl}/connect/token";

        /// <summary>
        /// Client ID for the Identity Server.
        /// </summary>
        public const string ClientId = "alza_partner";

        /// <summary>
        /// Redirect URL for the Identity Server.
        /// </summary>
        public const string ProjectCallBackUrl = "https://portal.dev.alza.cz/account/signin-callback";
    }
}
