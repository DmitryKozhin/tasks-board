namespace TasksBoard.Backend.Infrastructure
{
    public interface ICurrentUserAccessor
    {
        string GetCurrentUsername();
    }
}