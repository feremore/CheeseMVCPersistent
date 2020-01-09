using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }
        public IActionResult Index()
        {
            List<Menu> menuList = context.Menus.ToList();
            return View(menuList);
        }

        [HttpGet]
        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }
        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {

                Menu newMenu = new Menu
                {

                    Name = addMenuViewModel.Name,

                };
                context.Menus.Add(newMenu);
                context.SaveChanges();
                return Redirect("/Menu/AddItem/" + newMenu.ID);
            }
            return View(addMenuViewModel);
        }
        [HttpGet]
        public IActionResult ViewMenu(int id)
        {
            try
            {
                Menu newMenu =
                        context.Menus.Single(m => m.ID == id);
                List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();

                ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
                {
                    Menu = newMenu,
                    Items = items
                };
                return View(viewMenuViewModel);
            }
            catch (InvalidOperationException)
            {
                return Redirect("/Menu");
            }
            

        }
        
        public IActionResult AddItem(int id)
        {
            try{
                Menu newMenu =
                        context.Menus.Single(m => m.ID == id);
                AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(context.Cheeses.ToList())
                {
                    Menu = newMenu,
                    MenuID = newMenu.ID


                };
                return View(addMenuItemViewModel);
            }
            catch (InvalidOperationException) 
            { return Redirect("/Menu"); }
          
            
        }


        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            
            if (ModelState.IsValid)
            {
                
                IList<CheeseMenu> existingItems = context.CheeseMenus
        .Where(cm => cm.CheeseID == addMenuItemViewModel.CheeseID)
        .Where(cm => cm.MenuID == addMenuItemViewModel.MenuID).ToList();
                if (!existingItems.Any())
                {
                           
                    CheeseMenu cheeseMenu = new CheeseMenu
                    {
                        MenuID = addMenuItemViewModel.MenuID,
                        
                        CheeseID = addMenuItemViewModel.CheeseID,
                        
                    };

                    context.CheeseMenus.Add(cheeseMenu);
                    context.SaveChanges();
                    return Redirect("/Menu/ViewMenu/"+cheeseMenu.MenuID);
                }

            }

            Menu newMenu =
                    context.Menus.Single(m => m.ID == addMenuItemViewModel.MenuID);
            AddMenuItemViewModel nVM = new AddMenuItemViewModel(context.Cheeses.ToList())
            {
                Menu = newMenu,
                MenuID = newMenu.ID


            };

            return View(nVM);
        }
        public IActionResult Remove()
        {
            ViewBag.title = "Remove Menus";
            ViewBag.Menus = context.Menus.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] menuIds)
        {
            foreach (int mId in menuIds)
            {
                Menu theMenus = context.Menus.Single(m => m.ID == mId);
                context.Menus.Remove(theMenus);
            }

            context.SaveChanges();

            return Redirect("/Menu");
        }

    }
}