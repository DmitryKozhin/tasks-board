using System;

namespace TasksBoard.Backend.DbContext.Tables
{
    public abstract class BaseIdEntity
    {
        public Guid Id { get; set; }
    }
}