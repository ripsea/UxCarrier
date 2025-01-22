using Microsoft.EntityFrameworkCore;
using UxCarrier.Data;
using UxCarrier.Models;
using UxCarrier.Models.Dto;
using UxCarrier.Models.Entities;
using UxCarrier.Repository.IRepository;

namespace UxCarrier.Repository
{
    public class InvoiceItemRepository : RepositoryBase<EIVO03DbContext,InvoiceItem>, IInvoiceRepository
    {
        public InvoiceItemRepository(EIVO03DbContext db) : base(db)
        {
        }

        public (IEnumerable<InvoiceItem>, PagedResult) 
            GetInvoices(InvoicesQueryDto queryParams)
        {
            var invoices = FindAll()
                .Include(carrier => carrier.InvoiceCarrier)
                .Include(seller => seller.InvoiceSeller)
                .Include(buyer => buyer.InvoiceBuyer)
                .Include(bonus => bonus.InvoiceWinningNumber)
                .Include(amount => amount.InvoiceAmountType)
                .Include(invoice => invoice.InvoiceDetails)
                    .ThenInclude(detail => detail.InvoiceProduct)
                .Include(g => g.InvoiceDetails)
                    .ThenInclude(detail => detail.InvoiceProductItem)
                .Where(y => y.InvoiceCarrier.CarrierNo == queryParams.CarrierNo);
            
            return ToPagedList(invoices, queryParams.PageInfo);
        }
    }
}
