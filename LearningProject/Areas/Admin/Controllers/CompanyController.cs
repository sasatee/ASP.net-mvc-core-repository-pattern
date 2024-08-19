using Bookstore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Productstore.DataAccess.Repository.IRepository;
using Productstore.Utilities;

namespace Productstore.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles =SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;


        //register our service in DI container
        public CompanyController(IUnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;


        }



        public IActionResult Index()
        {

            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }





        //create/update action method 
        public IActionResult Upsert(int? id)  // Upsert ==> update or create UpdateInsert
        {


            //this means that create action view method is called for creating company if there is no id 
            if (id == null || id == 0)
            {
                //view 
                return View(new Company());
            }

            else
            {
                //this means that update action is called for update company with respective id 

                Company companyObj = _unitOfWork.Company.Get(u => u.CompanyId == id);

                //update
                return View(companyObj);

            }



        }





        //view action method for both update or create
        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {

            if (ModelState.IsValid)
            {
              

                if (companyObj.CompanyId == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);

                }


                _unitOfWork.Save();
                TempData["success"] = "Company has been successfully created";
                return RedirectToAction("Index");



            }
            else
            {
  

                return View(companyObj);


            }


        }



       


        #region  API CAllS
      
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });






        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {


            var companyToBeDeleteObj = _unitOfWork.Company.Get(u => u.CompanyId == id);
            if (companyToBeDeleteObj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });



            }


             _unitOfWork.Company.Remove(companyToBeDeleteObj);

             _unitOfWork.Save();

            return Json(new { success  = true, message = "Delete Successfully" });





            

        }

        #endregion

    }
}
