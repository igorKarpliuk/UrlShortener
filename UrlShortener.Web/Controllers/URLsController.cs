using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Models.Identity;
using UrlShortener.Core.Models;
using UrlShortener.Services.Interfaces;

namespace UrlShortener.Web.Controllers
{
    [Authorize]
    public class URLsController : Controller
    {
        private readonly IUrlService _urlService;
        private readonly UserManager<User> _userManager;
        public URLsController(IUrlService urlService, UserManager<User> userManager)
        {
            _urlService = urlService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.CurrentUserId = _userManager.GetUserId(User);
            if (TempData["Error"] != null)
            {
                ViewBag.Error = TempData["Error"].ToString();
            }
            return View(_urlService.GetAllUrls($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}"));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            return View(_urlService.GetUrlByIdForInfo(id));
        }

        [HttpPost]
        public IActionResult AddUrl(string Url)
        {
            if (Url is null || _urlService.GetUrlByUrl(Url) != null)
            {
                TempData["Error"] = "Url is empty or already exists in database!";
                return RedirectToAction(nameof(URLsController.Index), "URLs");
            }
            if(!Uri.TryCreate(Url, UriKind.Absolute, out Uri result))
            {
                TempData["Error"] = "Invalid Url!";
                return RedirectToAction(nameof(URLsController.Index), "URLs");
            }
            User user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            URL url = new URL()
            {
                Url = Url,
                ShortUrl = Url,
                CreationDate = DateTime.Now,
                User = user
            };
            _urlService.AddUrl(url);
            _urlService.AddShortUrlToUrl(url);
            return RedirectToAction(nameof(URLsController.Index), "URLs");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            URL url = _urlService.GetUrlById(id);
            if (url == null)
            {
                TempData["Error"] = "There is no url with this id!";
                return RedirectToAction(nameof(URLsController.Index), "URLs");
            }
            if (url.User.UserName == User.Identity.Name || User.IsInRole("Admin"))
            {
                _urlService.DeleteUrl(url);
                return RedirectToAction(nameof(URLsController.Index), "URLs");
            }
            else
            {
                TempData["Error"] = "You can't delete this Url!";
                return RedirectToAction(nameof(URLsController.Index), "URLs");
            }
        }
    }
}
