using FluentValidation;

namespace UxCarrier.Models.Dto
{
    public class OtpRequestDto
    {
        public string? Email { get; set; }
        public bool IsJoinMember { get; set; }
        public class OtpRequestDtoValidator : AbstractValidator<OtpRequestDto>
        {
            public OtpRequestDtoValidator()
            {
                RuleFor(x => x.Email)
                  .NotEmpty()
                  .WithMessage("Email is required.")
                  .EmailAddress();

                RuleFor(x => x.IsJoinMember)
                  .NotEmpty()
                  .WithMessage("IsJoinMember is required.");
            }
        }
    }
}