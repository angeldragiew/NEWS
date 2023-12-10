using Microsoft.AspNetCore.Mvc;
using NEWS.Core.Services.Interfaces;
using NEWS.Models;
using System.Diagnostics;

namespace NEWS.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService _newsService;

        public HomeController(INewsService newsService)
        {
            _newsService = newsService;
        }

        public async Task<IActionResult> Index()
        {
            var latestNews = await _newsService.GetLatest();
            ViewBag.TrendingNews = await _newsService.GetTrending();
            return View(latestNews);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}