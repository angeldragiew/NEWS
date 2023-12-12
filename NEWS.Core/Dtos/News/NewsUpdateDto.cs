namespace NEWS.Core.Dtos.News
{
    public class NewsUpdateDto
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int? CategoryId { get; set; }
        public List<string> Paragraphes { get; set; }
    }
}
