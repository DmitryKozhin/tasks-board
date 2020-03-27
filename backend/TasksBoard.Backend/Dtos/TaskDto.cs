using TasksBoard.Backend.DbContext;

namespace TasksBoard.Backend.Dtos
{
    public class TaskDto : BaseIdEntity
    {
        public string Header { get; set; }
        public string Description { get; set; }
        public int OrderNum { get; set; }

        public UserDto Owner { get; set; }
        public ColumnDto Column { get; set; }
    }
}