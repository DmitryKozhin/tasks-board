using System;

namespace TasksBoard.Backend.Dtos
{
    public class UpdateTaskDto : CreateTaskDto
    {
        public int OrderNum { get; set; }
    }
}