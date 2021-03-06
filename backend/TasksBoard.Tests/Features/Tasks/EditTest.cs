﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Features.Tasks;

using Xunit;
using Xunit.Abstractions;

namespace TasksBoard.Tests.Features.Tasks
{
    public class EditTest : BackendTestBase
    {
        public EditTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Edit_ChageAllProperties_TaskEdited()
        {
            var userId = await CreateUser();
            var columnId = await TaskTestHelper.CreateColumn(
                ContextInjector.WriteContext, userId, "test_board", "test_column");

            var columnId1 = await TaskTestHelper.CreateColumn(
                ContextInjector.WriteContext, userId, "test_board", "test_column1");

            var existingTaskId =
                await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, columnId, "test", "test_desc");

            var command = new Edit.Command()
            {
                TaskId = existingTaskId,
                Task = new Edit.TaskData()
                {
                    ColumnId = columnId1,
                    Description = "description",
                    Header = "header",
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Tasks.SingleOrDefaultAsync(d => d.Id == command.TaskId));
            var oldColumnWithUpdatedTask = await ExecuteDbContextAsync(db => db.Columns.Include(t => t.Tasks).SingleOrDefaultAsync(d => d.Id == columnId));
            var newColumnWithUpdatedTask = await ExecuteDbContextAsync(db => db.Columns.Include(t => t.Tasks).SingleOrDefaultAsync(d => d.Id == command.Task.ColumnId));

            updated.Should().NotBeNull();
            updated.Header.Should().BeEquivalentTo(command.Task.Header);
            updated.Description.Should().BeEquivalentTo(command.Task.Description);
            updated.OrderNum.Should().Be(0);
            updated.ColumnId.Should().Be(columnId1);

            oldColumnWithUpdatedTask.Tasks.Should().NotContain(updated);
            newColumnWithUpdatedTask.Tasks.Should().Contain(updated);
        }

        [Fact]
        public async Task Edit_AddAssignedUsers_UsersAdded()
        {
            var user1Id = await CreateUser("email", "name", "bio");
            var user2Id = await CreateUser("email1", "name1", "bio1");
            var user3Id = await CreateUser("email2", "name2", "bio2");

            var columnId = await TaskTestHelper.CreateColumn(
                ContextInjector.WriteContext, user1Id, "test_board", "test_column");

            var existingTaskId =
                await TaskTestHelper.CreateTask(ContextInjector.WriteContext, user1Id, columnId, "test", "test_desc");

            var command = new Edit.Command()
            {
                TaskId = existingTaskId,
                Task = new Edit.TaskData()
                {
                    AssignedUsers = new List<string>() { "email1", "email2" }
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Tasks
                .Include(t => t.AssignedUsers)
                .ThenInclude(t => t.User)
                .SingleOrDefaultAsync(d => d.Id == command.TaskId));

            updated.Should().NotBeNull();
            updated.AssignedUsers.Should().HaveCount(3);
            var assignedUserIds = updated.AssignedUsers.Select(t => t.UserId).ToList();
            foreach (var userId in new []{user1Id, user2Id, user3Id})
                assignedUserIds.Should().Contain(userId);
        }

        [Fact]
        public async Task Edit_RemoveAssignedUsers_UsersRemoved()
        {
            var user1Id = await CreateUser("email", "name", "bio");
            var user2Id = await CreateUser("email1", "name1", "bio1");
            var user3Id = await CreateUser("email2", "name2", "bio2");

            var columnId = await TaskTestHelper.CreateColumn(
                ContextInjector.WriteContext, user1Id, "test_board", "test_column");

            var existingTaskId =
                await TaskTestHelper.CreateTask(ContextInjector.WriteContext, user1Id, columnId, "test", "test_desc");

            await TaskTestHelper.AssignUsers(ContextInjector.WriteContext, existingTaskId,
                new List<Guid>() { user2Id, user3Id });

            var command = new Edit.Command()
            {
                TaskId = existingTaskId,
                Task = new Edit.TaskData()
                {
                    UnAssignedUsers = new List<string>() { "email1" }
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Tasks
                .Include(t => t.AssignedUsers)
                .ThenInclude(t => t.User)
                .SingleOrDefaultAsync(d => d.Id == command.TaskId));

            updated.Should().NotBeNull();
            updated.AssignedUsers.Should().HaveCount(2);
            var assignedUserIds = updated.AssignedUsers.Select(t => t.UserId).ToList();
            foreach (var userId in new[] { user1Id, user3Id })
                assignedUserIds.Should().Contain(userId);
        }

        [Fact]
        public async Task Edit_ChangeColumnAndOrder_ColumnAndOrderChanged()
        {
            var userId = await CreateUser();
            var column1Id = await TaskTestHelper.CreateColumn(
                ContextInjector.WriteContext, userId);

            var column2Id = await TaskTestHelper.CreateColumn(
                ContextInjector.WriteContext, userId);

            var task1InFirstColumn =
                await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, column1Id, "test", "test_desc");
            var task2InFirstColumn =
                await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, column1Id, "test1", "test_desc1");
            var task3InFirstColumn =
                await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, column1Id, "test2", "test_desc2");

            var command = new Edit.Command()
            {
                TaskId = task1InFirstColumn,
                Task = new Edit.TaskData()
                {
                    ColumnId = column2Id
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Tasks
                .Include(t => t.Column)
                .ThenInclude(t => t.Tasks)
                .SingleOrDefaultAsync(d => d.Id == command.TaskId));

            var oldColumn = await ExecuteDbContextAsync(db =>
                db.Columns.Include(t => t.Tasks).SingleOrDefaultAsync(t => t.Id == column1Id));

            updated.Should().NotBeNull();
            updated.ColumnId.Should().Be(column2Id);
            updated.OrderNum.Should().Be(0);
            oldColumn.Tasks.Should().HaveCount(2);
            oldColumn.Tasks.SingleOrDefault(t => t.Id == task2InFirstColumn).OrderNum.Should().Be(0);
            oldColumn.Tasks.SingleOrDefault(t => t.Id == task3InFirstColumn).OrderNum.Should().Be(1);
        }

        [Fact]
        public async Task Edit_ChangeOrder_OrderChanged()
        {
            var userId = await CreateUser();
            var column1Id = await TaskTestHelper.CreateColumn(
                ContextInjector.WriteContext, userId);

            var task1InFirstColumn =
                await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, column1Id, "test", "test_desc");
            var task2InFirstColumn =
                await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, column1Id, "test1", "test_desc1");
            var task3InFirstColumn =
                await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, column1Id, "test2", "test_desc2");
            var task4InFirstColumn =
                await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, column1Id, "test3", "test_desc3");

            var command = new Edit.Command()
            {
                TaskId = task1InFirstColumn,
                Task = new Edit.TaskData()
                {
                    OrderNum = 3
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Tasks
                .Include(t => t.Column)
                .ThenInclude(t => t.Tasks)
                .SingleOrDefaultAsync(d => d.Id == command.TaskId));

            updated.Should().NotBeNull();
            updated.OrderNum.Should().Be(command.Task.OrderNum);
            updated.Column.Tasks.SingleOrDefault(t => t.Id == task2InFirstColumn).OrderNum.Should().Be(0);
            updated.Column.Tasks.SingleOrDefault(t => t.Id == task3InFirstColumn).OrderNum.Should().Be(1);
            updated.Column.Tasks.SingleOrDefault(t => t.Id == task4InFirstColumn).OrderNum.Should().Be(2);
        }
    }
}