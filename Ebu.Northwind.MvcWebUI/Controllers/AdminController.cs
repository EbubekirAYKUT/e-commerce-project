using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ebu.Northwind.Business.Abstract;
using Ebu.Northwind.Entities.Concrete;
using Ebu.Northwind.MvcWebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ebu.Northwind.MvcWebUI.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        IProductService _productService;
        ICategoryService _categoryService;
        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            ProductListViewModel model = new ProductListViewModel
            {
                Products = _productService.GetAll()

            };
            return View(model);
        }

        public IActionResult Add()
        {
            var model = new ProductAddViewModel
            {
                Product = new Product(),
                Categories=_categoryService.GetAll()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.Add(product);
                TempData.Add("message", "Product was successfully added.");
            }

            return RedirectToAction("Add");
        }

        public IActionResult Update(int ProductId)
        {
            var model = new ProductUpdateViewModel
            {
                Product = _productService.GetById(ProductId),
                Categories = _categoryService.GetAll()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                _productService.Update(product);
                TempData.Add("message", "Product was suceessfully updated");
            }
            return RedirectToAction("Update");
        }

        public IActionResult Delete(int ProductId)
        {
            _productService.Delete(ProductId);
            TempData.Add("message", "Product was suceessfully updated");
            return RedirectToAction("Index");
        }
    }
}