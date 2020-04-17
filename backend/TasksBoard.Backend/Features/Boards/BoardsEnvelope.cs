using System;
using System.Collections.Generic;

namespace TasksBoard.Backend.Features.Boards
{
    public class BoardsEnvelope
    {
        public List<SmallBoardModel> Boards { get; set; }
        public int BoardsCount { get; set; }
    }

    public class SmallBoardModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}