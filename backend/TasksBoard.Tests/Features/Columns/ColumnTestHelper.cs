using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Features.Tasks;
using TasksBoard.Backend.Infrastructure.Context;

using Task = System.Threading.Tasks.Task;

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

        public static async Task<Guid> CreateColumn(TasksBoardContext context, Guid userId, Guid boardId,
            string columnHeader = "header", string columnColor = "color")
        {
            var column = new Column
            {
                Header = columnHeader, 
                BoardId = boardId, 
                OwnerId = userId,
                Color = columnColor
            };

            await context.Columns.AddAsync(column);
            await context.SaveChangesAsync();

            return column.Id;
        }

        public static async Task<List<Backend.Domain.Task>> CreateTasks(TasksBoardContext context, Guid userId, Guid columnId,
            int taskCount)
        {
            var tasks = new List<Backend.Domain.Task>();
            for (int i = 0; i < taskCount; i++)
            {
                tasks.Add(new Backend.Domain.Task
                {
                    ColumnId = columnId,
                    OwnerId = userId,
                    Header = $"header{i}",
                    Description = $"description{i}"
                });
            }

            await context.Tasks.AddRangeAsync(tasks);
            await context.SaveChangesAsync();

            return tasks.ToList();
        }

        public static async Task AddTasksTo(TasksBoardContext context, Guid columnId, IEnumerable<Guid> taskIds)
        {
            var column = await context.Columns.Include(t => t.Tasks).SingleOrDefaultAsync(t => t.Id == columnId);
            var tasks = context.Tasks.Where(t => taskIds.Contains(t.Id));

            await tasks.ForEachAsync(t => column.Tasks.Add(t));
            await context.SaveChangesAsync();
        }
    }
}