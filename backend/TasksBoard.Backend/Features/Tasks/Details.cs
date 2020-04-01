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
    public class Details
    {
        public class Query : IRequest<TaskEnvelope>
        {
            public Query(Guid taskId)
            {
                TaskId = taskId;
            }

            public Guid TaskId { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.TaskId).NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Query, TaskEnvelope>
        {
            private readonly TasksBoardContext _context;

            public QueryHandler(IDbContextInjector dbContextInjector)
            {
                _context = dbContextInjector.ReadContext;
            }

            public async Task<TaskEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var task = await _context.Tasks.FindAsync(request.TaskId);

                if (task == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Task = Constants.NOT_FOUND });

                return new TaskEnvelope(task);
            }
        }
    }
}