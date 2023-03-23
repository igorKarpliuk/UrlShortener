using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UrlShortener.Web.Models;

namespace UrlShortener.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string filePath = @"..\..\UrlShortener\UrlShortener.Data\about.txt";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult About()
        {
            ViewBag.Text = System.IO.File.ReadAllText(filePath);
            return View();
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public IActionResult About(string AboutText)
        {
            System.IO.File.WriteAllText(filePath, AboutText);
            ViewBag.Text = System.IO.File.ReadAllText(filePath);
            return View();
        }
    }
}