using System.Collections.Generic;

namespace TasksBoard.Backend.Domain
{
    public class Board : BaseIdEntity
    {
        public string Name { get; set; }

        public List<Column> Columns { get; set; }
    }
}