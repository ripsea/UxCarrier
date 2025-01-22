using UxCarrier.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;
using UxCarrier.Models.Entities;
using UxCarrier.Controllers;
using UxCarrier.Models.Dto;

namespace UxCarrier.Repository.IRepository
{
    public interface IInvoiceRepository : IRepositoryBase<InvoiceItem>
    {
        public (IEnumerable<InvoiceItem> invoiceItems, PagedResult pagedResult) GetInvoices(InvoicesQueryDto queryParams);
    }
}
