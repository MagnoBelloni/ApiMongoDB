using ApiMongoDB.Entities;
using ApiMongoDB.ViewModels;
using AutoMapper;

namespace ApiMongoDB.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<News, NewsViewModel>().ReverseMap();
        }
    }
}
