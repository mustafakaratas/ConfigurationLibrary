namespace ConfigurationReader.Settings;

internal static class AppSettings
{
    public static string ApplicationName { get; set; }
    public static int RefreshTimerIntervalInMs { get; set; }
    public const int ConfigurationCacheHours = 24;
}