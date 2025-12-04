using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace payment.api.Validator
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public sealed class TimestampValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult("thiếu thông tin timestamp");

            var _expectedFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
            bool isValid = DateTimeOffset.TryParseExact(
               value.ToString(), _expectedFormat, CultureInfo.InvariantCulture,
               DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
               out _);
            if (!isValid)
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} định dạng chưa đúng: {_expectedFormat}.");

            return ValidationResult.Success;
        }
    }

    public sealed class XApiInteractionIdValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult("thiếu thông tin X-API-Interaction-ID");
            
            return ValidationResult.Success;
        }
    }
}
