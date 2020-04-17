using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Infrastructure;
using TasksBoard.Backend.Infrastructure.Context;
using TasksBoard.Backend.Infrastructure.Errors;
using TasksBoard.Backend.Infrastructure.Extensions;

using Task = TasksBoard.Backend.Domain.Task;

namespace TasksBoard.Backend.Features.Tasks
{
    public class Create
    {
        public class TaskData
        {
            public string Header { get; set; }
            public string Description { get; set; }
            public Guid ColumnId { get; set; }
        }

        public class TaskDataValidator : AbstractValidator<TaskData>
        {
            public TaskDataValidator()
            {
                RuleFor(x => x.Header).NotNull().NotEmpty();
                RuleFor(x => x.ColumnId).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<TaskEnvelope>
        {
            public TaskData Task { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Task).NotNull().SetValidator(new TaskDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, TaskEnvelope>
        {
            private readonly ICurrentUserAccessor _userAccessor;
            private readonly TasksBoardContext _context;

            public Handler(IDbContextInjector dbContextInjector, ICurrentUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = dbContextInjector.WriteContext;
            }

            public async Task<TaskEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                var owner = await _context.Users.SingleOrDefaultAsync(t => t.Email.Equals(_userAccessor.GetCurrentUserEmail()), cancellationToken);
                if (owner == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { User = Constants.NOT_FOUND });

                var column = await _context.Columns
                    .Include(t => t.Tasks)
                    .SingleOrDefaultAsync(t => t.Id == request.Task.ColumnId, cancellationToken);
                
                if (column == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Column = Constants.NOT_FOUND });

                var task = new Task()
                {
                    ColumnId = column.Id,
                    Header = request.Task.Header,
                    Description = request.Task.Description,
                    OrderNum = column.Tasks.GetNextOrderNum(),
                    OwnerId = owner.Id,
                };

                await _context.Tasks.AddAsync(task, cancellationToken);
                await _context.UserTasks.AddAsync(new UserTask() { TaskId = task.Id, UserId = owner.Id }, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return new TaskEnvelope(task);
            }
        }
    }
}