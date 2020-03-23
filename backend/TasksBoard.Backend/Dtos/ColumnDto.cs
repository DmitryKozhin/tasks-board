using System.Collections.Generic;

using TasksBoard.Common.Entities;

namespace TasksBoard.Backend.Dtos
{
    public class ColumnDto : BaseIdEntity
    {
        public string Header { get; set; }
        public string Color { get; set; }
        public int OrderNum { get; set; }

        public List<TaskDto> Tasks { get; set; }
        public BoardDto Board { get; set; }
    }
}