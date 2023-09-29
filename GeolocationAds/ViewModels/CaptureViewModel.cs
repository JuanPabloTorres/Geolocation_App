using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using System.Collections.ObjectModel;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public class CaptureViewModel : BaseViewModel2<Capture, ICaptureService>
    {
        private ObservableCollection<ContentTypeTemplateViewModel> _contentTypestemplate;

        public ObservableCollection<ContentTypeTemplateViewModel> ContentTypesTemplate
        {
            get => _contentTypestemplate;
            set
            {
                if (_contentTypestemplate != value)
                {
                    _contentTypestemplate = value;

                    OnPropertyChanged();
                }
            }
        }

        public CaptureViewModel(Capture model, ICaptureService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
            this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel>();

            Task.Run(async () => { await LoadData(); });
        }

        protected override async Task LoadData()
        {
            try
            {
                var _apiResponse = await this.service.GetMyCaptures(LogUserPerfilTool.GetUserId());

                if (_apiResponse.IsSuccess)
                {
                    foreach (var item in _apiResponse.Data)
                    {
                        foreach (var content in item.Advertisements.Contents)
                        {
                            var _file = CommonsTool.SaveByteArrayToTempFile(content.Content);

                            var _template = ContentTypeTemplateFactory.BuilContentType(content, _file);

                            this.ContentTypesTemplate.Add(_template);
                        }
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
        }

        public async void Initialize()
        {
            this.IsLoading = true;

            await LoadData();

            this.IsLoading = false;
        }
    }
}