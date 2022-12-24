using ApiMongoDB.Entities;
using ApiMongoDB.Repository;
using ApiMongoDB.Services.Interfaces;
using ApiMongoDB.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApiMongoDB.Services
{
    public class GalleryService
    {
        private readonly IMapper _mapper;

        private readonly IMongoRepository<Gallery> _gallery;
        private readonly ICacheService _cacheService;
        private readonly string keyForCache = "gallery";

        public GalleryService(IMongoRepository<Gallery> gallery, IMapper mapper, ICacheService cacheService)
        {
            _mapper = mapper;
            _gallery = gallery;
            _cacheService = cacheService;
        }

        public Result<GalleryViewModel> Get(int page, int qtd)
        {
            var keyCache = $"{keyForCache}/{page}/{qtd}";
            var gallery = _cacheService.Get<Result<GalleryViewModel>>(keyCache);
            if (gallery is null)
            {
                gallery = _mapper.Map<Result<GalleryViewModel>>(_gallery.Get(page, qtd));
                _cacheService.Set(keyCache, gallery);
            }

            return gallery;
        }

        public GalleryViewModel Get(string id)
        {
            var keyCache = $"{keyForCache}/{id}";
            var gallery = _cacheService.Get<GalleryViewModel>(keyCache);
            if (gallery is null)
            {
                gallery = _mapper.Map<GalleryViewModel>(_gallery.Get(id));
                _cacheService.Set(keyCache, gallery);
            }

            return gallery;
        }


        public GalleryViewModel GetBySlug(string slug)
        {
            var keyCache = $"{keyForCache}/{slug}";
            var gallery = _cacheService.Get<GalleryViewModel>(keyCache);
            if (gallery is null)
            {
                gallery = _mapper.Map<GalleryViewModel>(_gallery.GetBySlug(slug));
                _cacheService.Set(keyCache, gallery);
            }

            return gallery;
        }

        public GalleryViewModel Create(GalleryViewModel gallery)
        {
            var entity = new Gallery(gallery.Title, gallery.Legend, gallery.Author, gallery.Tags, gallery.Status, gallery.GalleryImages, gallery.Thumb);

            _gallery.Create(entity);

            var keyCache = $"{keyForCache}/{gallery.Slug}";
            _cacheService.Set(keyCache, gallery);

            return Get(entity.Id);
        }

        public void Update(string id, GalleryViewModel galleryIn)
        {
            var keyCache = $"{keyForCache}/{id}";
            _gallery.Update(id, _mapper.Map<Gallery>(galleryIn));

            _cacheService.Remove(keyCache);
            _cacheService.Set(keyCache, galleryIn);
        }

        public void Remove(string id) 
        {
            var gallery = Get(id);

            _gallery.Remove(id);
            
            var keyCache = $"{keyForCache}/{id}";
            _cacheService.Remove(keyCache);

            keyCache = $"{keyForCache}/{gallery.Slug}";
            _cacheService.Remove(keyCache);
        }
    }
}
