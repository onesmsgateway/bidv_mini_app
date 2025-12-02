using MediatR;

namespace payment.api.Services.ModelApi
{
    public class ApiModelBase
    {
        public ApiModelBase()
        {
        }

        public interface IApiInput: IRequest<IApiResponse>{}

        public interface IApiResponse { }

        public interface IApiResponseData { }
    }
}
