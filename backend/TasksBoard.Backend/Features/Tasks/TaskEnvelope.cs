using TasksBoard.Backend.Domain;

namespace TasksBoard.Backend.Features.Tasks
{
    public class TaskEnvelope
    {
        public TaskEnvelope(Task task)
        {
            Task = task;
        }

        public Task Task { get; }
    }
}