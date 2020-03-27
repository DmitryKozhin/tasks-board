using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TasksBoard.Backend.Domain
{
    public class Board
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<Column> Columns { get; set; }

        [JsonIgnore]
        public List<UserBoard> UserBoards { get; set; }
    }
}