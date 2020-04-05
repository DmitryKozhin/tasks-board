using System;
using System.Threading.Tasks;

using FluentAssertions;

using TasksBoard.Backend.Features.Users;
using TasksBoard.Backend.Infrastructure.Errors;

using Xunit;
using Xunit.Abstractions;

namespace TasksBoard.Tests.Features.Users
{
    public class DetailsTest : BackendTestBase
    {
        public DetailsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Details_CheckExistingUser_ReturnUser()
        {
            var userEmail = "email";
            var userName = "name";
            var userBio = "bio";
            await CreateUser(userEmail, userName, userBio);

            var query = new Details.Query(userEmail);
            var user = await SendAsync(query);

            user.Should().NotBeNull();
            user.User.Should().NotBeNull();
            user.User.Email.Should().BeEquivalentTo(userEmail);
            user.User.Name.Should().BeEquivalentTo(userName);
            user.User.Biography.Should().BeEquivalentTo(userBio);
        }

        [Fact]
        public async Task Details_CheckNotExistingUser_ReturnException()
        {
            await CreateUser("email", "name", "bio");

            var query = new Details.Query("otherMail");

            Func<Task> getUserDetailFunc = async () => await SendAsync(query);
            getUserDetailFunc.Should().Throw<RestException>();
        }
    }
}