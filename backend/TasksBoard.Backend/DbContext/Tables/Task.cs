using System;

using TasksBoard.Common.Entities;

namespace TasksBoard.Backend.DbContext.Tables
{
    public class Task : BaseIdEntity
    {
        public string Header { get; set; }
        public string Description { get; set; }
        public int OrderNum { get; set; }

        public Guid OwnerId { get; set; }
        public Guid? AssignedToId { get; set; }

        public Guid ColumnId { get; set; }
        public Column Column { get; set; }
    }
}