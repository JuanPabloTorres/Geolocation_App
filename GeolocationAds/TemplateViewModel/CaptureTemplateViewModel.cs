using GeolocationAds.AppTools;
using GeolocationAds.Services;
using System.Collections.ObjectModel;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.TemplateViewModel;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public class CaptureTemplateViewModel : TemplateBaseViewModel
    {
        public ICaptureService Service { get; set; }

        public ObservableCollection<ContentTypeTemplateViewModel2> ContentTypesTemplate
        {
            get; set;
        }

        private Advertisement _currentAdvertisement;

        public Advertisement CurrentAdvertisement
        {
            get => _currentAdvertisement;
            set
            {
                if (_currentAdvertisement != value)
                {
                    _currentAdvertisement = value;

                    OnPropertyChanged();
                }
            }
        }

        private Capture _capture;

        public Capture Capture
        {
            get => _capture;
            set
            {
                if (_capture != value)
                {
                    _capture = value;

                    OnPropertyChanged();
                }
            }
        }

        public CaptureTemplateViewModel(Capture capture, ICaptureService service)
        {
            //this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel>();

            this.RemoveCommand = new Command<Capture>(Remove);

            this.Capture = capture;

            this.CurrentAdvertisement = capture.Advertisements;

            this.Service = service;

            //Task.Run(async () => { await FillTemplate(); });
        }

        public async Task FillTemplate()
        {
            if (!this.CurrentAdvertisement.Contents.IsObjectNull())
            {
                foreach (var item in this.CurrentAdvertisement.Contents)
                {
                    var _template = await AppToolCommon.ProcessContentItem(item);

                    this.ContentTypesTemplate.Add(_template);
                }
            }
        }

        public async Task InitializeAsync()
        {
            this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel2>();

            await FillTemplate();
        }

        public async void Remove(Capture capture)
        {
            try
            {
                var _removeResponse = await Shell.Current.DisplayAlert("Notification", $"Are you sure you want to remove this item?", "Yes", "No");

                if (_removeResponse)
                {
                    var _apiResponse = await this.Service.Remove(capture.ID);

                    if (_apiResponse.IsSuccess)
                    {
                        OnDeleteItem(EventArgs.Empty);
                    }
                    else
                    {
                        await CommonsTool.DisplayAlert("Error", _apiResponse.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}