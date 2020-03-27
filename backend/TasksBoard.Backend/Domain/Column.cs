using System;
using System.Collections.Generic;

namespace TasksBoard.Backend.Domain
{
    public class Column : BaseIdEntity
    {
        public string Header { get; set; }
        public string Color { get; set; }
        public int OrderNum { get; set; }

        public List<Task> Tasks { get; set; }
        public Board Board { get; set; }
    }
}