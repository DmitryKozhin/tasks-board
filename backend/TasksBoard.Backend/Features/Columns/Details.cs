using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Infrastructure.Context;
using TasksBoard.Backend.Infrastructure.Errors;

namespace TasksBoard.Backend.Features.Columns
{
    public class Details
    {
        public class Query : IRequest<ColumnEnvelope>
        {
            public Query(Guid columnId)
            {
                ColumnId = columnId;
            }

            public Guid ColumnId { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.ColumnId).NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Query, ColumnEnvelope>
        {
            private readonly TasksBoardContext _context;

            public QueryHandler(IDbContextInjector dbContextInjector)
            {
                _context = dbContextInjector.ReadContext;
            }

            public async Task<ColumnEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var column = await _context.Columns
                    .Include(t => t.Tasks)
                    .SingleOrDefaultAsync(t => t.Id == request.ColumnId, cancellationToken);

                if (column == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Column = Constants.NOT_FOUND });

                column.Tasks = column.Tasks.OrderBy(t => t.OrderNum).ToList();

                return new ColumnEnvelope(column);
            }
        }
    }
}