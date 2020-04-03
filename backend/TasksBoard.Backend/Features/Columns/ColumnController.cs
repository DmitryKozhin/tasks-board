using System;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TasksBoard.Backend.Infrastructure.Security;

namespace TasksBoard.Backend.Features.Columns
{
    [Route("column")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.SCHEMES)]
    public class ColumnController
    {
        private readonly IMediator _mediator;

        public ColumnController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ColumnEnvelope> Create([FromBody] Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _mediator.Send(new Delete.Command(id));
        }

        [HttpPut("{id}")]
        public async Task<ColumnEnvelope> Edit(Guid id, [FromBody] Edit.Command command)
        {
            command.ColumnId = id;
            return await _mediator.Send(command);
        }

        [HttpGet("{id}")]
        public async Task<ColumnEnvelope> Get(Guid id)
        {
            return await _mediator.Send(new Details.Query(id));
        }
    }
}