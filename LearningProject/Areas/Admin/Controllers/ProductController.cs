
using Productstore.Models;
using ProductstoreProject.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Productstore.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Bookstore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Productstore.Utilities;

namespace Productstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;


        //register our service in DI container
        public ProductController(IUnitOfWork unitofWork, IWebHostEnvironment IwebHostEnv)
        {
            _unitOfWork = unitofWork;
            _webHostEnvironment = IwebHostEnv;

        }
        public IActionResult Index()
        {

            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            return View(objProductList);
        }



        //create/update action method 
        public IActionResult Upsert(int? id)  // Upsert ==> update or create UpdateInsert
        {

            //Entity framework core projection 
            //retrieve categoryId and its value from our Category object
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category
              .GetAll().Select(u => new SelectListItem
              {
                  Text = u.Name,
                  Value = u.CategoryId.ToString()
              });

            //comment - passing Category List from our controller to our view
            //ViewBag.CategoryListViewBag = CategoryList;

            //ViewData use dictionary (key pair value)
            //ViewData["CategoryListViewData"] = CategoryList;


            ProductVM productVM = new ProductVM()
            {
                CategoryList = CategoryList,
                Product = new Product()
            };

            //this means that create action view method is called for creating product if there is no id 
            if (id == null || id == 0)
            {
                //view 
                return View(productVM);
            }

            else
            {
                //this means that update action is called for update product with respective id 

                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);

                //update
                return View(productVM);

            }



        }



        //view action method for both update or create
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath; // path for wwwRoot image
                if (file != null)
                {

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // eg: random id + .png / .jpeg ect ..
                    string productPath = Path.Combine(wwwRootPath, @"images\product"); // file path

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {

                        //if path exist 
                        // delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }




                    }


                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;

                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);

                }







                _unitOfWork.Save();
                TempData["success"] = "Product has been successfully created";
                return RedirectToAction("Index");










            }
            else
            {

                //list 
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(
                    u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.CategoryId.ToString()
                    });

                return View(productVM);


            }


        }




        //view action method for edit
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


        //remove the delete functionality as i have implement this action method as Api 
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

        //    if (productFromDb == null)
        //    {
        //        return NotFound();

        //    }
        //    return View(productFromDb);



        //}

        ////Delete action method 
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["success"] = "Product has been successfully deleted";
        //    return RedirectToAction("Index");




        //}

        #region  API CAllS
      
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });






        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {


            var productToBeDelete = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });



            }

            //remove image 
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDelete.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

             _unitOfWork.Product.Remove(productToBeDelete);

             _unitOfWork.Save();

            return Json(new { success  = true, message = "Delete Successfully" });





            

        }

        #endregion

    }
}
