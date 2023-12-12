﻿using Microsoft.AspNetCore.Hosting;
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
        public async Task CreateAsync(NewsCreateDto dto)
        {
            var path = UploadFile(dto.Image);
            News news = new News()
            {
                CategoryId = dto.CategoryId,
                Image = path,
                ApplicationUserId = _userManager.GetUserId(_httpContextAccessor!.HttpContext!.User),
                Date = DateTime.Now,
                Text = dto.Text,
                Title = dto.Title
            };

            await _newsRepository.AddAsync(news);
            await _newsRepository.SaveChangesAsync();
        }

        public async Task<NewsGetDto> GetById(int id)
        {
            var news = await _newsRepository.GetByIdAsync(id);

            if (news == null)
            {
                throw new ArgumentNullException("News not found!");
            }

            return new NewsGetDto
            {
                Id = news.Id,
                Date = news.Date.ToString(),
                Title = news.Title,
                Image = news.Image,
                CategoryName = news.Category.Name,
                Author = news.ApplicationUser.FirstName,
                //Paragraphes
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
                    Author = n.ApplicationUser.Email,
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
                    Author = n.ApplicationUser.Email,
                    CategoryName = n.Category.Name,
                    Date = n.Date.ToString("MM.dd.yyyy"),
                    Image = n.Image,
                    Title = n.Title.Length > 18 ? n.Title.Substring(0, 16) + "..." : n.Title
                }
            ).ToList();
        }

        public async Task<NewsGetDto> Details(int id)
        {
            var news = await _newsRepository.All()
                .Include(n => n.ApplicationUser)
                .Include(n => n.Category)
                .FirstOrDefaultAsync(n => n.Id == id);

            news.Views += 1;
            _newsRepository.Update(news);
            await _newsRepository.SaveChangesAsync();

            return new NewsGetDto()
            {
                Id = news.Id,
                Author = news.ApplicationUser.Email,
                CategoryName = news.Category.Name,
                Date = news.Date.ToString("MM.dd.yyyy"),
                Image = news.Image,
                Title = news.Title,
                Paragraphes = news.Text.Split(Environment.NewLine).ToList()
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
            var news = await _newsRepository.All()
                .Include(n => n.ApplicationUser)
                .Include(n => n.Category)
                 .Where(n => n.CategoryId == (id ?? 0))
                 .ToListAsync();

            return news.Select(n =>
                new NewsGetDto()
                {
                    Id = n.Id,
                    Author = n.ApplicationUser.Email,
                    CategoryName = n.Category.Name,
                    Date = n.Date.ToString("MM.dd.yyyy"),
                    Image = n.Image,
                    Title = n.Title.Length > 18 ? n.Title.Substring(0, 16) + "..." : n.Title,
                    Paragraphes = n.Text.Split(Environment.NewLine).ToList()
                }
            ).ToList();
        }

        public async Task UpdateAsync(NewsUpdateDto model)
        {
            var existingNew = await _newsRepository.GetByIdAsync(model.Id);
            if (existingNew == null)
            {
                throw new ArgumentNullException("New not found!");
            }

            existingNew.Date = DateTime.Now;
            existingNew.Title = model.Title;
            existingNew.Image = model.Image;
            existingNew.CategoryId = model.CategoryId;
            existingNew.ApplicationUserId = _userManager.GetUserId(_httpContextAccessor!.HttpContext!.User);
            //Paragraphes 
            
            _newsRepository.Update(existingNew);
            await _newsRepository.SaveChangesAsync();
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
    }
}
