using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Features.Users;
using TasksBoard.Backend.Infrastructure.Initializers;
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
        public async Task Create_UserWasCreatedSuccessfully()
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

            var created = await ExecuteDbContextAsync(db => db.Users.Where(d => d.Email == command.User.Email).SingleOrDefaultAsync());

            created.Should().NotBeNull();
            created.Hash.Should().BeEquivalentTo(new PasswordHasher().Hash("password", created.Salt));
        }
    }
}