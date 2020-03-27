using System.Collections.Generic;

using TasksBoard.Backend.DbContext;

namespace TasksBoard.Backend.Dtos
{
    public class BoardDto : BaseIdEntity
    {
        public string Name { get; set; }
        public List<ColumnDto> Columns { get; set; }
    }
}