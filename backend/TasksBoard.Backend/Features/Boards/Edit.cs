using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

using TasksBoard.Backend.Infrastructure.Context;

namespace TasksBoard.Backend.Features.Boards
{
    public class Edit
    {
        public class BoardData
        {
            public string Name { get; set; }
            public List<string> Users { get; set; }
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

        public class Handler : IRequestHandler<Create.Command, BoardEnvelope>
        {
            private readonly TasksBoardContext _context;

            public Handler(IDbContextInjector dbContextInjector)
            {
                _context = dbContextInjector.WriteContext;
            }

            public async Task<BoardEnvelope> Handle(Create.Command request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}