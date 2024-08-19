using Bookstore.DataAccess.Repository.IRepository;
using Bookstore.Models;
using Productstore.DataAccess.Repository;
using Productstore.Models;
using ProductstoreProject.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {

        private ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company obj)
        {
            _db.Companies.Update(obj);
           
           
        }
    }
}
