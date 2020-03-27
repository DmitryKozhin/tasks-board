using System.Collections.Generic;

using Newtonsoft.Json;

namespace TasksBoard.Backend.Domain
{
    public class User : BaseIdEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Biography { get; set; }

        public List<Board> Boards { get; set; }
        public List<Task> Tasks { get; set; }

        [JsonIgnore]
        public byte[] Hash { get; set; }

        [JsonIgnore]
        public byte[] Salt { get; set; }
    }
}