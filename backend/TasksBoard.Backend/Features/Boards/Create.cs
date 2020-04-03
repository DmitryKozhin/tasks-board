using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Infrastructure.Context;

namespace TasksBoard.Backend.Features.Boards
{
    public class Create
    {
        public class BoardData
        {
            public string Name { get; set; }
        }

        public class UserDataValidator : AbstractValidator<BoardData>
        {
            public UserDataValidator()
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
                RuleFor(x => x.Board).NotNull().SetValidator(new UserDataValidator());
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
                throw new System.NotImplementedException();
            }
        }
    }
}