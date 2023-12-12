using NEWS.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NEWS.Core.Dtos.News
{
    public class NewsCreateEditDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        [StringLength(3000)]
        public string Text { get; set; }

        public IFormFile? Image { get; set; }
        public string? ImageStr { get; set; }

        [Required]
        public int? CategoryId { get; set; }
    }
}
