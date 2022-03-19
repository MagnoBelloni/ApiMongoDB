using ApiMongoDB.Entities.Enums;

namespace ApiMongoDB.ViewModels
{
    public class NewsCreateViewModel
    {
        public string Hat { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public string Img { get; set; }
        public string Link { get; set; }
        public Status Status { get; set; }
    }
}
