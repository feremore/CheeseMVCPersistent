using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CheeseDbContext context;

        public CategoryController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<CheeseCategory> categoryList = context.Categories.ToList();
            
            return View(categoryList);
        }
        public IActionResult Add()
        {
            AddCategoryViewModel addCategoryViewModel = new AddCategoryViewModel();
            return View(addCategoryViewModel);
        }
        [HttpPost]
        public IActionResult Add(AddCategoryViewModel addCategoryViewModel)
        {
            if (ModelState.IsValid)
            {

                CheeseCategory newCategory = new CheeseCategory
                {

                    Name = addCategoryViewModel.Name,
                    
                };
                context.Categories.Add(newCategory);
                context.SaveChanges();
                return Redirect("/Category");



            }
            return View(addCategoryViewModel);
        }
        public IActionResult Remove()
        {
            ViewBag.title = "Remove Categories";
            ViewBag.categories = context.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] catIds)
        {
            foreach (int cId in catIds)
            {
                CheeseCategory theCategory = context.Categories.Single(c => c.ID == cId);
                context.Categories.Remove(theCategory);
            }

            context.SaveChanges();

            return Redirect("/Category");
        }
        [HttpGet]
        public IActionResult ViewCategory(int id)
        {
            try
            {
                CheeseCategory newCat =
                        context.Categories.Single(c => c.ID == id);
                List<Cheese> items = context
                    .Cheeses
                    .Include(item => item.Category)
                    .Where(cc => cc.CategoryID == id)
                    .ToList();
               
                ViewCategoryViewModel viewCategoryViewModel = new ViewCategoryViewModel
                {
                    Category = newCat,
                    Items = items
                };
                return View(viewCategoryViewModel);
            }
            catch (InvalidOperationException)
            {
                return Redirect("/Category");
            }


        }
    }

}
