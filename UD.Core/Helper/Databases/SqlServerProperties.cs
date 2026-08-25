namespace UD.Core.Helper.Databases
{
    public sealed class SqlServerProperties
    {
        public SqlServerProperties() { }
        public string? ProductVersion { get; set; }
        public string? ProductLevel { get; set; }
        public string? Edition { get; set; }
        public string? BuildClrVersion { get; set; }
        public string? Collation { get; set; }
        public string? ServerName { get; set; }
        public string? InstanceName { get; set; }
        public string? InstanceDefaultDataPath { get; set; }
        public string? InstanceDefaultLogPath { get; set; }
        public int? LCID { get; set; }
        public static readonly string Query =
        $"""
        SELECT
        SERVERPROPERTY('{nameof(ProductVersion)}') AS [{nameof(ProductVersion)}],
        SERVERPROPERTY('{nameof(ProductLevel)}') AS [{nameof(ProductLevel)}],
        SERVERPROPERTY('{nameof(Edition)}') AS [{nameof(Edition)}],
        SERVERPROPERTY('{nameof(BuildClrVersion)}') AS [{nameof(BuildClrVersion)}],
        SERVERPROPERTY('{nameof(Collation)}') AS [{nameof(Collation)}],
        SERVERPROPERTY('{nameof(ServerName)}') AS [{nameof(ServerName)}],
        SERVERPROPERTY('{nameof(InstanceName)}') AS [{nameof(InstanceName)}],
        SERVERPROPERTY('{nameof(InstanceDefaultDataPath)}') AS [{nameof(InstanceDefaultDataPath)}],
        SERVERPROPERTY('{nameof(InstanceDefaultLogPath)}') AS [{nameof(InstanceDefaultLogPath)}],
        SERVERPROPERTY('{nameof(LCID)}') AS [{nameof(LCID)}]
        """;
    }
}