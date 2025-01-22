using UxCarrier.Data;
using UxCarrier.Models;
using UxCarrier.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UxCarrier.Models.Entities;

namespace UxCarrier.Repository
{
    public class EInvoiceBindCardRepository : RepositoryBase<ApplicationDbContext, EInvoiceBindCard>, IEInvoiceBindCardRepository
    {
        public EInvoiceBindCardRepository(ApplicationDbContext db) : base(db)
        {
        }

    }
}
