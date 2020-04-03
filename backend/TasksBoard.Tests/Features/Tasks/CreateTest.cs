using System;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Features.Tasks;

using Xunit;
using Xunit.Abstractions;

using Task = System.Threading.Tasks.Task;

namespace TasksBoard.Tests.Features.Tasks
{
    public class CreateTest : BackendTestBase
    {
        public CreateTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CreateTask_TaskWasCreatedSuccessfully()
        {
            var userId = await CreateDefaultUser();
            var columnId = await CreateColumn(userId);

            var command = new Create.Command()
            {
                Task = new Create.TaskData()
                {
                    ColumnId = columnId,
                    Description = "description",
                    Header = "header"
                }
            };

            await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Tasks.Where(d => d.Header == command.Task.Header).SingleOrDefaultAsync());

            created.Should().NotBeNull();
            created.Header.Should().BeEquivalentTo(command.Task.Header);
            created.ColumnId.Should().Be(command.Task.ColumnId);
            created.Description.Should().BeEquivalentTo(command.Task.Description);
        }

        private async Task<Guid> CreateColumn(Guid userId)
        {
            var board = new Board() { Name = "test_board", OwnerId = userId };
            await ContextInjector.WriteContext.Boards.AddAsync(board);
            await ContextInjector.WriteContext.SaveChangesAsync();

            var column = new Column() { Header = "test_column", BoardId = board.Id, OwnerId = userId };
            await ContextInjector.WriteContext.Columns.AddAsync(column);
            await ContextInjector.WriteContext.SaveChangesAsync();

            return column.Id;
        }
    }
}