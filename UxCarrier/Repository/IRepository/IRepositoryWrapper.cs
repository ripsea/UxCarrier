namespace UxCarrier.Repository.IRepository
{
    public interface IRepositoryWrapper
    {
        IUxCardEmailRepository CardEmail { get; }
        IUxCardRepository Card { get; }
        IUxBindCardRepository BindCard { get; }
        IInvoiceRepository InvoiceItem { get; }
        IEInvoiceBindCardRepository EInvoiceBindCard { get; }
        void Save();
    }
}
