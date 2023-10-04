using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using System.Collections.ObjectModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public class CaptureViewModel : BaseViewModel2<Capture, ICaptureService>
    {
        public ObservableCollection<CaptureTemplateViewModel> CaptureTemplateViewModels { get; set; }

        public CaptureViewModel(Capture model, ICaptureService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
            this.CaptureTemplateViewModels = new ObservableCollection<CaptureTemplateViewModel>();

            this.SearchCommand = new Command(Initialize);

            CaptureTemplateViewModel.ItemDeleted += CaptureTemplateViewModel_ItemDeleted;
        }

        private async void CaptureTemplateViewModel_ItemDeleted(object sender, EventArgs e)
        {
            try
            {
                if (sender is CaptureTemplateViewModel model)
                {
                    var _toRemoveAdContent = this.CaptureTemplateViewModels.Where(v => v.Capture.ID == model.Capture.ID).FirstOrDefault();

                    if (!_toRemoveAdContent.IsObjectNull())
                    {
                        this.CaptureTemplateViewModels.Remove(_toRemoveAdContent);

                        await Shell.Current.DisplayAlert("Error", "Capture Removed", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        protected override async Task LoadData()
        {
            this.IsLoading = true;

            try
            {
                this.CaptureTemplateViewModels.Clear();

                var _apiResponse = await this.service.GetMyCaptures(LogUserPerfilTool.GetUserId());

                if (_apiResponse.IsSuccess)
                {
                    foreach (var item in _apiResponse.Data)
                    {
                        var _item = new CaptureTemplateViewModel(item, this.service);

                        this.CaptureTemplateViewModels.Add(_item);
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

            this.IsLoading = false;
        }

        public async void Initialize()
        {
            await LoadData();
        }
    }
}