using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Infrastructure;
using TasksBoard.Backend.Infrastructure.Context;

namespace TasksBoard.Backend.Features.Boards
{
    public class List
    {
        public class Query : IRequest<BoardsEnvelope>
        {
            public string Name { get; }

            public Query(string name)
            {
                Name = name;
            }
        }

        public class QueryHandler : IRequestHandler<Query, BoardsEnvelope>
        {
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IMapper _mapper;
            private readonly TasksBoardContext _context;

            public QueryHandler(IDbContextInjector dbContextInjector, ICurrentUserAccessor currentUserAccessor, 
                IMapper mapper)
            {
                _context = dbContextInjector.ReadContext;
                _currentUserAccessor = currentUserAccessor;
                _mapper = mapper;
            }

            public async Task<BoardsEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUserEmail = _currentUserAccessor.GetCurrentUserEmail();
                var queryable = _context.Boards
                    .Include(t => t.Owner)
                    .AsNoTracking();

                queryable = queryable.Where(t => t.Owner.Email.Equals(currentUserEmail));

                if (!string.IsNullOrEmpty(request.Name))
                    queryable = queryable.Where(t => t.Name.Contains(request.Name));

                var listOfBoards = await queryable.ToListAsync(cancellationToken);
                return new BoardsEnvelope()
                {
                    BoardsCount = listOfBoards.Count,
                    Boards = listOfBoards.Select(t => _mapper.Map<Board, SmallBoardModel>(t)).ToList()
                };
            }
        }
    }
}