using System.Collections.Generic;

using TasksBoard.Backend.Domain;

namespace TasksBoard.Backend.Features.Tasks
{
    public class TasksEnvelope
    {
        public List<Task> Tasks { get; set; }
        public int TasksCount { get; set; }
    }
}