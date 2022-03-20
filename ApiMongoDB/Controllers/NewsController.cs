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
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly NewsService _newsService;

        public NewsController(ILogger<NewsController> logger, NewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        [HttpGet("{page}/{quantity}")]
        public ActionResult<Result<NewsViewModel>> Get(int page, int quantity) => _newsService.Get(page, quantity);

        [HttpGet("{id:length(24)}", Name = "GetNews")]
        public ActionResult<NewsViewModel> Get(string id)
        {
            var news = _newsService.Get(id);

            if (news is null)
                return NotFound();

            return news;
        }

        [HttpPost]
        public ActionResult<NewsViewModel> Create(NewsCreateViewModel news)
        {
            var result = _newsService.Create(news);

            return CreatedAtRoute("GetNews", new { id = result.Id.ToString() }, result);
        }


        [HttpPut("{id:length(24)}")]
        public ActionResult<NewsViewModel> Update(string id, NewsViewModel newsIn)
        {
            var news = _newsService.Get(id);

            if (news is null)
                return NotFound();

            _newsService.Update(id, newsIn);

            return CreatedAtRoute("GetNews", new { id }, newsIn);

        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var news = _newsService.Get(id);

            if (news is null)
                return NotFound();

            _newsService.Remove(news.Id);

            return Ok("Noticia deletada com sucesso!");
        }
    }
}