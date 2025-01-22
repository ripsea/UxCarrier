using FluentValidation;

namespace UxCarrier.Models.Dto
{
    public class LoginRequestDto
    {
        public string? Email { get; set; }
        public string? OtpCode { get; set; }
        public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
        {
            public LoginRequestDtoValidator()
            {
                RuleFor(x => x.Email)
                  .NotEmpty()
                  .WithMessage("Email is required.")
                  .EmailAddress();

                RuleFor(x => x.OtpCode)
                  .NotEmpty()
                  .WithMessage("OtpCode is required.")
                  .Matches(@"^\d{4}$");
            }
        }

        public class TryLoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
        {
            public TryLoginRequestDtoValidator()
            {
                RuleFor(x => x.Email)
                  .NotEmpty()
                  .WithMessage("Email is required.")
                  .EmailAddress();
            }
        }
    }
}
