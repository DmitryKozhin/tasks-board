using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TasksBoard.Backend.Domain
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public int OrderNum { get; set; }

        public User Owner { get; set; }
        public List<Comment> Comments { get; set; }

        [JsonIgnore]
        public Column Column { get; set; }
    }
}