using System.Linq.Expressions;
using _01_framework.Domain;
using Microsoft.EntityFrameworkCore;

namespace _01_framework.Infrastructure
{
    public class RepositoryBase<TKey,T>:IRepository<TKey,T> where T : class
    {
        private readonly DbContext _context;

        public RepositoryBase(DbContext context)
        {
            _context = context;
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetBy(TKey id)
        {
            return _context.Find<T>(id);
        }

        public void Create(T entity)
        {
            _context.Add<T>(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public bool IsExist(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Any(expression);
        }
    }
}
