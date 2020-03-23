using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TasksBoard.UsersService.Dtos;

namespace TasksBoard.UsersService.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public AuthController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public Task Login([FromBody] LoginData loginData)
        {
            throw new NotImplementedException();
        }
    }
}