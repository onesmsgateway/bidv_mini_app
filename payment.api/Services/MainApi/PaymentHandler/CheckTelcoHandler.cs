using MediatR;
using Newtonsoft.Json;
using payment.api.Services.ModelApi;
using PaymentPackageTelco.api.Services.ModelApi.Request;
using PaymentPackageTelco.api.Services.ModelApi.Response;
using System.Net;
using System.Text;
using static payment.api.Services.ModelApi.ApiModelBase;

namespace PaymentPackageTelco.api.Services.MainApi.PaymentHandler
{
    public class CheckTelcoHandler : IRequestHandler<CheckTelcoRequest, IApiResponse>
    {
        public CheckTelcoHandler()
        {
        }
        public async Task<IApiResponse> Handle(CheckTelcoRequest CheckTelcoRequest, CancellationToken cancellationToken)
        {
            if(CheckTelcoRequest == null || string.IsNullOrEmpty(CheckTelcoRequest.PhoneNumber))
                return new ApiDetailedResponseBase()
                { StatusCode = HttpStatusCode.BadRequest, Message = "Phonenumber donnot allow empty", Details = null };
            
            using (var _httpClient = new HttpClient())
            {
                try
                {
                    using (var _request = new HttpRequestMessage(HttpMethod.Post, "https://topup-data.conek.vn/CheckTelco"))
                    {
                        var jsonBody = $"{{ \"phone_number\": \"{CheckTelcoRequest.PhoneNumber}\" }}";
                        _request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                        var response = await _httpClient.SendAsync(_request);

                        if (response.IsSuccessStatusCode)
                        {
                            var _json = await response.Content.ReadAsStringAsync();
                            return JsonConvert.DeserializeObject<CheckTelcoResponse>(_json);
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
