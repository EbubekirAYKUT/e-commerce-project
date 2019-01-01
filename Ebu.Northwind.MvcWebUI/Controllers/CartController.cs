using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ebu.Northwind.Business;
using Ebu.Northwind.Business.Abstract;
using Ebu.Northwind.Entities.Concrete;
using Ebu.Northwind.MvcWebUI.Models;
using Ebu.Northwind.MvcWebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ebu.Northwind.MvcWebUI.Controllers

{
    
    public class CartController : Controller
    {
        ICartSessionService _cartSessionService;
        ICartService _cartService;
        IProductService _productService;
        public CartController(ICartSessionService cartSessionService,ICartService cartService,IProductService productService)
        {
            _cartSessionService = cartSessionService;
            _cartService = cartService;
            _productService = productService;
        }
        
        public IActionResult AddToCart(int ProductId)
        {
            var productToBeAdded = _productService.GetById(ProductId);
            var cart = _cartSessionService.GetCart();
            _cartService.AddToCart(cart, productToBeAdded);
            _cartSessionService.SetCart(cart);
            TempData.Add("message", String.Format("Your product, {0} , was succesfully added to cart", productToBeAdded.ProductId));
            return RedirectToAction("Index", "Product");
        }
        
        public IActionResult List()
        {
            var Cart = _cartSessionService.GetCart();
            CartSummaryViewModel cartSummaryViewModel = new CartSummaryViewModel
            {
                cart = Cart
            };
            return View(cartSummaryViewModel);
        }

        public IActionResult Remove(int productId)
        {
            var cart = _cartSessionService.GetCart();
            _cartService.RemoveFromCart(cart, productId);
            _cartSessionService.SetCart(cart);
            TempData.Add("message", String.Format("Your product was succesfully removed from cart"));
            return RedirectToAction("List");
        }
        
        public IActionResult Complete()
        {
            var shippingDetailsViewModel = new ShippingDetailsViewModel
            {
                ShippingDetails = new ShippingDetails()
            };
            return View(shippingDetailsViewModel);
        }
        [HttpPost]
        public IActionResult Complete(ShippingDetails shippingDetails)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            TempData.Add("message", String.Format("Thank you {0}, your order is in process", shippingDetails.FirstName));
            return View();
        }
    }
}