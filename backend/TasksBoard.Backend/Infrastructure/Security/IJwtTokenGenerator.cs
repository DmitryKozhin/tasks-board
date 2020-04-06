using System.Threading.Tasks;

namespace TasksBoard.Backend.Infrastructure.Security
{
    public interface IJwtTokenGenerator
    {
        Task<string> CreateToken(string username);
    }
}