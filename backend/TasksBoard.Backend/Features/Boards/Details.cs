﻿using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Infrastructure.Context;
using TasksBoard.Backend.Infrastructure.Errors;

namespace TasksBoard.Backend.Features.Boards
{
    public class Details
    {
        public class Query : IRequest<BoardEnvelope>
        {
            public Query(Guid boardId)
            {
                BoardId = boardId;
            }

            public Guid BoardId { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.BoardId).NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Query, BoardEnvelope>
        {
            private readonly TasksBoardContext _context;

            public QueryHandler(IDbContextInjector dbContextInjector)
            {
                _context = dbContextInjector.ReadContext;
            }

            public async Task<BoardEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var board = await _context.Boards
                    .Include(t => t.Columns)
                    .ThenInclude(t => t.Tasks)
                    .SingleOrDefaultAsync(t => t.Id == request.BoardId, cancellationToken);

                if (board == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Board = Constants.NOT_FOUND });

                board.Columns = board.Columns.OrderBy(t => t.OrderNum).ToList();
                foreach (var column in board.Columns)
                    column.Tasks = column.Tasks.OrderBy(t => t.OrderNum).ToList();

                return new BoardEnvelope(board);
            }
        }
    }
}