using System;

namespace TasksBoard.Backend.DbContext.Tables
{
    public class Task : BaseIdEntity
    {
        public string Header { get; set; }
        public string Description { get; set; }

        public Guid? AssignedUserId { get; set; }

        public Guid ColumnId { get; set; }
        public Column Column { get; set; }
    }
}