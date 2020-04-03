using System;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Infrastructure.Security;

using Task = System.Threading.Tasks.Task;

namespace TasksBoard.Backend.Features.Boards
{
    [Route("board")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.SCHEMES)]
    public class BoardController
    {
        private readonly IMediator _mediator;

        public BoardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<BoardEnvelope> Create([FromBody] Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _mediator.Send(new Delete.Command(id));
        }

        [HttpPut("{id}")]
        public async Task<BoardEnvelope> Edit(Guid id, [FromBody] Edit.Command command)
        {
            command.BoardId = id;
            return await _mediator.Send(command);
        }

        [HttpGet("{id}")]
        public async Task<BoardEnvelope> Get(Guid id)
        {
            return await _mediator.Send(new Details.Query(id));
        }
    }
}