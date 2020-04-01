using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Infrastructure;
using TasksBoard.Backend.Infrastructure.Context;
using TasksBoard.Backend.Infrastructure.Security;

namespace TasksBoard.Backend.Features.Users
{
    public class Edit
    {
        public class UserData
        {
            public string Name { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public string Biography { get; set; }

        }

        public class Command : IRequest<UserEnvelope>
        {
            public UserData User { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, UserEnvelope>
        {
            private readonly TasksBoardContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IMapper _mapper;

            public Handler(IDbContextInjector dbContextInjector, IPasswordHasher passwordHasher,
                ICurrentUserAccessor currentUserAccessor, IMapper mapper)
            {
                _context = dbContextInjector.WriteContext;
                _passwordHasher = passwordHasher;
                _currentUserAccessor = currentUserAccessor;
                _mapper = mapper;
            }

            public async Task<UserEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                var currentUsername = _currentUserAccessor.GetCurrentName();
                var user = await _context.Users.Where(x => x.Name == currentUsername).FirstOrDefaultAsync(cancellationToken);

                user.Name = message.User.Name ?? user.Name;
                user.Email = message.User.Email ?? user.Email;
                user.Biography = message.User.Biography ?? user.Biography;

                if (!string.IsNullOrWhiteSpace(message.User.Password))
                {
                    var salt = Guid.NewGuid().ToByteArray();
                    user.Hash = _passwordHasher.Hash(message.User.Password, salt);
                    user.Salt = salt;
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new UserEnvelope(_mapper.Map<User, PublicUser>(user));
            }
        }
    }
}