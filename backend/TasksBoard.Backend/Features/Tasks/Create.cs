using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using TasksBoard.Backend.Infrastructure.Context;

namespace TasksBoard.Backend.Features.Tasks
{
    public class Create
    {
        public class TaskData
        {
            public string Header { get; set; }
            public string Description { get; set; }
            public Guid ColumnId { get; set; }
            public List<Guid> AssignedUserIds { get; set; }
        }

        public class UserDataValidator : AbstractValidator<TaskData>
        {
            public UserDataValidator()
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
                RuleFor(x => x.Task).NotNull().SetValidator(new UserDataValidator());
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