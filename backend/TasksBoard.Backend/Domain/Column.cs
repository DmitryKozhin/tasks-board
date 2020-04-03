using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TasksBoard.Backend.Domain
{
    public class Column
    {
        public Column()
        {
            Tasks = new HashSet<Task>();
        }

        public Guid Id { get; set; }
        public string Header { get; set; }
        public string Color { get; set; }
        public int OrderNum { get; set; }

        public ICollection<Task> Tasks { get; set; }

        [JsonIgnore]
        public Guid OwnerId { get; set; }
        [JsonIgnore]
        public User Owner { get; set; }

        [JsonIgnore]
        public Guid BoardId { get; set; }
        [JsonIgnore]
        public Board Board { get; set; }
    }
}