using System;

namespace TasksBoard.Backend.DbContext.Tables
{
    public class ColumnTask
    {
        public Guid ColumnId { get; set; }
        public Column Column { get; set; }

        public Guid TaskId { get; set; }
        public Task Task { get; set; }
    }
}