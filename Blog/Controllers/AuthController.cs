using Blog.Models.Data;
using Blog.Models.Entity;
using Blog.ViewModels.Auth.Login;
using Blog.ViewModels.Auth.Register;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class AuthController : Controller
    {
        private readonly DatabaseContext _context;

        public AuthController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel user) 
        {
            if (ModelState.IsValid) { 
                if(_context.Users.Any(x=> x.Username.Equals(user.Username) && x.Password.Equals(user.Password)))
                {
                    HttpContext.Session.SetString("user", user.Username);
                    return RedirectToAction("Index", "Home");
                } else
                {
                    ModelState.AddModelError("", "Böyle Bir Kullanıcı Bulunamadı");
                }
            }
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel user)
        {
            if (ModelState.IsValid)
            {
                if (!_context.Users.Any(x => x.Username.Equals(user.Username)))
                {
                    var newUser = new User(user.Username, user.Password);
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    TempData["message"] = "Başarıyla Kayıt Olundu.";
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    ModelState.AddModelError("", "Böyle Bir Kullanıcı Zaten Mevcut");
                }
            }
            return View();
        }
    }
}
