using System;
using System.Threading.Tasks;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Infrastructure.Context;

namespace TasksBoard.Tests.Features.Columns
{
    public static class ColumnTestHelper
    {
        public static async Task<Guid> CreateBoard(TasksBoardContext context, Guid userId, string boardName = "board")
        {
            var board = new Board { Name = boardName, OwnerId = userId };
            await context.Boards.AddAsync(board);
            await context.SaveChangesAsync();

            return board.Id;
        }
    }
}