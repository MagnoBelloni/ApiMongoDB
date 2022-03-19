using ApiMongoDB.Entities;
using ApiMongoDB.Services;
using ApiMongoDB.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoExternalController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;
        private readonly VideoService _videoService;

        public VideoExternalController(ILogger<UploadController> logger, VideoService videoService)
        {
            _logger = logger;
            _videoService = videoService;
        }

        [HttpGet("{page}/{quantity}")]
        public ActionResult<Result<VideoViewModel>> Get(int page, int quantity) => _videoService.Get(page, quantity);

        [HttpGet("{slug}")]
        public ActionResult<VideoViewModel> Get(string slug)
        {
            var news = _videoService.GetBySlug(slug);

            if(news is null)
                return NotFound();

            return news;
        }
    }
}
