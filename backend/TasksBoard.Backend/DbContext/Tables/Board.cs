using System;
using System.Collections.Generic;

using TasksBoard.Common.Entities;

namespace TasksBoard.Backend.DbContext.Tables
{
    public class Board : BaseIdEntity
    {
        public string Name { get; set; }
        public List<BoardColumn> Columns { get; set; }
    }
}