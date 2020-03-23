using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TasksBoard.UsersService.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public AuthController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public Task Login()
        {
            throw new NotImplementedException();
        }
    }
}