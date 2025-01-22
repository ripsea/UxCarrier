using FluentValidation;
using UxCarrier.Services.Validation;

namespace UxCarrier.Models.Request
{
    public class InvoiceQueryRequest: PageRequest
    {
        public string? Email { get; set; } = string.Empty;
    }

    public class InvoiceQueryRequestValidator
        : GetPaginatedDataRequestValidator<InvoiceQueryRequest>
    {
        public InvoiceQueryRequestValidator()
        {
            //Include(new ISmsReportRequestValidator(authSession));
            //RuleFor(x => x.Email).NotEmpty();
        }
    }
}