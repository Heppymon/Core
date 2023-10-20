namespace MyBotCore.Shared.Settings
{
    public class LoggerSettings
    {
        public string ApplicationName { get; set; }
        public string FilePath { get; set; }
        public int FileSizeLimitBytes { get; set; }
        public int RetainedFileLimit { get; set; }
        public string RollingInterval { get; set; }
        public string Template { get; set; }
        public bool DisableEFInformationLogs { get; set; }
        public string SentryDSN { get; set; } = string.Empty!;
        public int LogEventLevel { get; set; } = 4; // Default logLevel is Warning (4)
    }
}
