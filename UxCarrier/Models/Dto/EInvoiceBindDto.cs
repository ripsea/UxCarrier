using FluentValidation;
using System;
using System.Text.Json;

namespace UxCarrier.Models.Dto
{
    public class EInvoiceBindDto
    {
        public string? token { get; set; }
        public string? ban { get; set; }
        public string? nonce { get; set; }
        public string? token_flag { get; set; }
        public string? err_msg { get; set; }
        public string? signature { get; set; }
        public string? email { get; set; }

        public override string? ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }

    public class Step1Validator : AbstractValidator<EInvoiceBindDto>
    {
        public Step1Validator()
        {
            RuleFor(x => x.token).NotEmpty();
            RuleFor(x => x.ban).NotEmpty();
            RuleFor(x => x.email).Empty();
        }
    }

    public class Step4Validator : AbstractValidator<EInvoiceBindDto>
    {
        public Step4Validator()
        {
            RuleFor(x => x.nonce).NotEmpty();
            RuleFor(x => x.token).NotEmpty();
            //RuleFor(x => x.ban).NotEmpty();
            RuleFor(x => x.email).NotEmpty();
        }
    }

    //public class Step3Validator : AbstractValidator<EInvoiceBindDto>
    //{
    //    public Step3Validator()
    //    {
    //        RuleFor(x => x.nonce).NotEmpty();
    //        RuleFor(x => x.token_flag).NotEmpty();
    //    }
    //}

    public class Step0Validator : AbstractValidator<EInvoiceBindDto>
    {
        public Step0Validator()
        {
            RuleFor(x => x.token).Null();
            RuleFor(x => x.ban).Null();
            RuleFor(x => x.nonce).Null();
            RuleFor(x => x.token_flag).Null();
        }
    }
}