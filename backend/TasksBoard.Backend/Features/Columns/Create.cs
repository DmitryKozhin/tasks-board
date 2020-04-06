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

namespace TasksBoard.Backend.Features.Columns
{
    public class Create
    {
        public class ColumnData
        {
            public string Header { get; set; }
            public string Color { get; set; }
            public Guid BoardId { get; set; }
        }

        public class ColumnDataValidator : AbstractValidator<ColumnData>
        {
            public ColumnDataValidator()
            {
                RuleFor(x => x.Header).NotNull().NotEmpty();
                RuleFor(x => x.Color).NotNull().NotEmpty();
                RuleFor(x => x.BoardId).NotEmpty();
            }
        }

        public class Command : IRequest<ColumnEnvelope>
        {
            public ColumnData Column { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Column).NotNull().SetValidator(new ColumnDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, ColumnEnvelope>
        {
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly TasksBoardContext _context;

            public Handler(IDbContextInjector dbContextInjector, ICurrentUserAccessor currentUserAccessor)
            {
                _currentUserAccessor = currentUserAccessor;
                _context = dbContextInjector.WriteContext;
            }

            public async Task<ColumnEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                var owner = await _context.Users.SingleOrDefaultAsync(
                    t => t.Email.Equals(_currentUserAccessor.GetCurrentUserEmail()), cancellationToken);

                if (owner == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { User = Constants.NOT_FOUND });

                var board = await _context.Boards.SingleOrDefaultAsync(t => t.Id == request.Column.BoardId,
                    cancellationToken);

                if (board == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { Board = Constants.NOT_FOUND });

                var column = new Column()
                {
                    BoardId = request.Column.BoardId, 
                    Color = request.Column.Color, 
                    Header = request.Column.Header,
                    OwnerId = owner.Id,
                    OrderNum = board.Columns.GetNextOrderNum(),
                };

                await _context.Columns.AddAsync(column, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new ColumnEnvelope(column);
            }
        }
    }
}