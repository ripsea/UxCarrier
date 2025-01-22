using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UxCarrier.Data;
using UxCarrier.Models;
using UxCarrier.Repository.IRepository;

namespace UxCarrier.Repository
{
    public class RepositoryBase<TContext, T> : IRepositoryBase<T> where T : class where TContext : DbContext
    {
        protected TContext _context;
        internal DbSet<T> dbSet;
        public RepositoryBase(TContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }
        public IQueryable<T> FindAll() => dbSet.AsNoTracking();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            _context.Set<T>().Where(expression).AsNoTracking();
        public void Create(T entity) => dbSet.Add(entity);
        public void Update(T entity) => dbSet.Update(entity);
        public void Delete(T entity) => dbSet.Remove(entity);

        public (List<T> pagedItems, PagedResult paging) ToPagedList<T>(IQueryable<T> items, PageInfo paging)
        {
            var pagedItems = items;
            if (paging != null)
                pagedItems = pagedItems.Skip(paging.Offset).Take(paging.Limit);

            var totalRows = items.Count();
            var pagedResult = new PagedResult(paging, totalRows);
            return (pagedItems.ToList(), pagedResult);
        }
    }
}
