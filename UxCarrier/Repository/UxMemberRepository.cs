using UxCarrier.Data;
using UxCarrier.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UxCarrier.Models.Dto;
using UxCarrier.Models.Entities;
using System.Transactions;

namespace UxCarrier.Repository
{
    public class UxMemberRepository : RepositoryBase<ApplicationDbContext, UxCard>, IUxCardRepository
    {
        public UxMemberRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
