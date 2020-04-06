using TasksBoard.Backend.Domain;

namespace TasksBoard.Backend.Features.Boards
{
    public class BoardEnvelope
    {
        public BoardEnvelope(Board board)
        {
            Board = board;
        }

        public Board Board { get; }
    }
}