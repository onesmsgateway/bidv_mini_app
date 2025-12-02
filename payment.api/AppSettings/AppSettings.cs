namespace payment.api.AppSettings
{
    public static class AppConst
    {
        public static IConfigurationRoot _configuration;

        public static string GetAppSettingsKey(string key)
        {
            var _config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            _configuration = _config.Build();
            return _configuration[key];
        }

        public readonly static string jwtKey = GetAppSettingsKey("payment:jwt_key");
        public readonly static int jwtExpireHours = int.Parse(GetAppSettingsKey("payment:jwt_expire_hours"));
        public readonly static string webUrlBase = GetAppSettingsKey("webview:url_base");

        public readonly static string bidvSignSecretKey = GetAppSettingsKey("bidv:sign_secret_key");
        public readonly static string bidvAccessTokenClientId = GetAppSettingsKey("bidv:client_id");
        public readonly static string bidvAccessTokenClientSecret = GetAppSettingsKey("bidv:client_secret");
        public readonly static string bidvAccessTokenUrl = GetAppSettingsKey("bidv:request_access_token_url");
        public readonly static string bidvPaymentUrl = GetAppSettingsKey("bidv:payment_url");
        public readonly static string bidvSmartBankPaymentUrl = GetAppSettingsKey("bidv:smart_banking_payment_url"); //thieu endpoint
        public readonly static string bidvPartnerCode = GetAppSettingsKey("bidv:partner_code");
        public readonly static string bidvSecretKeyAres256 = GetAppSettingsKey("bidv:secret_key_ares256");
        public readonly static string bidvConfirmPaymentUrl = GetAppSettingsKey("bidv:payment_url");
        public readonly static string bidvNavigatorUrl = GetAppSettingsKey("bidv:navigator_url");

        public readonly static string partnerPrivateKeyRsa = GetAppSettingsKey("partner:private_key_rsa");
        public readonly static string partnerChannel = GetAppSettingsKey("partner:channel");
        public readonly static string partnerUserAgent = GetAppSettingsKey("partner:user_agent");
        public readonly static string partnerXCientCertificate = GetAppSettingsKey("partner:x_client_certificate");
        public readonly static string partnerCustomerIPAddress = GetAppSettingsKey("partner:customer_id_address");
        public readonly static string partnerServiceUrl = GetAppSettingsKey("invoice:service_url");
    }
}
