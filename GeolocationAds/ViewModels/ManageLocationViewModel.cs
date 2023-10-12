using GeolocationAds.Services;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public class ManageLocationViewModel : BaseViewModel2<Advertisement, IAdvertisementService>
    {
        public ManageLocationViewModel(Advertisement model, IAdvertisementService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
            this.ApplyQueryAttributesCompleted += ManageLocationViewModel_ApplyQueryAttributesCompleted; ;
        }

        private async void ManageLocationViewModel_ApplyQueryAttributesCompleted(object sender, EventArgs e)
        {
            try
            {
                this.IsLoading = true;

                //await this.LoadSetting();

                //foreach (var item in this.Model.Contents)
                //{
                //    var _template = await AppToolCommon.ProcessContentItem(item);

                //    _template.ContentTypeDeleted += ContentTypeTemplateViewModel_ContentTypeDeleted;

                //    this.ContentTypesTemplate.Add(_template);
                //}
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
        }
    }
}