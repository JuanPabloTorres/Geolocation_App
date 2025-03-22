using CommunityToolkit.Mvvm.ComponentModel;

namespace ToolsLibrary.Models
{
    public partial class AppSetting : BaseModel
    {
        [ObservableProperty]
        public string settingName;

        [ObservableProperty]
        public string value;
    }
}
