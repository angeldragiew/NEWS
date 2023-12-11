using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions;
using NEWS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using NEWS.Core.Dtos.News;

namespace NEWS.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;

        public NewsController(INewsService newsService,
            ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> All(int? id = null)
        {
            ViewBag.Categories = await _categoryService.All();
            var news = await _newsService.GetByCategoryId(id);
            return View(news);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.LatestNews = await _newsService.GetLatest();
            var news = await _newsService.Details(id);
            return View(news);
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.All();
            return View();
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Create([FromForm] NewsCreateDto dto)
        {
            await _newsService.CreateAsync(dto);
            return RedirectToAction("Create", "News");
        }
    }
}
