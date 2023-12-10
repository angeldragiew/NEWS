using NEWS.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Core.Dtos.News
{
    public class NewsGetDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string CategoryName { get; set; }
        public string Author { get; set; }
        public List<string> Paragraphes { get; set; }
    }
}
