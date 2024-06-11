using Productstore.DataAccess.Repository.IRepository;
using Productstore.Models;
using ProductstoreProject.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Productstore.DataAccess.Repository
{
    public class ProductRepository:Repository<Product>,IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
             _db.Products.Update(obj);  
        }
    }
}
