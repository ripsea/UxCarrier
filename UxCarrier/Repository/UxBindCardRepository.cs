using UxCarrier.Data;
using UxCarrier.Models;
using UxCarrier.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UxCarrier.Models.Entities;

namespace UxCarrier.Repository
{
    public class UxBindCardRepository : RepositoryBase<ApplicationDbContext, UxBindCard>, IUxBindCardRepository
    {
        public UxBindCardRepository(ApplicationDbContext db) : base(db)
        {
        }

    }
}
