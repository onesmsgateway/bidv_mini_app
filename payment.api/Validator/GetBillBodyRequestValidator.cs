using payment.api.Common;
using payment.api.Services.ModelApi.Request;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace payment.api.Validator
{
    public sealed class QueryBillBodyValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("thiếu thông tin Checksum");
            }
            if (!Regex.IsMatch(value.ToString(), "^[a-zA-Z0-9]*$"))
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} chỉ cho phép chữ số và ký tự không dấu");
            }

            return ValidationResult.Success;
        }
    }

    public static class CheckSumGetBilLValidator
    {
        public static bool IsValidChecksum(this GetBillBodyRequest request)
        {
            var _macSha256 = Utils.GenerateSha256(request.BillNumber, request.ServiceId);
            return request.Checksum.Equals(_macSha256);
        }
    }
}
