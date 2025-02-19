namespace ToolsLibrary.Enums
{
    public static class LoginProviders
    {
        public static Providers SettingName { get; }
    }

    public enum Providers
    {
        App=0,
        Google=1,
        Facebook=2,
    }
}
