using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Core.Dtos.News
{
    public class SimilarNewsDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string CategoryName { get; set; }
        public string Author { get; set; }
    }
}
