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
            var userId = await CreateUser();
            var columnId = await TaskTestHelper.CreateColumn(
                ContextInjector.WriteContext, userId, "test_board", "test_column");

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

            var created = await ExecuteDbContextAsync(db => db.Tasks.SingleOrDefaultAsync(d => d.Header == command.Task.Header));

            created.Should().NotBeNull();
            created.Header.Should().BeEquivalentTo(command.Task.Header);
            created.ColumnId.Should().Be(command.Task.ColumnId);
            created.Description.Should().BeEquivalentTo(command.Task.Description);
        }
    }
}