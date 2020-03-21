using System;

namespace TasksBoard.Backend.DbContext.Tables
{
    public class User : BaseIdEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}