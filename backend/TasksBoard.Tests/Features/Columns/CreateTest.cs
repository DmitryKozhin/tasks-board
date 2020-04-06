using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Features.Columns;
using TasksBoard.Backend.Infrastructure.Errors;

using Xunit;
using Xunit.Abstractions;

namespace TasksBoard.Tests.Features.Columns
{
    public class CreateTest : BackendTestBase
    {
        public CreateTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Create_ColumnWasCreated()
        {
            var userId = await CreateUser();
            var boardId = await ColumnTestHelper.CreateBoard(ContextInjector.WriteContext, userId);
            var command = new Create.Command()
            {
                Column = new Create.ColumnData()
                {
                    Header = "header",
                    Color = "color",
                    BoardId = boardId
                }
            };

            await SendAsync(command);

            var createdColumn = await ExecuteDbContextAsync(db =>
                db.Columns.SingleOrDefaultAsync(t => t.Header.Equals(command.Column.Header)));

            createdColumn.Should().NotBeNull();
            createdColumn.Header.Should().BeEquivalentTo(command.Column.Header);
            createdColumn.Color.Should().BeEquivalentTo(command.Column.Color);
            createdColumn.BoardId.Should().Be(command.Column.BoardId);
        }

        [Fact]
        public async Task Create_UseIncorrectBoard_ReturnException()
        {
            await CreateUser();
            var command = new Create.Command()
            {
                Column = new Create.ColumnData()
                {
                    Header = "header",
                    Color = "color",
                    BoardId = Guid.NewGuid()
                }
            };

            Func<Task> createColumnFunc = async () => await SendAsync(command);
            createColumnFunc.Should().Throw<RestException>();
        }

        [Fact]
        public void Create_UseIncorrectUser_ReturnException()
        {
            var command = new Create.Command()
            {
                Column = new Create.ColumnData()
                {
                    Header = "header",
                    Color = "color",
                    BoardId = Guid.NewGuid()
                }
            };

            Func<Task> createColumnFunc = async () => await SendAsync(command);
            createColumnFunc.Should().Throw<RestException>();
        }
    }
}