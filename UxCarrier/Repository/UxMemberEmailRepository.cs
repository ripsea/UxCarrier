using UxCarrier.Data;
using UxCarrier.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UxCarrier.Models.Dto;
using UxCarrier.Models.Entities;

namespace UxCarrier.Repository
{
    public class UxMemberEmailRepository : RepositoryBase<ApplicationDbContext, UxCardEmail>, IUxCardEmailRepository
    {
        public UxMemberEmailRepository(ApplicationDbContext db) : base(db)
        {
        }

    }
}
