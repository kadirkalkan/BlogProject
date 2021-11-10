using Blog.Models;
using Blog.Models.Data;
using Blog.ViewModels.Home.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Profile()
        {
            List<ArticleViewModel> list =
                _context.Articles
                .Where(x => x.AuthorId.ToString().Equals(HttpContext.Session.GetString("userId")))
                .Select(x => new ArticleViewModel()
                {
                    Id = x.Id,
                    AuthorId = x.AuthorId.ToString(),
                    AuthorName = x.Author.Username,
                    ArticlePicture = x.ArticlePicture,
                    Title = x.Title,
                    Content = x.Content,
                    // todo CreatedTime Eklenecek.
                }).ToList();
            return View(list);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
