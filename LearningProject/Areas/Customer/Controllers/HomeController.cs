using Productstore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Productstore.DataAccess.Repository.IRepository;

namespace Productstore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitofwork)
        {
            _logger = logger;
            _unitOfWork = unitofwork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }

        //detail get action method 
        public IActionResult Details(int? productId)
        {
            Product singleProductDetails = _unitOfWork.Product.Get(u=>u.Id == productId, includeProperties: "Category");
            return View(singleProductDetails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
