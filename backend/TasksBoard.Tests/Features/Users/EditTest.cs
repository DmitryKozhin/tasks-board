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
    public class EditTest : BackendTestBase
    {
        public EditTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Create_UserWasUpdatedSuccessfully()
        {
            await CreateDefaultUser();

            var command = new Edit.Command
            {
                User = new Edit.UserData
                {
                    Email = "email_1",
                    Password = "password_1",
                    Name = "username_1",
                    Biography = "biography_1"
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db =>
                db.Users.SingleOrDefaultAsync(d => d.Email == command.User.Email));

            updated.Should().NotBeNull();
            updated.Email.Should().BeEquivalentTo(command.User.Email);
            updated.Name.Should().BeEquivalentTo(command.User.Name);
            updated.Biography.Should().BeEquivalentTo(command.User.Biography);
            updated.Hash.Should().BeEquivalentTo(new PasswordHasher().Hash(command.User.Password, updated.Salt));
        }
    }
}