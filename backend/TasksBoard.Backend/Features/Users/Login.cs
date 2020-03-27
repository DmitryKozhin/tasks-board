using System.Linq;
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
    public class Login
    {
        public class UserData
        {
            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class UserDataValidator : AbstractValidator<UserData>
        {
            public UserDataValidator()
            {
                RuleFor(x => x.Email).NotNull().NotEmpty();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<UserEnvelope>
        {
            public UserData User { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).NotNull().SetValidator(new UserDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, UserEnvelope>
        {
            private readonly TasksBoardContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IMapper _mapper;

            public Handler(IDbContextInjector dbContextInjector, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
            {
                _context = dbContextInjector.ReadContext;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
                _mapper = mapper;
            }

            public async Task<UserEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                var user = await _context.Users.Where(x => x.Email == message.User.Email).SingleOrDefaultAsync(cancellationToken);
                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });

                if (!user.Hash.SequenceEqual(_passwordHasher.Hash(message.User.Password, user.Salt)))
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });

                var person = _mapper.Map<User, Person>(user);
                person.Token = await _jwtTokenGenerator.CreateToken(person.Name);
                return new UserEnvelope(person);
            }
        }
    }
}