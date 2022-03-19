using ApiMongoDB.Entities;
using ApiMongoDB.Repository;
using ApiMongoDB.ViewModels;
using AutoMapper;

namespace ApiMongoDB.Services
{
    public class NewsService
    {
        private readonly IMapper _mapper;
        private readonly IMongoRepository<News> _repository;

        public NewsService(IMapper mapper, IMongoRepository<News> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public List<NewsViewModel> Get() =>
            _mapper.Map<List<NewsViewModel>>(_repository.Get());

        public NewsViewModel Get(string id) =>
            _mapper.Map<NewsViewModel>(_repository.Get(id));

        public NewsViewModel GetBySlug(string slug) =>
            _mapper.Map<NewsViewModel>(_repository.GetBySlug(slug));

        public NewsViewModel Create(NewsCreateViewModel news)
        {
            var entity = new News(news.Hat, news.Title, news.Text, news.Author, news.Img, news.Status);
            var response = _repository.Create(entity);
            return _mapper.Map<NewsViewModel>(response);
        }

        public void Update(string id, NewsViewModel news)
        {
            _repository.Update(id, _mapper.Map<News>(news));
        }

        public void Remove(string id) => _repository.Remove(id);
    }
}
