using System;

namespace TasksBoard.Backend.Domain
{
    public class UserTask
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid TaskId { get; set; }
        public Task Task { get; set; }
    }
}