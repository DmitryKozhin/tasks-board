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
using TasksBoard.Backend.Infrastructure.Context;
using TasksBoard.Backend.Infrastructure.Errors;

using Task = System.Threading.Tasks.Task;

namespace TasksBoard.Backend.Features.Boards
{
    public class Edit
    {
        public class BoardData
        {
            public string Name { get; set; }
            public List<string> AddedUsers { get; set; }
            public List<string> RemovedUsers { get; set; }
            public List<Guid> AddedColumns { get; set; }
            public List<Guid> RemovedColumns { get; set; }
        }

        public class Command : IRequest<BoardEnvelope>
        {
            public BoardData Board { get; set; }
            public Guid BoardId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Board).NotNull();
                RuleFor(x => x.BoardId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, BoardEnvelope>
        {
            private readonly TasksBoardContext _context;

            public Handler(IDbContextInjector dbContextInjector)
            {
                _context = dbContextInjector.WriteContext;
            }

            public async Task<BoardEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                //TODO: add check current user.
                var board = await _context.Boards
                    .Include(t => t.Columns)
                    .Include(t => t.UserBoards)
                    .ThenInclude(t => t.User)
                    .SingleOrDefaultAsync(t => t.Id == request.BoardId, cancellationToken);
                
                if (board == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Board = Constants.NOT_FOUND });

                if (!string.IsNullOrEmpty(request.Board.Name))
                    board.Name = request.Board.Name;

                if (request.Board.RemovedColumns.Any())
                    await HandleColumns(request.Board.RemovedColumns, t => board.Columns.Remove(t), cancellationToken);

                if (request.Board.AddedColumns.Any())
                    await HandleColumns(request.Board.AddedColumns, t => board.Columns.Add(t), cancellationToken);

                if (request.Board.AddedUsers.Any())
                    await HandleUsers(request.Board.AddedUsers, 
                        t => board.UserBoards.Add(new UserBoard() { BoardId = request.BoardId, UserId = t.Id }), cancellationToken);

                if (request.Board.RemovedUsers.Any())
                    await HandleUsers(request.Board.RemovedUsers, 
                        t => board.UserBoards.Remove(new UserBoard() { BoardId = request.BoardId, UserId = t.Id }), cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
                return new BoardEnvelope(board);
            }

            private async Task HandleColumns(List<Guid> columnIds, Action<Column> handle, CancellationToken cancellationToken)
            {
                var columns = _context.Columns.Where(t => columnIds.Contains(t.Id));
                await columns.ForEachAsync(handle, cancellationToken);
            }

            private async Task HandleUsers(List<string> userEmails, Action<User> handle, CancellationToken cancellationToken)
            {
                var usersToAdd = _context.Users.Where(t => userEmails.Contains(t.Email));
                await usersToAdd.ForEachAsync(handle, cancellationToken);
            }
        }
    }
}