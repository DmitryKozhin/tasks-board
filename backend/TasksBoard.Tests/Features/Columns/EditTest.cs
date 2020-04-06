using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Features.Columns;
using TasksBoard.Backend.Features.Tasks;

using Xunit;
using Xunit.Abstractions;

using Edit = TasksBoard.Backend.Features.Columns.Edit;
using Task = System.Threading.Tasks.Task;

namespace TasksBoard.Tests.Features.Columns
{
    public class EditTest : BackendTestBase
    {
        public EditTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Edit_ChangeAllProperties_ColumnWasEdited()
        {
            var userId = await CreateUser();
            var boardId = await ColumnTestHelper.CreateBoard(ContextInjector.WriteContext, userId);
            var existingColumnId =
                await ColumnTestHelper.CreateColumn(ContextInjector.WriteContext, userId, boardId, "header", "color");

            var command = new Edit.Command()
            {
                ColumnId = existingColumnId,
                Column = new Edit.ColumnData()
                {
                    Color = "color1",
                    Header = "header1",
                    OrderNum = 1
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Columns.SingleOrDefaultAsync(d => d.Id == command.ColumnId));

            updated.Should().NotBeNull();
            updated.Header.Should().BeEquivalentTo(command.Column.Header);
            updated.Color.Should().BeEquivalentTo(command.Column.Color);
            updated.OrderNum.Should().Be(command.Column.OrderNum);
        }

        [Fact]
        public async Task Edit_AddTasks_TasksAdded()
        {
            var userId = await CreateUser();
            var boardId = await ColumnTestHelper.CreateBoard(ContextInjector.WriteContext, userId);
            var existingColumnId = 
                await ColumnTestHelper.CreateColumn(ContextInjector.WriteContext, userId, boardId, "header", "color");

            var tasksCount = 3;
            var tasks = await ColumnTestHelper.CreateTasks(ContextInjector.WriteContext, userId, existingColumnId, tasksCount);

            var command = new Edit.Command()
            {
                ColumnId = existingColumnId,
                Column = new Edit.ColumnData()
                {
                    Color = "color1",
                    Header = "header1",
                    OrderNum = 1,
                    AddedTasks = tasks.Select(t => t.Id).ToList()
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Columns
                .Include(t => t.Tasks)
                .SingleOrDefaultAsync(d => d.Id == command.ColumnId));
            
            updated.Should().NotBeNull();
            updated.Tasks.Should().HaveCount(tasksCount);
            var columnTaskIds = updated.Tasks.Select(t => t.Id).ToList();
            foreach (var task in tasks)
                columnTaskIds.Should().Contain(task.Id);
        }

        [Fact]
        public async Task Edit_RemoveTasks_TasksRemoved()
        {
            var userId = await CreateUser();
            var boardId = await ColumnTestHelper.CreateBoard(ContextInjector.WriteContext, userId);
            var existingColumnId =
                await ColumnTestHelper.CreateColumn(ContextInjector.WriteContext, userId, boardId, "header", "color");

            var tasks = await ColumnTestHelper.CreateTasks(ContextInjector.WriteContext, userId, existingColumnId, 3);
            await ColumnTestHelper.AddTasksTo(ContextInjector.WriteContext, existingColumnId, tasks.Select(t => t.Id));

            var command = new Edit.Command()
            {
                ColumnId = existingColumnId,
                Column = new Edit.ColumnData()
                {
                    Color = "color1",
                    Header = "header1",
                    OrderNum = 1,
                    RemovedTasks = new List<Guid>() { tasks[0].Id, tasks[2].Id }
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Columns
                .Include(t => t.Tasks)
                .SingleOrDefaultAsync(d => d.Id == command.ColumnId));

            updated.Should().NotBeNull();
            updated.Tasks.Should().HaveCount(1);
            updated.Tasks.Select(t => t.Id).Should().Contain(tasks[1].Id);
        }
    }
}