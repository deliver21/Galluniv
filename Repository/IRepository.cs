using System.Linq.Expressions;

namespace GallUni.Repository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        T Get(Expression<Func<T, bool>> filter, string? includeproperties = null, bool track = false);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeproperties = null);

    }
}
