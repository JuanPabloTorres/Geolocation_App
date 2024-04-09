using CommunityToolkit.Maui.Views;
using GeolocationAds.AppTools;
using GeolocationAds.Pages;
using GeolocationAds.Services;
using GeolocationAds.TemplateViewModel;
using GeolocationAds.Tools;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToolsLibrary.Extensions;
using ToolsLibrary.Factories;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace ToolsLibrary.TemplateViewModel
{
    public class MangeContentTemplateViewModel : TemplateBaseViewModel
    {
        //public ICommand SetLocationCommand { get; set; }

        //public ICommand OpenActionPopUpCommand { get; set; }

        //public ICommand onNavigate { get; set; }

        private ICommand _setLocationCommand;
        public ICommand SetLocationCommand => _setLocationCommand ??= new Command<Advertisement>(SetLocationYesOrNoAlert);

        private ICommand _openActionPopUpCommand;
        public ICommand OpenActionPopUpCommand => _openActionPopUpCommand ??= new Command(async () => await OpenActionPopUp());

        private ICommand _onNavigate;
        public ICommand OnNavigate => _onNavigate ??= new Command<int>(Navigate);


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

        //public ObservableCollection<ContentTypeTemplateViewModel> ContentTypesTemplate { get; set; }

        public ObservableCollection<ContentTypeTemplateViewModel> ContentTypesTemplate { get; } = new ObservableCollection<ContentTypeTemplateViewModel>();


        public MangeContentTemplateViewModel(IAdvertisementService advertisementService, IGeolocationAdService geolocationAdService, Advertisement advertisement) : base(advertisementService, geolocationAdService)
        {
            this.CurrentAdvertisement = advertisement;

            //SetLocationCommand = new Command<Advertisement>(SetLocationYesOrNoAlert);

            RemoveCommand = new Command<Advertisement>(RemoveContentYesOrNoAlert);

            //OpenActionPopUpCommand = new Command(async () => { await OpenActionPopUp(); });

            //this.OnNavigate = new Command<int>(Navigate);
        }

        public async Task InitializeAsync()
        {
            //this.ContentTypesTemplate = new ObservableCollection<ContentTypeTemplateViewModel>();

            await FillTemplate();
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

        public async Task OpenActionPopUp()
        {
            try
            {
                this.IsLoading = true;

                string action = await Shell.Current.DisplayActionSheet("Actions: ", "Cancel", null, "Detail", "Set Location", "Manage Location");

                switch (action)
                {
                    case "Detail":

                        Navigate(this.CurrentAdvertisement.ID);

                        break;

                    case "Set Location":

                        SetLocationYesOrNoAlert(this.CurrentAdvertisement);

                        break;

                    case "Manage Location":

                        var navigationParameter = new Dictionary<string, object> { { "ID", this.CurrentAdvertisement.ID } };

                        await NavigateAsync(nameof(ManageLocation), navigationParameter);

                        break;

                    default:

                        break;
                }
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

        private async Task CreateAdToLocation(Advertisement ad)
        {
            try
            {
                this.IsLoading = true;

                var locationReponse = await GeolocationTool.GetLocation();

                if (!locationReponse.IsSuccess)
                {
                    await CommonsTool.DisplayAlert("Error", locationReponse.Message);

                    return;
                }

                GeolocationAd newAd = GeolocationAdFactory.CreateGeolocationAd(ad, locationReponse.Data);

                var _apiResponse = await this.geolocationAdService.Add(newAd);

                if (_apiResponse.IsSuccess)
                {
                    this.CurrentAdvertisement = ad;

                    await CommonsTool.DisplayAlert("Notification", _apiResponse.Message);
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", _apiResponse.Message);
                }
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

        private async Task RemoveContent(Advertisement ad)
        {
            try
            {
                this.IsLoading = true;

                var _apiResponse = await this.advertisementService.Remove(ad.ID);

                if (_apiResponse.IsSuccess)
                {
                    RemoveCurrentItem();

                    await CommonsTool.DisplayAlert("Notification", _apiResponse.Message);
                }
                else
                {
                    await CommonsTool.DisplayAlert("Error", _apiResponse.Message);
                }
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

        private async void SetLocationYesOrNoAlert(Advertisement selectAd)
        {
            if (!selectAd.IsObjectNull())
            {
                var response = await Shell.Current.DisplayAlert("Notification", $"Do you want to set the current location to {selectAd.Title} ?", "Yes", "No");

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
                var response = await Shell.Current.DisplayAlert("Notification", $"Are you sure you want to remove this item?", "Yes", "No");

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