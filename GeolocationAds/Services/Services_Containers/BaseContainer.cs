using ToolsLibrary.Tools;

namespace GeolocationAds.Services.Services_Containers
{
    public class BaseContainer : IBaseContainer
    {
        public LogUserPerfilTool LogUserPerfilTool { get; }

        public BaseContainer(LogUserPerfilTool logUserPerfilTool)
        {
            LogUserPerfilTool = logUserPerfilTool;
        }
    }
}