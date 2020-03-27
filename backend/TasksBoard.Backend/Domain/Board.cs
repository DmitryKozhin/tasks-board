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
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<Column> Columns { get; set; }

        [JsonIgnore]
        public ICollection<UserBoard> UserBoards { get; set; }
    }
}