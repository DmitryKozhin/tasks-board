using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using TasksBoard.Backend.Features.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace TasksBoard.Tests.Features.Tasks
{
    public class ListTest : BackendTestBase
    {
        public ListTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task List_FilterByTaskIds_ReturnListOfTask()
        {
            var userId = await CreateUser();
            var column1Id = await TaskTestHelper.CreateColumn(ContextInjector.WriteContext, userId);
            var column2Id = await TaskTestHelper.CreateColumn(ContextInjector.WriteContext, userId);

            var task1Header = "header";
            var task1Description = "desc";
            var task1Id = await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, column1Id, task1Header,
                task1Description);

            var task2Header = "header2";
            var task2Description = "desc2";
            var task2Id = await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, column2Id, task2Header,
                task2Description);

            var task3Header = "header3";
            var task3Description = "desc3";
            var task3Id = await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, column1Id, task3Header,
                task3Description);

            var query = new List.Query(new List<Guid>(){ task2Id, task3Id }, null, null);
            var tasksEnvelope = await SendAsync(query);

            tasksEnvelope.Should().NotBeNull();
            tasksEnvelope.Tasks.Should().NotBeNull();
            tasksEnvelope.TasksCount.Should().Be(2);
            tasksEnvelope.Tasks.Should().HaveCount(2);

            AssertTask(tasksEnvelope.Tasks.FirstOrDefault(t => t.Id == task2Id), task2Header, task2Description, userId, column2Id);
            AssertTask(tasksEnvelope.Tasks.FirstOrDefault(t => t.Id == task3Id), task3Header, task3Description, userId, column1Id);
        }

        [Fact]
        public async Task List_FilterByAssignedUserId_ReturnListOfTask()
        {
            var user1Id = await CreateUser();
            var user2Id = await CreateUser();
            var column1Id = await TaskTestHelper.CreateColumn(ContextInjector.WriteContext, user1Id);
            var column2Id = await TaskTestHelper.CreateColumn(ContextInjector.WriteContext, user1Id);

            var task1Header = "headerAABB";
            var task1Description = "desc";
            await TaskTestHelper.CreateTask(ContextInjector.WriteContext, user1Id, column1Id, task1Header,
                task1Description);

            var task2Header = "header2BBCC";
            var task2Description = "desc2";
            await TaskTestHelper.CreateTask(ContextInjector.WriteContext, user1Id, column2Id, task2Header,
                task2Description);

            var task3Header = "header3CCDD";
            var task3Description = "desc3";
            await TaskTestHelper.CreateTask(ContextInjector.WriteContext, user1Id, column1Id, task3Header,
                task3Description, new List<Guid>() { user2Id });

            var query = new List.Query(null, user2Id, null);
            var tasksEnvelope = await SendAsync(query);

            tasksEnvelope.TasksCount.Should().Be(1);
            tasksEnvelope.Tasks.Should().HaveCount(1);
        }

        private void AssertTask(Backend.Domain.Task task, string header, string description, Guid userId, Guid columnId)
        {
            task.Should().NotBeNull();
            task.OwnerId.Should().Be(userId);
            task.ColumnId.Should().Be(columnId);
            task.Header.Should().BeEquivalentTo(header);
            task.Description.Should().BeEquivalentTo(description);
        }
    }
}