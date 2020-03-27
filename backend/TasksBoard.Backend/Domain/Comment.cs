using System;

namespace TasksBoard.Backend.Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Message { get; set; }

        public User Author { get; set; }
    }
}