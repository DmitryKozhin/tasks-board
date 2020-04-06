using System;
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
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid ColumnId { get; }

            public Command(Guid columnId)
            {
                ColumnId = columnId;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ColumnId).NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Command>
        {
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly TasksBoardContext _context;

            public QueryHandler(IDbContextInjector dbContextInjector, ICurrentUserAccessor currentUserAccessor)
            {
                _currentUserAccessor = currentUserAccessor;
                _context = dbContextInjector.WriteContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var column = await _context.Columns
                    .Include(t => t.Owner)
                    .SingleOrDefaultAsync(t => t.Id == request.ColumnId, cancellationToken);

                if (column == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Column = Constants.NOT_FOUND });

                if (!column.Owner.Email.Equals(_currentUserAccessor.GetCurrentUserEmail()))
                    throw new RestException(HttpStatusCode.BadRequest, new { User = Constants.NO_OWNER });

                _context.Columns.Remove(column);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}