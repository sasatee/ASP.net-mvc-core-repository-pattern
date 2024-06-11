
using Productstore.Models;
using ProductstoreProject.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Productstore.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Bookstore.Models.ViewModels;

namespace Productstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;

        }
        public IActionResult Index()
        {
        
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
           
            return View(objProductList);
        }
        //get action method for Create
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
              .GetAll().Select(u => new SelectListItem
              {
                  Text = u.Name,
                  Value = u.CategoryId.ToString()
              });
            //ViewBag.CategoryListViewBag = CategoryList;
            //ViewData["CategoryListViewData"] = CategoryList;
            ProductVM productVM = new ProductVM()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };
            return View(productVM);


        }

        //post action method
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product has been successfully created";
                return RedirectToAction("Index");
            }
            else 
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.CategoryId.ToString()
                    });

                return View(productVM); 
            
            
            }


        }


        //update action method 
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // only work finding primary key
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);


          


            if (productFromDb == null)
            {
                return NotFound();

            }
            return View(productFromDb);



        }

        //post action method 
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
    
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj); 
                _unitOfWork.Save();
                TempData["success"] = "Product has been successfully edited";
                return RedirectToAction("Index"); 
            }
            return View(); 


        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();

            }
            return View(productFromDb);



        }

        //Delete action method 
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product has been successfully deleted";
            return RedirectToAction("Index");




        }
    }
}
