using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TasksBoard.Backend.Domain
{
    public class Column
    {
        public Guid Id { get; set; }
        public string Header { get; set; }
        public string Color { get; set; }
        public int OrderNum { get; set; }

        public List<Task> Tasks { get; set; }

        [JsonIgnore]
        public Board Board { get; set; }
    }
}