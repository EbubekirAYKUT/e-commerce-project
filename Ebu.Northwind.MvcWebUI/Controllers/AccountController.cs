using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ebu.Northwind.MvcWebUI.Entities;
using Ebu.Northwind.MvcWebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ebu.Northwind.MvcWebUI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IPasswordValidator<ApplicationUser> _passwordValidator;
        private IPasswordHasher<ApplicationUser> _passwordHasher;

        public AccountController(UserManager<ApplicationUser> userManager,
            IPasswordValidator<ApplicationUser> passwordValidator,
            IPasswordHasher<ApplicationUser> passwordHasher,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = registerViewModel.UserName,
                    Email = registerViewModel.Email
                };

                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
                
            }
            return View(registerViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index",_userManager.Users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string Id,string Password,string Email)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user != null)
            {
                user.Email = Email;
                IdentityResult validPass = null;
                if (!String.IsNullOrEmpty(Password))
                {
                    validPass = await _passwordValidator.ValidateAsync(_userManager, user, Password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, Password);
                    }
                    else
                    {
                        foreach (var item in validPass.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                if (validPass.Succeeded)
                {
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(/*returnUrl ?? "/"*/"Index","Product");
                    }
                }
                ModelState.AddModelError("Email", "Invalid Email Or Password");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Product");
        }

        //------------------------------ Role----------------------------------------------
        [Authorize(Roles = "Admin")]
        public IActionResult RoleIndex()
        {
            return View(_roleManager.Roles);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(string name)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleIndex");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
           
                return View(name);
            
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);

            if (role != null)
            {
                var result =await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    TempData.Add("message", "Role was successfully deleted");
                    return RedirectToAction("RoleIndex");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return RedirectToAction("RoleIndex");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);

            var members = new List<ApplicationUser>();
            var nonmembers = new List<ApplicationUser>();

            foreach (var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name)
                    ? members 
                    :nonmembers;

                list.Add(user);
            }

            var model = new RoleDetails
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(RoleEditModel model)
        {
            IdentityResult result;

            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user!=null)
                    {
                        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                foreach (var userId in model.IdToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                    

                }
                
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("RoleIndex");
            }
            else
            {
                return RedirectToAction("EditRole", model.RoleId);
            }

        }




        /* private UserManager<CustomIdentityUser> _userManager;
         private RoleManager<CustomIdentityRole> _roleManager;
         private SignInManager<CustomIdentityUser> _signInManager;

         public AccountController(UserManager<CustomIdentityUser> userManager, RoleManager<CustomIdentityRole> roleManager, SignInManager<CustomIdentityUser> signInManager)
         {
             _userManager = userManager;
             _roleManager = roleManager;
             _signInManager = signInManager;
         }

         public ActionResult Register()
         {
             return View();
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Register(RegisterViewModel registerViewModel)
         {
             if (ModelState.IsValid)
             {
                 CustomIdentityUser user = new CustomIdentityUser
                 {
                     UserName = registerViewModel.UserName,
                     Email = registerViewModel.Email
                 };

                 IdentityResult result =
                     _userManager.CreateAsync(user, registerViewModel.Password).Result;

                 if (result.Succeeded)
                 {
                     if (!_roleManager.RoleExistsAsync("Admin").Result)
                     {
                         CustomIdentityRole role = new CustomIdentityRole
                         {
                             Name = "Admin"
                         };

                         IdentityResult roleResult = _roleManager.CreateAsync(role).Result;

                         if (!roleResult.Succeeded)
                         {
                             ModelState.AddModelError("", "We can't add the role!");
                             return View(registerViewModel);
                         }
                     }

                     _userManager.AddToRoleAsync(user, "Admin").Wait();
                     return RedirectToAction("Login", "Account");
                 }
             }

             return View(registerViewModel);
         }

         public ActionResult Login()
         {
             return View();
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Login(LoginViewModel loginViewModel)
         {
             if (ModelState.IsValid)
             {
                 var result = _signInManager.PasswordSignInAsync(loginViewModel.UserName,
                     loginViewModel.Password, loginViewModel.RememberMe, false).Result;

                 if (result.Succeeded)
                 {
                     return RedirectToAction("Index", "Admin");
                 }

                 ModelState.AddModelError("", "Invalid login!");
             }

             return View(loginViewModel);
         }

         //[HttpPost]
         //[ValidateAntiForgeryToken]
         public ActionResult LogOff()
         {
             _signInManager.SignOutAsync().Wait();
             return RedirectToAction("Login");
         }*/
    }
}