using NEWS.Core.Dtos.News;

namespace NEWS.Core.Services.Interfaces
{
    public interface INewsService
    {
        Task CreateAsync(NewsCreateDto dto);
        Task<NewsGetDto> GetById(int id);
        Task<List<NewsGetDto>> GetLatest();
        Task<List<NewsGetDto>> GetTrending();
        Task<NewsGetDto> Details(int id);
        Task<List<NewsGetDto>> GetByCategoryId(int? id = null);
        Task UpdateAsync(NewsUpdateDto model);
        Task DeleteAsync(int id);
    }
}
