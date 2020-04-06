using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using TasksBoard.Backend.Infrastructure;
using TasksBoard.Backend.Infrastructure.Security;

namespace TasksBoard.Backend.Features.Users
{
    [Route("user")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.SCHEMES)]
    public class UserController
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public UserController(IMediator mediator, ICurrentUserAccessor currentUserAccessor)
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpGet]
        public async Task<UserEnvelope> GetCurrent()
        {
            var currentUserEmail = _currentUserAccessor.GetCurrentUserEmail();
            return await _mediator.Send(new Details.Query(currentUserEmail));
        }

        [HttpPut]
        public async Task<UserEnvelope> UpdateUser([FromBody]Edit.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}