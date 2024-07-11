using Microsoft.EntityFrameworkCore;
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
             var objFromDb = _db.Products.FirstOrDefault(u=>u.Id == obj.Id);
            if (objFromDb != null) 
            {
                objFromDb.Title = obj.Title;    
                objFromDb.Description = obj.Description;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.Price100 = obj.Price100;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Author = obj.Author;
                objFromDb.CategoryId = obj.CategoryId;
                if (obj.ImageUrl != null) 
                { 
                    objFromDb.ImageUrl = obj.ImageUrl;  
                
                }

                
            
            
            
            
            }
        }
    }
}
