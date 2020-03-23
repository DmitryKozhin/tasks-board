using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TasksBoard.Common.Dtos;
using TasksBoard.UsersService.Dtos;

namespace TasksBoard.UsersService.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public Task<UserDto> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task<List<UserDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task Create(CreateUserDto createUserDto)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public Task<UserDto> Update(Guid id, CreateUserDto updateUserDto)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}