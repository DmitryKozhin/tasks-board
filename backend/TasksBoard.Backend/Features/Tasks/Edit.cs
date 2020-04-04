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
            public List<string> AssignedUsers { get; set; }
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

                if (request.Task.AssignedUsers != null)
                {
                    var neededUsers = _context.Users.Where(t => request.Task.AssignedUsers.Contains(t.Email));
                    var assignedUsers =
                        neededUsers.Select(t => new UserTask() { TaskId = task.Id, UserId = t.Id })
                            .ToList();

                    var unAssignedUsers = _context.UserTasks.Except(assignedUsers);

                    _context.UserTasks.RemoveRange(unAssignedUsers);
                    await _context.UserTasks.AddRangeAsync(assignedUsers, cancellationToken);
                }

                task.Header = request.Task.Header ?? task.Header;
                task.Description = request.Task.Description ?? task.Description;
                task.OrderNum = request.Task.OrderNum ?? task.OrderNum;

                if (request.Task.ColumnId.TryGetValue(out var columnId) && task.ColumnId != columnId)
                {
                    var oldColumn = await _context.Columns.SingleAsync(t => t.Id == task.ColumnId, cancellationToken);
                    var newColumn = await _context.Columns.SingleAsync(t => t.Id == columnId, cancellationToken);
                    oldColumn.Tasks.Remove(task);
                    newColumn.Tasks.Add(task);

                    task.ColumnId = columnId;
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new TaskEnvelope(task);
            }
        }
    }
}