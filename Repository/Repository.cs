using GallUni.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

namespace GallUni.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet { get; set; }

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeproperties = null, bool track = false)
        {
            IQueryable<T> query;
            if(track)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            if (!String.IsNullOrEmpty(includeproperties))
            {
                foreach (var property in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query.Include(property);
                }
            }
            query =query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeproperties = null)
        {
            IQueryable<T> query=dbSet;
            if (filter!=null)
            {
                query = query.Where(filter);
            }
            if(!String.IsNullOrEmpty(includeproperties))
            {
                foreach(var property in includeproperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query.Include(property);
                }
            }
            
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }
    }
}
