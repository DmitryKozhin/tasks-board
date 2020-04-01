using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TasksBoard.Backend.Infrastructure.Security;

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
    }
}