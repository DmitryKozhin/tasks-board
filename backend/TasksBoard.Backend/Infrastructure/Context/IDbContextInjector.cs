using System;

namespace TasksBoard.Backend.Infrastructure.Context
{
    public interface IDbContextInjector : IDisposable
    {
        TasksBoardContext WriteContext { get; }
        TasksBoardContext ReadContext { get; }
    }
}