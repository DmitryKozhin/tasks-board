using System.Security;

using TasksBoard.Common.Dtos;

namespace TasksBoard.UsersService.Dtos
{
    public class CreateUserDto : UserDto
    {
        public string Password { get; set; }
    }
}