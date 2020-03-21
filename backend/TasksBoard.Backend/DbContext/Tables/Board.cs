using System;
using System.Collections.Generic;

namespace TasksBoard.Backend.DbContext.Tables
{
    public class Board : BaseIdEntity
    {
        public string Name { get; set; }
        public List<BoardColumn> Columns { get; set; }
    }
}