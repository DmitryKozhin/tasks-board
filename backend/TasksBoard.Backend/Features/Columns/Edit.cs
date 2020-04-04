using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Infrastructure;
using TasksBoard.Backend.Infrastructure.Context;
using TasksBoard.Backend.Infrastructure.Errors;

namespace TasksBoard.Backend.Features.Columns
{
    public class Edit
    {
        public class ColumnData
        {
            public string Header { get; set; }
            public string Color { get; set; }
            public int? OrderNum { get; set; }
            public List<Guid> AddedTasks { get; set; }
            public List<Guid> RemovedTasks { get; set; }
        }

        public class Command : IRequest<ColumnEnvelope>
        {
            public ColumnData Column { get; set; }
            public Guid ColumnId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Column).NotNull();
                RuleFor(x => x.ColumnId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, ColumnEnvelope>
        {
            private readonly TasksBoardContext _context;

            public Handler(IDbContextInjector dbContextInjector)
            {
                _context = dbContextInjector.WriteContext;
            }

            public async Task<ColumnEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                var column =
                    await _context.Columns.FirstOrDefaultAsync(t => t.Id == request.ColumnId, cancellationToken);

                if (column == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Column = Constants.NOT_FOUND });

                column.Header = request.Column.Header ?? column.Header;
                column.Color = request.Column.Color ?? column.Color;
                column.OrderNum = request.Column.OrderNum ?? column.OrderNum;

                if (request.Column.RemovedTasks?.Any() == true)
                    await HandleTasks(request.Column.RemovedTasks, t => column.Tasks.Remove(t), cancellationToken);

                if (request.Column.AddedTasks?.Any() == true)
                    await HandleTasks(request.Column.AddedTasks, t => column.Tasks.Add(t), cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return new ColumnEnvelope(column);
            }

            private async Task HandleTasks(List<Guid> taskIds, Action<Domain.Task> handle, CancellationToken cancellationToken)
            {
                var tasks = _context.Tasks.Where(t => taskIds.Contains(t.Id));
                await tasks.ForEachAsync(handle, cancellationToken);
            }
        }
    }
}