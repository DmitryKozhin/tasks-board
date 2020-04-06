using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Features.Users;
using TasksBoard.Backend.Infrastructure.Security;

using Xunit;
using Xunit.Abstractions;

namespace TasksBoard.Tests.Features.Users
{
    public class CreateTests : BackendTestBase
    {
        public CreateTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Create_UserCreated()
        {
            var command = new Create.Command()
            {
                User = new Create.UserData()
                {
                    Email = "email",
                    Password = "password",
                    Name = "username"
                }
            };

            await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Users.SingleOrDefaultAsync(d => d.Email == command.User.Email));

            created.Should().NotBeNull();
            created.Email.Should().BeEquivalentTo(command.User.Email);
            created.Name.Should().BeEquivalentTo(command.User.Name);
            created.Hash.Should().BeEquivalentTo(new PasswordHasher().Hash(command.User.Password, created.Salt));
        }
    }
}