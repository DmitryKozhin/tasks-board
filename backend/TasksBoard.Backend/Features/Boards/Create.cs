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

namespace TasksBoard.Backend.Features.Boards
{
    public class Create
    {
        public class BoardData
        {
            public string Name { get; set; }
        }

        public class BoardDataValidator : AbstractValidator<BoardData>
        {
            public BoardDataValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<BoardEnvelope>
        {
            public BoardData Board { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Board).NotNull().SetValidator(new BoardDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, BoardEnvelope>
        {
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly TasksBoardContext _context;

            public Handler(IDbContextInjector dbContextInjector, ICurrentUserAccessor currentUserAccessor)
            {
                _currentUserAccessor = currentUserAccessor;
                _context = dbContextInjector.WriteContext;
            }

            public async Task<BoardEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Boards.Where(x => x.Name == request.Board.Name).AnyAsync(cancellationToken))
                    throw new RestException(HttpStatusCode.BadRequest, new { Name = Constants.IN_USE });

                var owner = await _context.Users.SingleAsync(t => t.Email.Equals(_currentUserAccessor.GetCurrentUserEmail()), cancellationToken);
                var board = new Board(){ Name = request.Board.Name, OwnerId = owner.Id };
                await _context.Boards.AddAsync(board, cancellationToken);
                await _context.UserBoards.AddAsync(new UserBoard() { BoardId = board.Id, UserId = owner.Id },
                    cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return new BoardEnvelope(board);
            }
        }
    }
}