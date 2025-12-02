using Microsoft.AspNetCore.Authentication.BearerToken;
using Newtonsoft.Json;
using payment.api.AppSettings;
using System.Net.Http.Headers;

namespace payment.api.Services.CommonServices
{
    public class BidvAccountService
    {
        public static async Task<AccessTokenResponse> GetAccessTokenAsync()
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
                         new KeyValuePair<string, string>("client_id", AppConst.bidvAccessTokenClientId),
                         new KeyValuePair<string, string>("client_secret", AppConst.bidvAccessTokenClientSecret),
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
    }
}
