
using Learning.Models;
using LearningProject.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;

namespace LearningProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public IActionResult Index()
        {
            //tell ef core to get/retrieve obj  such as data save in our database as a list 
            //
            List<Category> objCategory = _db.Categories.ToList();
            return View(objCategory);
        }
        //get action method for Create
        public IActionResult Create() 
        { 
            return View();
        
         
        }

        //post action method
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display order cannot exactly match the Category name");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj); // tell ef core keeps a track of what are all the changes that I have to do in the database
                _db.SaveChanges();// actually go to the database and create that category 
                TempData["success"] = "category has been created successfully";
                return RedirectToAction("Index"); // if obj is validation navigate to Index/Home
            }
           return View(); // if obj is not valid stay on Category/Create


        }


        //update action method 
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // only work finding primary key
            Category? categoryFromDb = _db.Categories.Find(id);


            // works when finding any field e.g want to search on name
            //category? categoryfromdb1 = _db.categories.firstordefault(u => u.name.contains("test"));
            //// find categoryid
            //category? categoryfromdb2 = _db.categories.firstordefault(u => u.categoryid == id);
            //// conditional filtering : for primary key
            //category? categoryfromdb3 = _db.categories.where(u=>u.categoryid == id).firstordefault(); 


            if (categoryFromDb == null) 
            {
                return NotFound();
            
            }
            return View (categoryFromDb);   
  
  

        }

        //post action method 
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
          //remove server side validation 
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj); // this tell ef core keeps  track of update change are to be made that  have to do in the database
                _db.SaveChanges();// actually go to the database and update that category 
                TempData["success"] = "category has been edit successfully";
                return RedirectToAction("Index"); // if obj is validation navigate to Index/Home
            }
            return View(); // if obj is not valid stay on Category/Create


        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            
            Category? categoryFromDb = _db.Categories.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();

            }
            return View(categoryFromDb);



        }

        //Deelete action method 
        [HttpPost,ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "category has been delete successfully";
            return RedirectToAction("Index");




        }
    }
}
