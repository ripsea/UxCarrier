using FluentValidation;

namespace UxCarrier.Models
{
    public abstract class GetPaginatedDataRequestValidator<T> : AbstractValidator<T> where T : PageRequest
    {
        protected GetPaginatedDataRequestValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Sort)
                .Must(CommonValidators.CheckSort)
                .WithMessage($"sort should be like: -name,+createdTime");
        }
    }

    public class PageRequest
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Sort { get; set; }
    };

    public class sortOperatorValidator : AbstractValidator<string>
    {
        public sortOperatorValidator()
        {
            RuleFor(x => x).Must(s => s.StartsWith("+") || s.StartsWith("-"));
        }
    }
}
