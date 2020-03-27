using System;

namespace TasksBoard.Backend.Domain
{
    public class Task : BaseIdEntity
    {
        public string Header { get; set; }
        public string Description { get; set; }
        public int OrderNum { get; set; }

        public User Owner { get; set; }

        public Column Column { get; set; }
    }
}