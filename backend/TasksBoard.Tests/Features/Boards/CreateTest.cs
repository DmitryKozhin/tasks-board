using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Features.Boards;
using TasksBoard.Backend.Infrastructure.Errors;

using Xunit;
using Xunit.Abstractions;

namespace TasksBoard.Tests.Features.Boards
{
    public class CreateTest : BackendTestBase
    {
        public CreateTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Create_BoardCreated()
        {
            var userId = await CreateUser();
            
            var command = new Create.Command()
            {
                Board = new Create.BoardData()
                {
                    Name = "name"
                }
            };

            await SendAsync(command);

            var created = await ExecuteDbContextAsync(db =>
                db.Boards.SingleOrDefaultAsync(t => t.Name.Equals(command.Board.Name)));

            created.Should().NotBeNull();
            created.Name.Should().BeEquivalentTo(command.Board.Name);
            created.OwnerId.Should().Be(userId);
        }


        [Fact]
        public void Create_UseIncorrectUser_ReturnException()
        {
            var command = new Create.Command()
            {
                Board = new Create.BoardData()
                {
                    Name = "name"
                }
            };

            Func<Task> createBoardFunc = async () => await SendAsync(command);
            createBoardFunc.Should().Throw<RestException>();
        }

        [Fact]
        public async Task Create_UseIncorrectBoardName_ReturnException()
        {
            var userId = await CreateUser();
            var boardName = "name";
            await BoardTestHelper.CreateBoard(ContextInjector.WriteContext, userId, boardName);

            var command = new Create.Command()
            {
                Board = new Create.BoardData()
                {
                    Name = boardName
                }
            };

            Func<Task> createBoardFunc = async () => await SendAsync(command);
            createBoardFunc.Should().Throw<RestException>();
        }
    }
}