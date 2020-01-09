using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace CheeseMVC.ViewModels
{
    
    public class ViewCategoryViewModel
    {
        public CheeseCategory Category { get; set; }
        public IList<Cheese> Items { get; set; }
        public ViewCategoryViewModel()
        {

        }
    }
}
   
