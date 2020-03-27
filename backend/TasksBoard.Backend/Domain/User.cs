using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace TasksBoard.Backend.Domain
{
    public class User
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Biography { get; set; }

        [JsonIgnore]
        public List<UserBoard> Boards { get; set; }
        
        [JsonIgnore]
        public List<UserTask> Tasks { get; set; }

        [JsonIgnore]
        public byte[] Hash { get; set; }

        [JsonIgnore]
        public byte[] Salt { get; set; }
    }
}