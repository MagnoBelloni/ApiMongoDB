using ApiMongoDB.Entities;
using ApiMongoDB.Repository;
using ApiMongoDB.ViewModels;
using AutoMapper;

namespace ApiMongoDB.Services
{
    public class VideoService
    {
        private readonly IMapper _mapper;

        private readonly IMongoRepository<Video> _video;

        public VideoService(IMongoRepository<Video> video, IMapper mapper)
        {
            _mapper = mapper;
            _video = video;
        }

        public Result<VideoViewModel> Get(int page, int quantity) =>
            _mapper.Map<Result<VideoViewModel>>(_video.Get(page, quantity));

        public VideoViewModel Get(string id) =>
           _mapper.Map<VideoViewModel>(_video.Get(id));

        public VideoViewModel GetBySlug(string slug) =>
       _mapper.Map<VideoViewModel>(_video.GetBySlug(slug));


        public VideoViewModel Create(VideoCreateViewModel video)
        {
            var entity = new Video(video.Hat, video.Title, video.Author, video.Thumbnail, video.UrlVideo, video.Status);
            var response = _video.Create(entity);

            return _mapper.Map<VideoViewModel>(response);
        }

        public void Update(string id, VideoViewModel newsIn)
        {
            _video.Update(id, _mapper.Map<Video>(newsIn));
        }

        public void Remove(string id) => _video.Remove(id);

    }
}
