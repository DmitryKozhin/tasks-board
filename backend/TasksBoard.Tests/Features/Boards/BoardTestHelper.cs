using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Infrastructure.Context;

using Task = System.Threading.Tasks.Task;

namespace TasksBoard.Tests.Features.Boards
{
    public static class BoardTestHelper
    {
        public static async Task<Guid> CreateBoard(TasksBoardContext context, Guid userId, string boardName = "board")
        {
            var board = new Board { Name = boardName, OwnerId = userId };

            await context.Boards.AddAsync(board);
            await context.UserBoards.AddAsync(new UserBoard() { BoardId = board.Id, UserId = userId });
            await context.SaveChangesAsync();

            return board.Id;
        }

        public static async Task AddUserBoards(TasksBoardContext context, Guid boardId, IEnumerable<Guid> userIds)
        {
            var board = await context.Boards.Include(t => t.UserBoards).FirstOrDefaultAsync(t => t.Id == boardId);
            var users = context.Users.Where(t => userIds.Contains(t.Id)).ToList();
            users.ForEach(t => board.UserBoards.Add(new UserBoard() { BoardId = boardId, UserId = t.Id }));
            await context.SaveChangesAsync();
        }

        public static async Task<List<Column>> CreateColumns(TasksBoardContext context, Guid userId, Guid boardId, int columnsCount)
        {
            var columns = new List<Column>();
            for (int i = 0; i < columnsCount; i++)
            {
                columns.Add(new Column
                {
                    OwnerId = userId,
                    BoardId = boardId,
                    Header = $"header{i}",
                });
            }

            await context.Columns.AddRangeAsync(columns);
            await context.SaveChangesAsync();

            return columns.ToList();
        }
    }
}