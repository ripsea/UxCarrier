using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;
using UxCarrier.Models.Dto;
using UxCarrier.Models.Entities;

namespace UxCarrier.Repository.IRepository
{
    public interface IUxCardRepository : IRepositoryBase<UxCard>
    {
    }
}
