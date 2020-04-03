using System;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using TasksBoard.Backend.Infrastructure.Context;

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
            private readonly TasksBoardContext _context;

            public QueryHandler(IDbContextInjector dbContextInjector)
            {
                _context = dbContextInjector.WriteContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}