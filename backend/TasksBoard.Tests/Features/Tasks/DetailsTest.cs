using System;
using System.Threading.Tasks;

using FluentAssertions;

using TasksBoard.Backend.Features.Tasks;
using TasksBoard.Backend.Infrastructure.Errors;

using Xunit;
using Xunit.Abstractions;

namespace TasksBoard.Tests.Features.Tasks
{
    public class DetailsTest : BackendTestBase
    {
        public DetailsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Details_CheckExistingTask_ReturnTask()
        {
            var userId = await CreateUser();
            var columnId = await TaskTestHelper.CreateColumn(ContextInjector.WriteContext, userId);
            var taskHeader = "header";
            var taskDescription = "desc";
            var taskId = await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, columnId, taskHeader,
                taskDescription);

            var query = new Details.Query(taskId);
            var taskEnvelope = await SendAsync(query);

            taskEnvelope.Should().NotBeNull();
            taskEnvelope.Task.Should().NotBeNull();
            taskEnvelope.Task.Header.Should().BeEquivalentTo(taskHeader);
            taskEnvelope.Task.Description.Should().BeEquivalentTo(taskDescription);
            taskEnvelope.Task.ColumnId.Should().Be(columnId);
        }

        [Fact]
        public async Task Details_CheckNotExistingTask_ReturnException()
        {
            var userId = await CreateUser();
            var columnId = await TaskTestHelper.CreateColumn(ContextInjector.WriteContext, userId);
            var taskId = await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, columnId);

            var rndNotExistingGuid = Guid.NewGuid();
            while (rndNotExistingGuid == taskId)
                rndNotExistingGuid = Guid.NewGuid();

            var query = new Details.Query(rndNotExistingGuid);
            Func<Task> getTaskDetailFunc = async () => await SendAsync(query);
            getTaskDetailFunc.Should().Throw<RestException>();
        }
    }
}