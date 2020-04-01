using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

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

            public Task<TaskEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}