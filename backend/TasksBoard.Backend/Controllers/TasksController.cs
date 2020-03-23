using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TasksBoard.Backend.Dtos;

namespace TasksBoard.Backend.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;

        public TasksController(ILogger<TasksController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public Task<TaskDto> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Task<List<TaskDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task Create([FromBody] CreateTaskDto createTaskDto)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public Task<TaskDto> Update(Guid id, [FromBody] UpdateTaskDto updateTaskDto)
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