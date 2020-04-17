using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using TasksBoard.Backend.Domain;
using TasksBoard.Backend.Infrastructure.Context;

using Task = System.Threading.Tasks.Task;

namespace TasksBoard.Tests.Features.Tasks
{
    public static class TaskTestHelper
    {
        public static async Task AssignUsers(TasksBoardContext context, Guid taskId, List<Guid> userIds)
        {
            var task = await context.Tasks.Include(t => t.AssignedUsers).FirstOrDefaultAsync(t => t.Id == taskId);
            var users = context.Users.Where(t => userIds.Contains(t.Id)).ToList();
            users.ForEach(t => task.AssignedUsers.Add(new UserTask { TaskId = taskId, UserId = t.Id }));
            await context.SaveChangesAsync();
        }

        public static async Task<Guid> CreateColumn(TasksBoardContext context, Guid userId, string boardName = "board",
            string columnHeader = "column")
        {
            var board = new Board { Name = boardName, OwnerId = userId };
            await context.Boards.AddAsync(board);
            await context.SaveChangesAsync();

            var column = new Column { Header = columnHeader, BoardId = board.Id, OwnerId = userId };
            await context.Columns.AddAsync(column);
            await context.SaveChangesAsync();

            return column.Id;
        }

        public static async Task<Guid> CreateTask(TasksBoardContext context, Guid userId, Guid columnId,
            string header = "header",
            string description = "desc",
            List<Guid> assignedUsers = null)
        {
            var task = new Backend.Domain.Task
            {
                ColumnId = columnId,
                OwnerId = userId,
                Header = header,
                Description = description,
                OrderNum = context.Tasks.Any() ? context.Tasks.Max(t => t.OrderNum) + 1 : default
            };

            await context.Tasks.AddAsync(task);
            await context.UserTasks.AddAsync(new UserTask { TaskId = task.Id, UserId = userId });
            assignedUsers?.ForEach(t => context.UserTasks.Add(new UserTask { TaskId = task.Id, UserId = t }));

            await context.SaveChangesAsync();

            return task.Id;
        }
    }
}