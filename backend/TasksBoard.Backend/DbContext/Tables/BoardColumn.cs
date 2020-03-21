using System;

namespace TasksBoard.Backend.DbContext.Tables
{
    public class BoardColumn
    {
        public Guid BoardId { get; set; }
        public Board Board { get; set; }

        public Guid ColumnId { get; set; }
        public Column Column { get; set; }
    }
}