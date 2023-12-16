using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NEWS.Core.Dtos.Category;
using NEWS.Core.Dtos.News;
using NEWS.Core.Services.Interfaces;
using NEWS.Data.Migrations;
using NEWS.Infrastructure.Data.Models;
using NEWS.Infrastructure.Data.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly IRepository<News> _newsRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public NewsService(IRepository<News> newsRepository,
            IRepository<Category> categoryRepository,
             IWebHostEnvironment hostEnvironment,
             IHttpContextAccessor httpContextAccessor,
             UserManager<ApplicationUser> userManager)
        {
            _newsRepository = newsRepository;
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
        }
        public async Task CreateAsync(NewsCreateEditDto dto)
        {
            News news = new News()
            {
                CategoryId = dto.CategoryId,
                Image = UploadFile(dto.Image),
                ApplicationUserId = _userManager.GetUserId(_httpContextAccessor!.HttpContext!.User),
                Date = DateTime.Now,
                Text = dto.Text,
                Title = dto.Title
            };

            await _newsRepository.AddAsync(news);
            await _newsRepository.SaveChangesAsync();
        }
        public async Task UpdateAsync(NewsCreateEditDto dto)
        {
            var existingNew = await _newsRepository.GetByIdAsync(dto.Id);
            if (existingNew == null)
            {
                throw new ArgumentNullException("New not found!");
            }

            if(dto.Image is not null)
            {
                DeleteFile(existingNew.Image);
                existingNew.Image = UploadFile(dto.Image);
            }

            existingNew.Date = DateTime.Now;
            existingNew.Title = dto.Title;
            existingNew.CategoryId = dto.CategoryId;
            existingNew.Text = dto.Text;

            _newsRepository.Update(existingNew);
            await _newsRepository.SaveChangesAsync();
        }
        public async Task<NewsCreateEditDto> GetById(int id)
        {
            var news = await _newsRepository.GetByIdAsync(id);

            if (news == null)
            {
                throw new ArgumentNullException("News not found!");
            }

            return new NewsCreateEditDto
            {
                Id = news.Id,
                Title = news.Title,
                ImageStr = news.Image,
                CategoryId = news.CategoryId,
                Text = news.Text
            };
        }

        public async Task<List<NewsGetDto>> GetLatest()
        {
            var latestNews = await _newsRepository.All()
                .Include(n => n.ApplicationUser)
                .Include(n => n.Category)
                .OrderByDescending(n => n.Date)
                .Take(3)
                .ToListAsync();

            return latestNews.Select(n =>
                new NewsGetDto()
                {
                    Id = n.Id,
                    Author = $"{n.ApplicationUser.FirstName} {n.ApplicationUser.LastName}",
                    CategoryName = n.Category.Name,
                    Date = n.Date.ToString("MM.dd.yyyy"),
                    Image = n.Image,
                    Title = n.Title.Length > 18 ? n.Title.Substring(0, 16) + "..." : n.Title
                }
            ).ToList();
        }

        public async Task<List<NewsGetDto>> GetTrending()
        {
            var latestNews = await _newsRepository.All()
                .Include(n => n.ApplicationUser)
                .Include(n => n.Category)
                .OrderByDescending(n => n.Views)
                .Take(3)
                .ToListAsync();

            return latestNews.Select(n =>
                new NewsGetDto()
                {
                    Id = n.Id,
                    Author = $"{n.ApplicationUser.FirstName} {n.ApplicationUser.LastName}",
                    CategoryName = n.Category.Name,
                    Date = n.Date.ToString("MM.dd.yyyy"),
                    Image = n.Image,
                    Title = n.Title.Length > 18 ? n.Title.Substring(0, 16) + "..." : n.Title
                }
            ).ToList();
        }

        public async Task<NewsGetDto> Details(int id)
        {
            var allAews = await _newsRepository.All()
                .Include(n => n.ApplicationUser)
                .Include(n => n.Category)
                .ToListAsync();

            var detailsNews = allAews.FirstOrDefault(n => n.Id == id);
            ThrowIfCategoryIsLocked(detailsNews.Category);

            var similarNews = allAews
                .Where(n => n.CategoryId == detailsNews.CategoryId && n.Id != detailsNews.Id)
                .Take(3)
                .Select(n=> new SimilarNewsDto()
                {
                    Id = n.Id,
                    Author = $"{n.ApplicationUser.FirstName} {n.ApplicationUser.LastName}",
                    CategoryName = n.Category.Name,
                    Date = n.Date.ToString("MM.dd.yyyy"),
                    Image = n.Image,
                    Title = n.Title
                }).ToList();

            detailsNews.Views += 1;
            _newsRepository.Update(detailsNews);
            await _newsRepository.SaveChangesAsync();

            return new NewsGetDto()
            {
                Id = detailsNews.Id,
                Author = $"{detailsNews.ApplicationUser.FirstName} {detailsNews.ApplicationUser.LastName}",
                CategoryName = detailsNews.Category.Name,
                Date = detailsNews.Date.ToString("MM.dd.yyyy"),
                Image = detailsNews.Image,
                Title = detailsNews.Title,
                Paragraphes = detailsNews.Text.Split(Environment.NewLine).ToList(),
                SimilarNews = similarNews
            };
        }

        public async Task<List<NewsGetDto>> GetByCategoryId(int? id = null)
        {
            if (id == null)
            {
                var firstCat = await _categoryRepository.All()
                    .FirstOrDefaultAsync();
                id = firstCat?.Id;
            }

            var category = await _categoryRepository.All()
                    .FirstOrDefaultAsync(c=>c.Id==id);
            ThrowIfCategoryIsLocked(category);

            var news = await _newsRepository.All()
                .Include(n => n.ApplicationUser)
                .Include(n => n.Category)
                 .Where(n => n.CategoryId == (id ?? 0))
                 .ToListAsync();


            return news.Select(n =>
                new NewsGetDto()
                {
                    Id = n.Id,
                    Author = $"{n.ApplicationUser.FirstName} {n.ApplicationUser.LastName}",
                    CategoryName = n.Category.Name,
                    Date = n.Date.ToString("MM.dd.yyyy"),
                    Image = n.Image,
                    Title = n.Title.Length > 18 ? n.Title.Substring(0, 16) + "..." : n.Title,
                    Paragraphes = n.Text.Split(Environment.NewLine).ToList()
                }
            ).ToList();
        }

        public async Task DeleteAsync(int id)
        {
            var existingNew = await _newsRepository.GetByIdAsync(id);

            if (existingNew == null)
            {
                throw new ArgumentNullException("Unknown new!");
            }

            _newsRepository.Delete(existingNew);
            await _newsRepository.SaveChangesAsync();
        }

        private string UploadFile(IFormFile file)
        {
            string fileName = null;
            if (file != null)
            {
                string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return fileName;
        }

        private void DeleteFile(string fileName)
        {
            var folderPath = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
            var filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public async Task<List<NewsGetDto>> GetCurrentUserNews()
        {
            var id = _userManager.GetUserId(_httpContextAccessor!.HttpContext!.User);

            var news = await _newsRepository.All()
                .Include(n => n.ApplicationUser)
                .Include(n => n.Category)
                .Where(n => n.ApplicationUserId == id)
                .OrderByDescending(n=>n.Date)
                .ToListAsync();

            return news.Select(n =>
               new NewsGetDto()
               {
                   Id = n.Id,
                   Author = $"{n.ApplicationUser.FirstName} {n.ApplicationUser.LastName}",
                   CategoryName = n.Category.Name,
                   Date = n.Date.ToString("MM.dd.yyyy"),
                   Image = n.Image,
                   Title = n.Title.Length > 18 ? n.Title.Substring(0, 16) + "..." : n.Title,
                   Paragraphes = n.Text.Split(Environment.NewLine).ToList()
               }
           ).ToList();
        }

        private void ThrowIfCategoryIsLocked(Category cat)
        {
            if (cat?.IsLocked ?? false)
            {
                throw new AccessViolationException("Log in to get access to this category!");
            }
        }
    }
}
