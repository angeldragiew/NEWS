using Microsoft.EntityFrameworkCore;
using NEWS.Core.Dtos;
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
                }).ToListAsync();
        }

        public async Task CreateAsync(CategoryDto model)
        {
            Category category = new Category()
            {
                Name = model.Name,
            };

            await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Category category = await _categoryRepo.All().FirstOrDefaultAsync(e => e.Id == id);

            if (category == null)
            {
                throw new ArgumentNullException("Unknown category!");
            }

            _categoryRepo.Delete(category);
            await _categoryRepo.SaveChangesAsync();
        }
    }
}
