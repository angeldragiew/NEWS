using NEWS.Core.Dtos.News;

namespace NEWS.Core.Services.Interfaces
{
    public interface INewsService
    {
        Task CreateAsync(NewsCreateEditDto dto);
        Task<NewsCreateEditDto> GetById(int id);
        Task<List<NewsGetDto>> GetLatest();
        Task<List<NewsGetDto>> GetTrending();
        Task<NewsGetDto> Details(int id);
        Task<List<NewsGetDto>> GetByCategoryId(int? id = null);
        Task<List<NewsGetDto>> GetCurrentUserNews();
        Task UpdateAsync(NewsCreateEditDto dto);
        Task DeleteAsync(int id);
    }
}
