using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions;
using NEWS.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using NEWS.Core.Dtos.News;
using NEWS.Core.Constants;

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
        [Authorize]
        public async Task<IActionResult> MyNews()
        {
            var news = await _newsService.GetCurrentUserNews();
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
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.All();
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] NewsCreateEditDto dto)
        {
            if (!ModelState.IsValid || dto.Image is null)
            {
                TempData[MessageConstant.ErrorMessage] = "Invalid data!";
                ViewBag.Categories = await _categoryService.All();
                return View(dto);
            }

            try
            {
                await _newsService.CreateAsync(dto);
                TempData[MessageConstant.SuccessMessage] = "News created successfully!";
                return RedirectToAction("All");
            }
            catch (Exception ex)
            {
                TempData[MessageConstant.ErrorMessage] = ex.Message;
                return View(dto);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var news = await _newsService.GetById(id);
                ViewBag.Categories = await _categoryService.All();
                return View(news);
            }
            catch (Exception ex)
            {
                TempData[MessageConstant.ErrorMessage] = ex.Message;
                return RedirectToAction("MyNews");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(NewsCreateEditDto dto)
        {
            if (!ModelState.IsValid)
            {
                TempData[MessageConstant.ErrorMessage] = "Invalid data!";
                ViewBag.Categories = await _categoryService.All();
                return View(dto);
            }

            try
            {
                await _newsService.UpdateAsync(dto);
                TempData[MessageConstant.SuccessMessage] = "News updated successfully!";
                return RedirectToAction("MyNews");
            }
            catch (Exception ex)
            {
                TempData[MessageConstant.ErrorMessage] = ex.Message;
                return View(dto);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _newsService.DeleteAsync(id);
                TempData[MessageConstant.SuccessMessage] = "News deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData[MessageConstant.ErrorMessage] = ex.Message;
            }

            return RedirectToAction("MyNews");
        }
    }
}
