using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Infrastructure.Context;
using TasksBoard.Backend.Infrastructure.Errors;
using TasksBoard.Backend.Infrastructure.Security;

namespace TasksBoard.Backend.Features.Users
{
    public class Details
    {
        public class Query : IRequest<UserEnvelope>
        {
            public Query(string email)
            {
                Email = email;
            }
            public string Email { get; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IRequestHandler<Query, UserEnvelope>
        {
            private readonly TasksBoardContext _context;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IMapper _mapper;

            public QueryHandler(IDbContextInjector dbContextInjector, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
            {
                _context = dbContextInjector.ReadContext;
                _jwtTokenGenerator = jwtTokenGenerator;
                _mapper = mapper;
            }

            public async Task<UserEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == message.Email, cancellationToken);

                if (user == null)
                    throw new RestException(HttpStatusCode.NotFound, new { User = Constants.NOT_FOUND });

                var person = _mapper.Map<User, PublicUser>(user);
                person.Token = await _jwtTokenGenerator.CreateToken(person.Name);
                return new UserEnvelope(person);
            }
        }
    }
}