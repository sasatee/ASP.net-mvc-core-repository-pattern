using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Productstore.DataAccess.Repository.IRepository;
using ProductstoreProject.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace Productstore.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet =_db.Set<T>();
            //means _db.Categories == dbSet

            _db.Products.Include(u => u.Category).Include(u => u.CategoryId); // relationship means Product table has relationship on Category table with CategoryId(fK) in Product table 
            
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter,string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var Includeproperty in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {

                    query = query.Include(Includeproperty);


                }
            }

            return query?.FirstOrDefault();
        }
        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var Includeproperty in includeProperties
                    .Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {

                    query = query.Include(Includeproperty);
                
                
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
