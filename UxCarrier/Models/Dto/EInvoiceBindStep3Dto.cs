using FluentValidation;
using System;

namespace UxCarrier.Models.Dto
{
    public class EInvoiceBindStep3Dto
    {
        public string? nonce { get; set; }
        public string? token_flag { get; set; }
        public string? err_msg { get; set; } = "請洽客服";
    }

    public class EInvoiceBindRespDtoValidator : AbstractValidator<EInvoiceBindStep3Dto>
    {
        public EInvoiceBindRespDtoValidator()
        {
            RuleFor(x => x.nonce).NotEmpty();
            RuleFor(x => x.token_flag).NotEmpty();
        }
    }
}