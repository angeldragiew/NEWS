using NEWS.Core.Dtos.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Core.Services.Interfaces
{
    public interface INewsService
    {
        Task CreateAsync(NewsCreateDto dto);
        Task<List<NewsGetDto>> GetLatest();
        Task<List<NewsGetDto>> GetTrending();
        Task<NewsGetDto> Details(int id);
    }
}
