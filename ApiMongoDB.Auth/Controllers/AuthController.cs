using ApiMongoDB.Auth.Models;
using ApiMongoDB.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongoDB.Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly TokenService _tokenService;

        public AuthController(ILogger<AuthController> logger, TokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpPost]
        public ActionResult Post([FromBody] UserViewModel request)
        {
            string token;
            if(_tokenService.IsAuthenticated(request, out token))
            {
                return Ok(token);
            }
            else
            {
                return BadRequest("Usuário ou Senha inválido(s)");
            }
        }
    }
}