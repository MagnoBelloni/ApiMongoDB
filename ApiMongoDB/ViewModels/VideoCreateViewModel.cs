using ApiMongoDB.Entities.Enums;

namespace ApiMongoDB.ViewModels
{
    public class VideoCreateViewModel
    {
        public string Hat { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Thumbnail { get; set; }
        public string UrlVideo { get; set; }
        public string? Slug { get; protected set; }
        public Status Status { get; set; }
    }
}
