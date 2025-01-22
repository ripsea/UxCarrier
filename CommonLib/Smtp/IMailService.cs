namespace CommonLib.Smtp
{
    public interface IMailService
    {
        Task<bool> SendMailAsync(MailData mailData, CancellationToken ct = default);
    }
}
