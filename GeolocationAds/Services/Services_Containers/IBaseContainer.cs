using GeolocationAds.ViewModels;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services.Services_Containers
{
    public interface IBaseContainer
    {
        LogUserPerfilTool LogUserPerfilTool { get; }

        IAppSettingService AppSettingService { get; }

        AppShellViewModel2 AppShellViewModel { get; }
    }
}
