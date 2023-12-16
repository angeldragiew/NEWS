using Microsoft.EntityFrameworkCore;
using NEWS.Core.Dtos.Category;
using NEWS.Core.Services.Interfaces;
using NEWS.Infrastructure.Data.Models;
using NEWS.Infrastructure.Data.Repo;

namespace NEWS.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepo;

        public CategoryService(IRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<CategoryDto>> All()
        {
            return await _categoryRepo
                .All()
                .Select(c => new CategoryDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsLocked = c.IsLocked ?? false
                }).ToListAsync();
        }

        public async Task CreateAsync(CategoryDto model)
        {
            Category category = new Category()
            {
                Name = model.Name,
                IsLocked = model.IsLocked
            };

            await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveChangesAsync();
        }

        public async Task<CategoryDto> GetById(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
            {
                throw new ArgumentNullException("Category not found!");
            }

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                IsLocked = category.IsLocked ?? false
            };
        }

        public async Task UpdateAsync(CategoryDto model)
        {
            var existingCategory = await _categoryRepo.GetByIdAsync(model.Id);

            if (existingCategory == null)
            {
                throw new ArgumentNullException("Category not found!");
            }

            existingCategory.Name = model.Name;
            existingCategory.IsLocked = model.IsLocked;

            _categoryRepo.Update(existingCategory);
            await _categoryRepo.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Category category = await _categoryRepo.GetByIdAsync(id);

            if (category == null)
            {
                throw new ArgumentNullException("Unknown category!");
            }

            _categoryRepo.Delete(category);
            await _categoryRepo.SaveChangesAsync();
        }
    }
}
