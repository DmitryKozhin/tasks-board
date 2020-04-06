using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TasksBoard.Backend.Domain
{
    public class Board
    {
        public Board()
        {
            UserBoards = new HashSet<UserBoard>();
            Columns = new HashSet<Column>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<Column> Columns { get; set; }

        [JsonIgnore]
        public Guid OwnerId { get; set; }
        [JsonIgnore]
        public User Owner { get; set; }

        [JsonIgnore]
        public ICollection<UserBoard> UserBoards { get; set; }
    }
}