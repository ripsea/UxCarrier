using FluentValidation;
using System.Net;
using System.Text.Json;

namespace UxCarrier.Models.Dto
{
    public class UxBindStep1Dto
    {
        public string Token { get; set; }

        public class PostStep1DtoValidator : AbstractValidator<UxBindStep1Dto>
        {
            public PostStep1DtoValidator()
            {
                RuleFor(x => x.Token)
                  .NotEmpty();
            }
        }

        public override string? ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
