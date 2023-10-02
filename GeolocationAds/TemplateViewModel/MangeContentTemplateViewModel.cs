using CommunityToolkit.Maui.Views;
using GeolocationAds.App_ViewModel_Factory;
using GeolocationAds.Pages;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using GeolocationAds.Tools;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace ToolsLibrary.TemplateViewModel
{
    public class MangeContentTemplateViewModel : TemplateBaseViewModel
    {
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

        //public MangeContentTemplateViewModel()
        //{
        //    SetLocationCommand = new Command<Advertisement>(SetLocationYesOrNoAlert);
        //}

        //public MangeContentTemplateViewModel(IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService) : base(advertisementService, geolocationAdService)
        //{
        //    SetLocationCommand = new Command<Advertisement>(SetLocationYesOrNoAlert);

        //    RemoveCommand = new Command<Advertisement>(RemoveContentYesOrNoAlert);

        //    this.onNavigate = new Command<int>(Navigate);
        //}

        public MangeContentTemplateViewModel(IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, Advertisement advertisement) : base(advertisementService, geolocationAdService)
        {
            SetLocationCommand = new Command<Advertisement>(SetLocationYesOrNoAlert);

            RemoveCommand = new Command<Advertisement>(RemoveContentYesOrNoAlert);

            this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel>();

            this.CurrentAdvertisement = advertisement;

            this.onNavigate = new Command<int>(Navigate);

            FillTemplate();
        }

        public MangeContentTemplateViewModel(IAdvertisementService advertisementService, Advertisement advertisement) : base(advertisementService)
        {
            this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel>();

            this.CurrentAdvertisement = advertisement;

            FillTemplate();
        }

        private MediaSource _mediaSource;

        public MediaSource MediaSource
        {
            get => _mediaSource;
            set
            {
                if (_mediaSource != value)
                {
                    _mediaSource = value;

                    OnPropertyChanged();
                }
            }
        }

        private Image _image;

        public Image Image
        {
            get => _image;
            set
            {
                if (_image != value)
                {
                    _image = value;

                    OnPropertyChanged();
                }
            }
        }

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

        public void FillTemplate()
        {
            if (!this.CurrentAdvertisement.Contents.IsObjectNull())
            {
                foreach (var item in this.CurrentAdvertisement.Contents)
                {
                    if (item.Type == ContentVisualType.Image)
                    {
                        var _template = ContentTypeTemplateFactory.BuilContentType(item, item.Content);

                        this.ContentTypesTemplate.Add(_template);
                    }
                    else
                    {
                        var _file = CommonsTool.SaveByteArrayToTempFile(item.Content);

                        var _template = ContentTypeTemplateFactory.BuilContentType(item, _file);

                        this.ContentTypesTemplate.Add(_template);
                    }
                }
            }
        }

        public delegate void RemoveItemEventHandler(object sender, EventArgs e);

        public static event RemoveItemEventHandler ItemDeleted;

        protected virtual void OnDeleteItem(EventArgs e)
        {
            ItemDeleted?.Invoke(this, e);
        }



        public ICommand SetLocationCommand { get; set; }

        public ICommand RemoveCommand { get; set; }

        public ICommand onNavigate { get; set; }

        private async Task CreateAdToLocation(Advertisement ad)
        {
            this.IsLoading = true;

            try
            {
                var locationReponse = await GeolocationTool.GetLocation();

                if (locationReponse.IsSuccess)
                {
                    GeolocationAd newAd = new GeolocationAd()
                    {
                        AdvertisingId = ad.ID,
                        Advertisement = ad,
                        CreateDate = DateTime.Now,
                        Latitude = locationReponse.Data.Latitude,
                        Longitude = locationReponse.Data.Longitude,
                        ExpirationDate = DateTime.Now.AddDays(7)
                    };

                    var _apiResponse = await this.geolocationAdService.Add(newAd);

                    if (_apiResponse.IsSuccess)
                    {
                        this.CurrentAdvertisement = _apiResponse.Data.Advertisement;

                        await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", locationReponse.Message, "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

            this.IsLoading = false;
        }

        private async Task RemoveContent(Advertisement ad)
        {
            try
            {
                this.IsLoading = true;

                var locationReponse = await GeolocationTool.GetLocation();

                if (locationReponse.IsSuccess)
                {
                    var _apiResponse = await this.advertisementService.Remove(ad.ID);

                    if (_apiResponse.IsSuccess)
                    {
                        //WeakReferenceMessenger.Default.Send(new DeleteItemMessage(ad));

                        RemoveCurrentItem();

                        await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", locationReponse.Message, "OK");
                }

                this.IsLoading = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void SetLocationYesOrNoAlert(Advertisement selectAd)
        {
            if (!selectAd.IsObjectNull())
            {
                var response = await Shell.Current.DisplayAlert("Notification", $"Set Location To:{selectAd.Title}", "Yes", "No");

                if (response)
                {
                    await this.CreateAdToLocation(selectAd);
                }
            }
        }

        private async void RemoveContentYesOrNoAlert(Advertisement selectAd)
        {
            if (!selectAd.IsObjectNull())
            {
                var response = await Shell.Current.DisplayAlert("Notification", $" Remove:{selectAd.Title}", "Yes", "No");

                if (response)
                {
                    await this.RemoveContent(selectAd);
                }
            }
        }

        private async void Navigate(int id)
        {
            var navigationParameter = new Dictionary<string, object> { { "ID", id } };

            await Shell.Current.GoToAsync(nameof(EditAdvertisment), navigationParameter);
        }

        public override void RemoveCurrentItem()
        {
            OnDeleteItem(EventArgs.Empty);
        }
    }
}