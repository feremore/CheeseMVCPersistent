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
        
        public IActionResult AddItem(int id)
        {
            
            Menu newMenu =
                    context.Menus.Single(m => m.ID == id);
            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(context.Cheeses.ToList())
            {
                menu = newMenu,
                menuID = newMenu.ID
               

            };
            return View(addMenuItemViewModel);
        }


        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            
            if (ModelState.IsValid)
            {
                
                IList<CheeseMenu> existingItems = context.CheeseMenus
        .Where(cm => cm.CheeseID == addMenuItemViewModel.cheeseID)
        .Where(cm => cm.MenuID == addMenuItemViewModel.menuID).ToList();
                if (!existingItems.Any())
                {
                           
                    CheeseMenu cheeseMenu = new CheeseMenu
                    {
                        MenuID = addMenuItemViewModel.menuID,
                        
                        CheeseID = addMenuItemViewModel.cheeseID,
                        
                    };

                    context.CheeseMenus.Add(cheeseMenu);
                    context.SaveChanges();
                    return Redirect("/Menu/ViewMenu/"+cheeseMenu.MenuID);
                }

            }

            Menu newMenu =
                    context.Menus.Single(m => m.ID == addMenuItemViewModel.menuID);
            AddMenuItemViewModel nVM = new AddMenuItemViewModel(context.Cheeses.ToList())
            {
                menu = newMenu,
                menuID = newMenu.ID


            };

            return View(nVM);
        }

    }
}