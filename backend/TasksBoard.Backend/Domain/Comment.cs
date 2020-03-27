using System;

namespace TasksBoard.Backend.Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public Guid AuthorId { get; set; }
        public User Author { get; set; }

        public Guid? ParentId { get; set; }
        public Comment Parent { get; set; }
    }
}