using CommonLib.Smtp;
using Microsoft.Extensions.Options;

namespace UxCarrier.Services.Email
{
    public class EmailFactory
    {
        private readonly MailSettings _mailSettings;
        private readonly EmailBody _emailBody;
        private readonly CommonLib.Smtp.IMailService _mailService;
        public EmailFactory(
            EmailBody emailBody,
            IOptions<MailSettings> mailSettingsOptions,
            CommonLib.Smtp.IMailService mailService)
        {
            _mailSettings = mailSettingsOptions.Value;
            _mailService = mailService;
            _emailBody = emailBody;
        }

        public async Task<string> CreateVerifyCodeEmailBody(string verifyCode)
        {
            _emailBody.VerifyCode = verifyCode;
            _emailBody.SetTemplateItem("VerifyCode");
            return await _emailBody.GetViewRenderString();
        }

        public async void SendEmailToCustomer(string emailTo, string subject, string body)
        {
            List<string> emailList = new List<string>
            {
                emailTo
            };

            MailData mailData = new MailData(
                to: emailList,
                subject: subject,
                body: body,
                from: _mailSettings.From,
                displayName: _mailSettings.DisplayName,
                replyTo: null,
                replyToName: null,
                bcc: null,
                cc: null
            );
            //FileLogger.Logger.Error(emailTo);
            await _mailService.SendMailAsync(mailData, default);

        }
    }
}
