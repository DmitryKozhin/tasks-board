using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using FluentValidation;

using MediatR;

using TasksBoard.Backend.Infrastructure.Context;

namespace TasksBoard.Backend.Features.Tasks
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Command(Guid taskId)
            {
                TaskId = taskId;
            }

            public Guid TaskId { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.TaskId).NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Command>
        {
            private readonly TasksBoardContext _context;

            public QueryHandler(IDbContextInjector dbContextInjector)
            {
                _context = dbContextInjector.WriteContext;
            }

            public Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}