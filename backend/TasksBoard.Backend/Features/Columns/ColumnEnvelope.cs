using TasksBoard.Backend.Domain;

namespace TasksBoard.Backend.Features.Columns
{
    public class ColumnEnvelope
    {
        public ColumnEnvelope(Column column)
        {
            Column = column;
        }

        public Column Column { get; }
    }
}