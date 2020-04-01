using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Infrastructure.Security;

using Task = System.Threading.Tasks.Task;

namespace TasksBoard.Backend.Features.Tasks
{
    [Route("task")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.SCHEMES)]
    public class TaskController
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<TaskEnvelope> Get(Guid id)
        {
            return await _mediator.Send(new Details.Query(id));
        }

        [HttpGet]
        public async Task<TasksEnvelope> Get([FromQuery] List<Guid> taskIds, [FromQuery] Guid? assignedUserId)
        {
            return await _mediator.Send(new List.Query(taskIds, assignedUserId));
        }

        [HttpPost]
        public async Task<TaskEnvelope> Create([FromBody] Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<TaskEnvelope> Edit(Guid id, [FromBody] Edit.Command command)
        {
            command.TaskId = id;
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _mediator.Send(new Delete.Command(id));
        }
    }
}