using ApiMongoDB.Entities;
using ApiMongoDB.Services;
using ApiMongoDB.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongoDB.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly VideoService _videoService;

        public VideoController(ILogger<NewsController> logger, VideoService videoService)
        {
            _logger = logger;
            _videoService = videoService;
        }

        [HttpGet("{page}/{quantity}")]
        public ActionResult<Result<VideoViewModel>> Get(int page, int quantity) => _videoService.Get(page, quantity);

        [HttpGet("{id:length(24)}", Name = "GetVideos")]
        public ActionResult<VideoViewModel> Get(string id)
        {
            var news = _videoService.Get(id);

            if (news is null)
                return NotFound();

            return news;
        }

        [HttpPost]
        public ActionResult<VideoViewModel> Create(VideoCreateViewModel news)
        {
            var result = _videoService.Create(news);

            return CreatedAtRoute("GetNews", new { id = result.Id!.ToString() }, result);
        }


        [HttpPut("{id:length(24)}")]
        public ActionResult<VideoViewModel> Update(string id, VideoViewModel newsIn)
        {
            var news = _videoService.Get(id);

            if (news is null)
                return NotFound();

            _videoService.Update(id, newsIn);

            return CreatedAtRoute("GetNews", new { id }, newsIn);

        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var news = _videoService.Get(id);

            if (news is null)
                return NotFound();

            _videoService.Remove(news.Id!);

            return Ok("Noticia deletada com sucesso!");
        }
    }
}
