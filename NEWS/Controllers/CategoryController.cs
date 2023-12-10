using Microsoft.AspNetCore.Mvc;
using NEWS.Core.Constants;
using NEWS.Core.Dtos.Category;
using NEWS.Core.Services;
using NEWS.Core.Services.Interfaces;

namespace NEWS.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.All();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                TempData[MessageConstant.ErrorMessage] = "Invalid data!";
                var categories = await _categoryService.All();
                return View(categories);
            }

            try
            {
                await _categoryService.CreateAsync(category);
                TempData[MessageConstant.SuccessMessage] = "Category created successfully!";
            }
            catch (Exception)
            {
                TempData[MessageConstant.ErrorMessage] = "Could not create the category!";
            }
            return RedirectToAction("Create", "Category");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.Delete(id);
                //TempData[MessageConstant.SuccessMessage] = "Category deleted successfully!";
            }
            catch (ArgumentNullException ex)
            {
                //TempData[MessageConstant.ErrorMessage] = ex.Message;
            }
            return RedirectToAction("Create", "Category");
        }
    }
}
