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
        public async Task Edit_ChageAllProperty_TaskWasEdited()
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
                    OrderNum = 1
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Tasks.SingleOrDefaultAsync(d => d.Id == command.TaskId));
            var oldColumnWithUpdatedTask = await ExecuteDbContextAsync(db => db.Columns.Include(t => t.Tasks).SingleOrDefaultAsync(d => d.Id == columnId));
            var newColumnWithUpdatedTask = await ExecuteDbContextAsync(db => db.Columns.Include(t => t.Tasks).SingleOrDefaultAsync(d => d.Id == command.Task.ColumnId));

            updated.Should().NotBeNull();
            updated.Header.Should().BeEquivalentTo(command.Task.Header);
            updated.Description.Should().BeEquivalentTo(command.Task.Description);
            updated.OrderNum.Should().Be(command.Task.OrderNum.Value);
            updated.ColumnId.Should().Be(columnId1);

            oldColumnWithUpdatedTask.Tasks.Should().NotContain(updated);
            newColumnWithUpdatedTask.Tasks.Should().Contain(updated);
        }

        [Fact]
        public async Task Edit_ChangeSomeProperty_TaskWasEdited()
        {
            var userId = await CreateUser();
            var columnId = await TaskTestHelper.CreateColumn(
                ContextInjector.WriteContext, userId, "test_board", "test_column");

            var taskDesc = "test_desc";
            var existingTaskId =
                await TaskTestHelper.CreateTask(ContextInjector.WriteContext, userId, columnId, "test", taskDesc);

            var command = new Edit.Command()
            {
                TaskId = existingTaskId,
                Task = new Edit.TaskData()
                {
                    Header = "test1"
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Tasks.SingleOrDefaultAsync(d => d.Id == command.TaskId));

            updated.Should().NotBeNull();
            updated.Header.Should().BeEquivalentTo(command.Task.Header);
            updated.Description.Should().BeEquivalentTo(taskDesc);
            updated.ColumnId.Should().Be(columnId);
        }

        [Fact]
        public async Task Edit_AddAssignedUsers_TaskWasEdited()
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
            updated.AssignedUsers.Select(t => t.UserId).Should().Contain(user1Id);
            updated.AssignedUsers.Select(t => t.UserId).Should().Contain(user2Id);
            updated.AssignedUsers.Select(t => t.UserId).Should().Contain(user3Id);
        }

        [Fact]
        public async Task Edit_RemoveAssignedUsers_TaskWasEdited()
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
            updated.AssignedUsers.Select(t => t.UserId).Should().Contain(user1Id);
            updated.AssignedUsers.Select(t => t.UserId).Should().NotContain(user2Id);
            updated.AssignedUsers.Select(t => t.UserId).Should().Contain(user3Id);
        }
    }
}