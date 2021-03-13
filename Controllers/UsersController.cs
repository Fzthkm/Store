using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewStudio.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace NewStudio.Controllers
{
    public class UsersController : Controller
    {
        private context db;
        public UsersController(context context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u._Username == model.Login);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new User { _Username = model.Login, _Password = model.Password,_Name = model.Name };
                    Role userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                    if (userRole != null)
                        user.Role = userRole;

                    db.Users.Add(user);
                    await db.SaveChangesAsync();

                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u._Username == model.Login && u._Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user._Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Users");
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            User CurrentUser = await db.Users
                .Include(r => r.Role)
                .FirstOrDefaultAsync(u => u._Username == id);
            UpdateViewModel vm = new UpdateViewModel { User = CurrentUser };
            return View(vm);
        }
        [Authorize]
        public async Task<IActionResult> Edit(UpdateViewModel vm)
        {
            vm.User = await db.Users.FirstOrDefaultAsync(u => u._Username == User.Identity.Name);
            if (!String.IsNullOrEmpty(vm.newPassword) && !String.IsNullOrEmpty(vm.oldPassword) && !String.IsNullOrEmpty(vm.confirmPassword))
            {
                if (vm.newPassword == vm.confirmPassword)
                {
                    if (vm.oldPassword == vm.User._Password)
                    {
                        vm.User._Password = vm.newPassword;
                        db.Users.Update(vm.User);
                        db.SaveChanges();
                    }
                }
            }
            if (!String.IsNullOrEmpty(vm.newAdress))
            {
                vm.User.Adress = vm.newAdress;
                db.Users.Update(vm.User);
                db.SaveChanges();
            }
            if (!String.IsNullOrEmpty(vm.newCard))
            {
                vm.User.CreditCard = vm.newCard;
                db.Users.Update(vm.User);
                db.SaveChanges();
            }
            return RedirectToAction("Index","Home");
        }
    }
}
