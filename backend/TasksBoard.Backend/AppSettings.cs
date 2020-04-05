using System;

using Microsoft.Extensions.Configuration;

using Serilog;

namespace TasksBoard.Backend
{
    public class AppSettings
    {
        private readonly IConfiguration _configuration;

        public AppSettings(SourceType sourceType)
        {
            Source = sourceType;
            ReadSettings();
        }

        public AppSettings(SourceType sourceType, IConfiguration configuration)
        {
            Source = sourceType;
            _configuration = configuration;
            if (sourceType == SourceType.Configuration)
                Source = SourceType.EnvironmentVariables;

            ReadSettings();
        }

        public string LogDirectory { get; private set; }
        public SourceType Source { get; }

        public string MasterDatabaseConnectionString { get; set; }
        public string SyncReplicaDatabaseConnectionString { get; set; }

        public DbConnectionData MasterDatabaseConnectionData { get; set; }
        public DbConnectionData SyncReplicaDatabaseConnectionData { get; set; }

        private string GetSetting(string setting)
        {
            if (Source == SourceType.Configuration) return _configuration[setting];

            var platform = Environment.OSVersion.Platform;

            var target = EnvironmentVariableTarget.User;
            if (platform == PlatformID.Unix || platform == PlatformID.MacOSX)
                target = EnvironmentVariableTarget.Process;

            var variablePrefix = "TasksBoard_";

            if (Source == SourceType.TestEnvironmentVariables)
                variablePrefix = "Test_" + variablePrefix;

            var environmentVariableName = variablePrefix + setting;
            var result = Environment.GetEnvironmentVariable(variablePrefix + setting, target);

#if DEBUG
            Log.Information(
                $"Environment variable getting: {environmentVariableName} = '{result}'");
#endif

            return result;
        }

        private string GetConnectionString(string replicaName, out DbConnectionData data)
        {
            data = ReadConnectionData(replicaName);
            var result =
                    $"Server={data.Host};"
                    + $"Database={data.Name};"
                    + $"User Id={data.User};"
                    + $"Password={data.Pass};"
                    + $"Port={data.Port};"
                    + "SSL Mode=Prefer;"
                    + "Trust Server Certificate=true;"
                    + "Server Compatibility Mode=Redshift;"
                ;

            return result;
        }

        private DbConnectionData ReadConnectionData(string replicaName)
        {
            var result = new DbConnectionData
            {
                Host = GetSetting(replicaName + "_DbHost"),
                Port = GetSetting(replicaName + "_DbPort"),
                User = GetSetting(replicaName + "_DbUser"),
                Pass = GetSetting(replicaName + "_DbPass"),
                Name = GetSetting(replicaName + "_DbName")
            };

            return result;
        }

        private void ReadSettings()
        {
            LogDirectory = GetSetting("LogDirectory");

            MasterDatabaseConnectionString =
                GetConnectionString("Master", out var masterDatabaseConnectionData);
            MasterDatabaseConnectionData = masterDatabaseConnectionData;

            SyncReplicaDatabaseConnectionString =
                GetConnectionString("Sync", out var syncReplicaDatabaseConnectionData);
            SyncReplicaDatabaseConnectionData = syncReplicaDatabaseConnectionData;
        }

        public class DbConnectionData
        {
            public string Host { get; set; }
            public string Name { get; set; }
            public string Pass { get; set; }
            public string Port { get; set; }
            public string User { get; set; }
        }

        public enum SourceType
        {
            TestEnvironmentVariables,
            Configuration,
            EnvironmentVariables
        }
    }
}