using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TasksBoard.Backend.Domain
{
    public class Task : IOrderableEntity
    {
        public Task()
        {
            AssignedUsers = new HashSet<UserTask>();
        }

        public Guid Id { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public int OrderNum { get; set; }

        [JsonIgnore]
        public Guid OwnerId { get; set; }
        [JsonIgnore]
        public User Owner { get; set; }

        public List<Comment> Comments { get; set; }

        [JsonIgnore]
        public Guid ColumnId { get; set; }
        [JsonIgnore]
        public Column Column { get; set; }

        [JsonIgnore]
        public ICollection<UserTask> AssignedUsers { get; set; }
    }
}