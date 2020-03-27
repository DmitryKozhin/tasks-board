using System;

namespace TasksBoard.Backend.Domain
{
    public class TaskAssignedUser
    {
        public Guid AssignedUserId { get; set; }
        public User AssignedUser { get; set; }

        public Guid TaskId { get; set; }
        public Task Task { get; set; }
    }
}