using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using TasksBoard.Backend.Infrastructure.Context;

namespace TasksBoard.Backend.Features.Tasks
{
    public class List
    {
        public class Query : IRequest<TasksEnvelope>
        {
            public Query(List<Guid> taskIds, Guid? assignedUserId, Guid? columnId)
            {
                TaskIds = taskIds;
                AssignedUserId = assignedUserId;
                ColumnId = columnId;
            }

            public Guid? AssignedUserId { get; }

            public Guid? ColumnId { get; }
            public List<Guid> TaskIds { get; }
        }

        public class QueryHandler : IRequestHandler<Query, TasksEnvelope>
        {
            private TasksBoardContext _context;

            public QueryHandler(IDbContextInjector dbContextInjector)
            {
                _context = dbContextInjector.ReadContext;
            }

            public Task<TasksEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}