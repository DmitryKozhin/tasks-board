using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Infrastructure;
using TasksBoard.Backend.Infrastructure.Context;

namespace TasksBoard.Backend.Features.Tasks
{
    public class Edit
    {
        public class TaskData
        {
            public string Header { get; set; }
            public string Description { get; set; }
            public Guid? ColumnId { get; set; }
            public int? OrderNum { get; set; }
            public List<Guid> AssignedUserIds { get; set; }
        }

        public class Command : IRequest<TaskEnvelope>
        {
            public TaskData Task { get; set; }
            public Guid TaskId { get; set; }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Task).NotNull();
                RuleFor(x => x.TaskId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, TaskEnvelope>
        {
            private readonly TasksBoardContext _context;

            public Handler(IDbContextInjector dbContextInjector)
            {
                _context = dbContextInjector.WriteContext;
            }

            public async Task<TaskEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                var task = await _context.Tasks.SingleAsync(t => t.Id == request.TaskId, cancellationToken);

                if (request.Task.AssignedUserIds != null)
                {
                    var assignedUsers =
                        request.Task.AssignedUserIds.Select(t => new UserTask() { TaskId = task.Id, UserId = t })
                            .ToList();

                    var unAssignedUsers = _context.UserTasks.Except(assignedUsers);

                    _context.UserTasks.RemoveRange(unAssignedUsers);
                    await _context.UserTasks.AddRangeAsync(assignedUsers, cancellationToken);
                }

                if (request.Task.Header != null)
                    task.Header = request.Task.Header;

                if (request.Task.Description != null)
                    task.Description = request.Task.Description;

                if (request.Task.OrderNum.TryGetValue(out var orderNum))
                    task.OrderNum = orderNum;

                if (request.Task.ColumnId.TryGetValue(out var columnId))
                    task.ColumnId = columnId;

                await _context.SaveChangesAsync(cancellationToken);

                return new TaskEnvelope(task);
            }
        }
    }
}