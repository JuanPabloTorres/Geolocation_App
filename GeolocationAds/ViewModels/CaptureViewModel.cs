using GeolocationAds.Services;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public class CaptureViewModel : BaseViewModel2<Capture, ICaptureService>
    {
        public CaptureViewModel(Capture model, ICaptureService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
        }
    }
}
