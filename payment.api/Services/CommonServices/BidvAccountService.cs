using Newtonsoft.Json;
using payment.api.AppSettings;
using payment.api.Services.ModelApi.Response;
using PaymentPackageTelco.api.Common;
using System.Net.Http.Headers;

namespace payment.api.Services.CommonServices
{
    public class BidvAccountService
    {
        public static async Task<AccessTokenResponse> GetAccessTokenAsync(string tokenClientId, string clientSecret)
        {
            var _requestAccessTokenUrl = AppConst.bidvAccessTokenUrl;
            using (var _httpClient = new HttpClient())
            {
                try
                {
                    using (var _request = new HttpRequestMessage(HttpMethod.Post, _requestAccessTokenUrl))
                    {
                        var content = new FormUrlEncodedContent(new[]
                        { 
                         new KeyValuePair<string, string>("grant_type", "client_credentials"),
                         new KeyValuePair<string, string>("client_id", tokenClientId),
                         new KeyValuePair<string, string>("client_secret", clientSecret),
                         new KeyValuePair<string, string>("scope", "read"),
                        });
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        _request.Content = content;

                        var response = await _httpClient.SendAsync(_request);
                        if (response.IsSuccessStatusCode)
                        {
                            var _json = await response.Content.ReadAsStringAsync();
                          return JsonConvert.DeserializeObject<AccessTokenResponse>(_json);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                return null;
            }
        }

        public static async Task<AccessTokenResponse?> GetTokenAsynWithCache(string tokenClientId, string clientSecret, string _keyCache)
        {
            var _cacheToken = RedisHelper.Get<AccessTokenResponse>(_keyCache);
            if (_cacheToken == null || string.IsNullOrWhiteSpace(_cacheToken.access_token))
            {
                _cacheToken = await GetAccessTokenAsync(tokenClientId, clientSecret);
                RedisHelper.Set(_keyCache, _cacheToken, int.Parse(AppConst.bidvCacheMunutesDuration));
            }
            return _cacheToken;
        }
    }
}
