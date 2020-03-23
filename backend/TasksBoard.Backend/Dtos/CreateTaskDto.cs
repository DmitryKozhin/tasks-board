using System;

namespace TasksBoard.Backend.Dtos
{
    public class CreateTaskDto
    {
        public string Header { get; set; }
        public string Description { get; set; }

        public Guid? AssignedToId { get; set; }

        public Guid ColumnId { get; set; }
    }
}