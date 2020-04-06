using System;

namespace TasksBoard.Backend.Domain
{
    public class UserBoard
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid BoardId { get; set; }
        public Board Board { get; set; }
    }
}