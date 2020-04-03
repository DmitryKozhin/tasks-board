using System;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Features.Tasks;

using Xunit;
using Xunit.Abstractions;

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
            // create column before task.

            var command = new Create.Command()
            {
                Task = new Create.TaskData()
                {
                    ColumnId = Guid.Empty,
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
    }
}