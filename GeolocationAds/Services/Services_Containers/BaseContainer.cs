using GeolocationAds.ViewModels;
using ToolsLibrary.Tools;

namespace GeolocationAds.Services.Services_Containers
{
    public class BaseContainer : IBaseContainer
    {
        public LogUserPerfilTool LogUserPerfilTool { get; }

        public IAppSettingService AppSettingService { get; }

        public AppShellViewModel2 AppShellViewModel { get; }

        public BaseContainer(LogUserPerfilTool logUserPerfilTool, IAppSettingService appSettingService)
        {
            LogUserPerfilTool = logUserPerfilTool;

            AppSettingService = appSettingService;
        }

        public BaseContainer(LogUserPerfilTool logUserPerfilTool)
        {
            LogUserPerfilTool = logUserPerfilTool;
        }

        public BaseContainer(LogUserPerfilTool logUserPerfilTool, AppShellViewModel2 appShellViewModel2)
        {
            LogUserPerfilTool = logUserPerfilTool;

            AppShellViewModel = appShellViewModel2;
        }
    }
}