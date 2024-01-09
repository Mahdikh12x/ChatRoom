using System.Linq.Expressions;

namespace _01_framework.Domain
{
   public interface IRepository<TKey,T> where T:class
    {
        List<T> GetAll();
        T GetBy(TKey id);
        void Create(T entity);
        void SaveChanges();
        bool IsExist(Expression<Func<T, bool>> expression);
    }

}
