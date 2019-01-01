using Ebu.Northwind.Business.Abstract;
using Ebu.Northwind.MvcWebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ebu.Northwind.MvcWebUI.Controllers
{
    public class ProductController:Controller
    {
        IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public ActionResult Index(int page=1, int category=0)
        {
            int pagesize = 10;
            var products = _productService.GetByCategory(category);
            ProductListViewModel model = new ProductListViewModel
            {
                Products = products.Skip((page - 1) * pagesize).Take(pagesize).ToList(),
                PageCount = (int)Math.Ceiling(products.Count / (double)pagesize),
                PageSize = pagesize,
                CurrentCategory = category,
                CurrentPage=page
            };
            return View(model);
        }
    }
}
