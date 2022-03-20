using ApiMongoDB.Entities;
using ApiMongoDB.ViewModels;
using AutoMapper;

namespace ApiMongoDB.Config
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<News, NewsViewModel>().ReverseMap();
            CreateMap<Video, VideoViewModel>().ReverseMap();
            CreateMap<Gallery, GalleryViewModel>().ReverseMap();

            CreateMap<Result<Video>, Result<VideoViewModel>>().ReverseMap();
            CreateMap<Result<News>, Result<NewsViewModel>>().ReverseMap();
            CreateMap<Result<Gallery>, Result<GalleryViewModel>>().ReverseMap();
        }
    }
}
