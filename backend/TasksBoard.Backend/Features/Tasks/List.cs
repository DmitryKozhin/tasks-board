using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Infrastructure;
using TasksBoard.Backend.Infrastructure.Context;

namespace TasksBoard.Backend.Features.Tasks
{
    public class List
    {
        public class Query : IRequest<TasksEnvelope>
        {
            public Query(List<Guid> taskIds, Guid? assignedUserId, string header)
            {
                TaskIds = taskIds;
                AssignedUserId = assignedUserId;
                Header = header;
            }

            public Guid? AssignedUserId { get; }
            public string Header { get; }

            public List<Guid> TaskIds { get; }
        }

        public class QueryHandler : IRequestHandler<Query, TasksEnvelope>
        {
            private TasksBoardContext _context;

            public QueryHandler(IDbContextInjector dbContextInjector)
            {
                _context = dbContextInjector.ReadContext;
            }

            public async Task<TasksEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                if (!request.AssignedUserId.HasValue && request.TaskIds == null && string.IsNullOrEmpty(request.Header))
                {
                    var tasks = await _context.Tasks.ToListAsync(cancellationToken);
                    return new TasksEnvelope()
                    {
                        Tasks = tasks,
                        TasksCount = tasks.Count
                    };
                }

                var queryable = _context.Tasks
                    .Include(t => t.AssignedUsers)
                    .ThenInclude(t => t.User)
                    .AsNoTracking();

                if (request.AssignedUserId.TryGetValue(out var assignedUserId))
                    queryable = queryable.Where(t => t.AssignedUsers.Select(x => x.UserId).Contains(assignedUserId));

                if (!string.IsNullOrEmpty(request.Header))
                    queryable = queryable.Where(t => t.Header.Contains(request.Header));

                if (request.TaskIds != null)
                    queryable = queryable.Where(t => request.TaskIds.Contains(t.Id));

                var listOfTasks = await queryable.ToListAsync(cancellationToken);
                return new TasksEnvelope()
                {
                    TasksCount = listOfTasks.Count,
                    Tasks = listOfTasks
                };
            }
        }
    }
}