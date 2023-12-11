using NEWS.Core.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Core.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> All();
        Task CreateAsync(CategoryDto model);
        Task<CategoryDto> GetById(int id);
        Task UpdateAsync(CategoryDto model);
        Task Delete(int id);
    }
}
