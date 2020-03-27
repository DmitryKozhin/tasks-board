using System;

using FluentAssertions;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Features.Users;
using TasksBoard.Backend.Infrastructure.Initializers;
using TasksBoard.Backend.Infrastructure.Security;

using Xunit;
using Xunit.Abstractions;

using Task = System.Threading.Tasks.Task;

namespace TasksBoard.Tests.Features.Users
{
    public class LoginTest : BackendTestBase
    {
        public LoginTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Login_UserWasLoginSuccessfully()
        {
            var salt = Guid.NewGuid().ToByteArray();
            var person = new User
            {
                Name = "username",
                Email = "email",
                Hash = new PasswordHasher().Hash("password", salt),
                Salt = salt
            };
            await InsertAsync(person);

            var command = new Login.Command
            {
                User = new Login.UserData
                {
                    Email = "email",
                    Password = "password"
                }
            };

            var user = await SendAsync(command);
            user.Should().NotBeNull();
            user.User.Should().NotBeNull();
            user.User.Email.Should().BeEquivalentTo(command.User.Email);
            user.User.Name.Should().BeEquivalentTo("username");
            user.User.Token.Should().NotBeEmpty();
        }
    }
}