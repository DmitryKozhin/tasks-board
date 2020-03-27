using System;

namespace TasksBoard.Backend.DbContext
{
    public abstract class BaseIdEntity
    {
        public Guid Id { get; set; }
    }
}