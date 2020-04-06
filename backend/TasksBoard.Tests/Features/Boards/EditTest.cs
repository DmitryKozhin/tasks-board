using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Features.Boards;

using Xunit;
using Xunit.Abstractions;

namespace TasksBoard.Tests.Features.Boards
{
    public class EditTest : BackendTestBase
    {
        public EditTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Edit_ChangeNameProperty_BoardEdited()
        {
            var userId = await CreateUser();
            var existingBoardId = await BoardTestHelper.CreateBoard(ContextInjector.WriteContext, userId);

            var command = new Edit.Command()
            {
                BoardId = existingBoardId,
                Board = new Edit.BoardData()
                {
                    Name = "name1"
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Boards.SingleOrDefaultAsync(t => t.Id == existingBoardId));

            updated.Should().NotBeNull();
            updated.Name.Should().BeEquivalentTo(command.Board.Name);
            updated.OwnerId.Should().Be(userId);
        }

        [Fact]
        public async Task Edit_AddUsers_UsersAdded()
        {
            var user1Id = await CreateUser("email", "name", "bio");
            var user2Id = await CreateUser("email1", "name1", "bio1");
            var user3Id = await CreateUser("email2", "name2", "bio2");

            var existingBoardId = await BoardTestHelper.CreateBoard(
                ContextInjector.WriteContext, user1Id);

            var command = new Edit.Command()
            {
                BoardId = existingBoardId,
                Board = new Edit.BoardData()
                {
                    AddedUsers = new List<string>() { "email1", "email2" }
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Boards
                .Include(t => t.UserBoards)
                .ThenInclude(t => t.User)
                .SingleOrDefaultAsync(d => d.Id == command.BoardId));

            updated.Should().NotBeNull();
            updated.UserBoards.Should().HaveCount(3);
            var userBoardIds = updated.UserBoards.Select(t => t.UserId).ToList();
            foreach (var userId in new[] { user1Id, user2Id, user3Id })
                userBoardIds.Should().Contain(userId);
        }

        [Fact]
        public async Task Edit_RemoveUsers_UsersRemoved()
        {
            var user1Id = await CreateUser("email", "name", "bio");
            var user2Id = await CreateUser("email1", "name1", "bio1");
            var user3Id = await CreateUser("email2", "name2", "bio2");

            var existingBoardId = await BoardTestHelper.CreateBoard(
                ContextInjector.WriteContext, user1Id);

            await BoardTestHelper.AddUserBoards(ContextInjector.WriteContext, existingBoardId,
                new List<Guid>() { user2Id, user3Id });

            var command = new Edit.Command()
            {
                BoardId = existingBoardId,
                Board = new Edit.BoardData()
                {
                    RemovedUsers = new List<string>() { "email1" }
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Boards
                .Include(t => t.UserBoards)
                .ThenInclude(t => t.User)
                .SingleOrDefaultAsync(d => d.Id == command.BoardId));

            updated.Should().NotBeNull();
            updated.UserBoards.Should().HaveCount(2);
            var userBoardIds = updated.UserBoards.Select(t => t.UserId).ToList();
            foreach (var userId in new[] { user1Id, user3Id })
                userBoardIds.Should().Contain(userId);
        }

        [Fact]
        public async Task Edit_AddColumns_ColumnsAdded()
        {
            var userId = await CreateUser();
            var existingBoardId = await BoardTestHelper.CreateBoard(
                ContextInjector.WriteContext, userId);

            var columns =
                await BoardTestHelper.CreateColumns(ContextInjector.WriteContext, userId, existingBoardId, 3);

            var command = new Edit.Command()
            {
                BoardId = existingBoardId,
                Board = new Edit.BoardData()
                {
                    AddedColumns = columns.Select(t => t.Id).ToList()
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Boards
                .Include(t => t.Columns)
                .SingleOrDefaultAsync(d => d.Id == command.BoardId));

            updated.Should().NotBeNull();
            updated.Columns.Should().HaveCount(3);
            var boardColumns = updated.Columns.Select(t => t.Id).ToList();
            foreach (var column in columns)
                boardColumns.Should().Contain(column.Id);
        }

        [Fact]
        public async Task Edit_RemoveColumns_ColumnsRemoved()
        {
            var userId = await CreateUser();
            var existingBoardId = await BoardTestHelper.CreateBoard(
                ContextInjector.WriteContext, userId);

            var columns =
                await BoardTestHelper.CreateColumns(ContextInjector.WriteContext, userId, existingBoardId, 3);

            await BoardTestHelper.AddUserBoards(ContextInjector.WriteContext, existingBoardId,
                columns.Select(t => t.Id));

            var command = new Edit.Command()
            {
                BoardId = existingBoardId,
                Board = new Edit.BoardData()
                {
                    RemovedColumns = new List<Guid>() { columns[1].Id }
                }
            };

            await SendAsync(command);

            var updated = await ExecuteDbContextAsync(db => db.Boards
                .Include(t => t.Columns)
                .SingleOrDefaultAsync(d => d.Id == command.BoardId));

            updated.Should().NotBeNull();
            updated.Columns.Should().HaveCount(2);
            var boardColumns = updated.Columns.Select(t => t.Id).ToList();
            foreach (var columnId in new []{ columns[0].Id, columns[0].Id })
                boardColumns.Should().Contain(columnId);
        }
    }
}