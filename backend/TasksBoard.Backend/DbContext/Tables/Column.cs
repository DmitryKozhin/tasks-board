using System;
using System.Collections.Generic;

namespace TasksBoard.Backend.DbContext.Tables
{
    public class Column : BaseIdEntity
    {
        public string Header { get; set; }
        public string Color { get; set; }
        public int OrderNum { get; set; }

        public List<Guid> TaskIds { get; set; }
        public List<ColumnTask> Tasks { get; set; }

        public Guid BoardId { get; set; }
        public Board Board { get; set; }
    }
}