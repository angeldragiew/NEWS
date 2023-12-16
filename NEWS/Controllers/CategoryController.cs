using Microsoft.AspNetCore.Mvc;
using NEWS.Core.Constants;
using NEWS.Core.Dtos.Category;
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
            ViewBag.Categories = await _categoryService.All();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                TempData[MessageConstant.ErrorMessage] = "Invalid data!";
                ViewBag.Categories = await _categoryService.All();
                return View(category);
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
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var category = await _categoryService.GetById(id);
                if (category == null)
                {
                    TempData[MessageConstant.ErrorMessage] = "Category not found!";
                    return RedirectToAction("Create", "Category");
                }
                return View(category);
            }
            catch (Exception ex)
            {
                TempData[MessageConstant.ErrorMessage] = ex.Message;
                return RedirectToAction("Create", "Category");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryDto category)
        {
            if (!ModelState.IsValid)
            {
                TempData[MessageConstant.ErrorMessage] = "Invalid data!";
                return View(category);
            }

            try
            {
                await _categoryService.UpdateAsync(category);
                TempData[MessageConstant.SuccessMessage] = "Category updated successfully!";
            }
            catch (Exception ex)
            {
                TempData[MessageConstant.ErrorMessage] = ex.Message;
            }
            return RedirectToAction("Create", "Category");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.Delete(id);
                TempData[MessageConstant.SuccessMessage] = "Category deleted successfully!";
            }
            catch (ArgumentNullException ex)
            {
                TempData[MessageConstant.ErrorMessage] = ex.Message;
            }
            return RedirectToAction("Create", "Category");
        }
    }
}
