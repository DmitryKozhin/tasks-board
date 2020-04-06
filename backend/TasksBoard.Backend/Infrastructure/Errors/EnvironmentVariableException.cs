using System;

namespace TasksBoard.Backend.Infrastructure.Errors
{
    public class EnvironmentVariableException : Exception
    {
        public EnvironmentVariableException(string envVariableName) 
            : base($"The environment varialbe '{envVariableName} doesn't exist!'")
        {
            EnvVariableName = envVariableName;
        }

        public string EnvVariableName { get; }
    }
}