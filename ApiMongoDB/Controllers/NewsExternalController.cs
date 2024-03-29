﻿using ApiMongoDB.Entities;
using ApiMongoDB.Services;
using ApiMongoDB.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsExternalController : ControllerBase
    {
        private readonly ILogger<UploadController> _logger;
        private readonly NewsService _newsService;

        public NewsExternalController(ILogger<UploadController> logger, NewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        [HttpGet("{page}/{quantity}")]
        public ActionResult<Result<NewsViewModel>> Get(int page, int quantity) => _newsService.Get(page, quantity);

        [HttpGet("{slug}")]
        public ActionResult<NewsViewModel> Get(string slug)
        {
            var news = _newsService.GetBySlug(slug);

            if(news is null)
                return NotFound();

            return news;
        }
    }
}
