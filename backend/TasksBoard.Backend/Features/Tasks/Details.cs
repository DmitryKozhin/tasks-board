using System;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using TasksBoard.Backend.Infrastructure.Context;

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
            private TasksBoardContext _context;

            public QueryHandler(IDbContextInjector dbContextInjector)
            {
                _context = dbContextInjector.ReadContext;
            }

            public Task<TaskEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}