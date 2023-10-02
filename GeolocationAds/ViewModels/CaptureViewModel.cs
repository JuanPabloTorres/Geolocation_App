using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using System.Collections.ObjectModel;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public class CaptureViewModel : BaseViewModel2<Capture, ICaptureService>
    {
        private IAdvertisementService advertisementService { get; set; }

        public ObservableCollection<CaptureTemplateViewModel> MangeContents { get; set; }

        public CaptureViewModel(Capture model, ICaptureService service, LogUserPerfilTool logUserPerfil, IAdvertisementService advertisementService) : base(model, service, logUserPerfil)
        {
            this.MangeContents = new ObservableCollection<CaptureTemplateViewModel>();

            Task.Run(async () => { await LoadData(); });

            this.advertisementService = advertisementService;
        }

        protected override async Task LoadData()
        {
            this.IsLoading = true;

            try
            {
                this.MangeContents.Clear();

                var _apiResponse = await this.service.GetMyCaptures(LogUserPerfilTool.GetUserId());

                if (_apiResponse.IsSuccess)
                {
                    //foreach (var item in _apiResponse.Data)
                    //{
                    //    foreach (var content in item.Advertisements.Contents)
                    //    {
                    //        if (content.Type == ContentVisualType.Image)
                    //        {
                    //            var _template = ContentTypeTemplateFactory.BuilContentType(content, content.Content);

                    //            this.ContentTypesTemplate.Add(_template);
                    //        }
                    //        else
                    //        {
                    //            var _file = CommonsTool.SaveByteArrayToTempFile(content.Content);

                    //            var _template = ContentTypeTemplateFactory.BuilContentType(content, _file);

                    //            this.ContentTypesTemplate.Add(_template);
                    //        }

                    //        //var _file = CommonsTool.SaveByteArrayToTempFile(content.Content);

                    //        //var _template = ContentTypeTemplateFactory.BuilContentType(content, _file);

                    //        //this.ContentTypesTemplate.Add(_template);
                    //    }

                    //}

                    foreach (var item in _apiResponse.Data)
                    {
                        var _item = new CaptureTemplateViewModel(item.Advertisements);

                        this.MangeContents.Add(_item);
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
            this.IsLoading = true;

            await LoadData();

            this.IsLoading = false;
        }
    }
}