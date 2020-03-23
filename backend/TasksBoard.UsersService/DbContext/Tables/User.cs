using TasksBoard.Common.Entities;

namespace TasksBoard.UsersService.DbContext.Tables
{
    public class User : BaseIdEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}