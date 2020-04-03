using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Infrastructure;
using TasksBoard.Backend.Infrastructure.Context;
using TasksBoard.Backend.Infrastructure.Errors;

namespace TasksBoard.Backend.Features.Boards
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Command(Guid boardId)
            {
                BoardId = boardId;
            }

            public Guid BoardId { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.BoardId).NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Command>
        {
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly TasksBoardContext _context;

            public QueryHandler(IDbContextInjector dbContextInjector, ICurrentUserAccessor currentUserAccessor)
            {
                _currentUserAccessor = currentUserAccessor;
                _context = dbContextInjector.WriteContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var board = await _context.Boards
                    .Include(t => t.Owner)
                    .SingleOrDefaultAsync(t => t.Id == request.BoardId, cancellationToken);

                if (board == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Board = Constants.NOT_FOUND });

                if (!board.Owner.Email.Equals(_currentUserAccessor.GetCurrentUserEmail()))
                    throw new RestException(HttpStatusCode.BadRequest, new { User = Constants.NO_OWNER });

                _context.Boards.Remove(board);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}