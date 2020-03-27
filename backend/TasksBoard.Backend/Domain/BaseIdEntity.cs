using System;

using Newtonsoft.Json;

namespace TasksBoard.Backend.Domain
{
    public abstract class BaseIdEntity
    {
        [JsonIgnore]
        public Guid Id { get; set; }
    }
}