using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using TasksBoard.Backend.Infrastructure.Context;
using TasksBoard.Backend.Infrastructure.Errors;

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

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var task = await _context.Tasks.FindAsync(request.TaskId);

                if (task == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Task = Constants.NOT_FOUND });

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}