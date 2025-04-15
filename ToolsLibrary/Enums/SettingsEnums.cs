namespace ToolsLibrary.Enums
{
    public static class SettingsEnums
    {
        public static SettingName SettingName { get; }
    }

    public enum SettingName
    {
        MeterDistance,
        AdTypes,
        SearchRadiusRange
    }

    public enum AdType
    {
        Broadcast,
        Social,
        Store,
        Offer,
        News
    }



}