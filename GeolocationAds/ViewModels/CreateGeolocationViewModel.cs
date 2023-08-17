using GeolocationAds.Services;
using System.Windows.Input;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public class CreateGeolocationViewModel : BaseViewModel
    {
        private IGeolocationAdService geolocationAdService;

        private IAdvertisementService advertisementService;

        // Command property for the submit button
        public ICommand SubmitCommand { get; }

        public ICommand FileUploadCommand { get; }

        private Advertisement _advertisement;

        private Image _image;

        private byte[] fileBytes;

        private bool isImageSelected;

        private bool isAnimation;

        public bool IsAnimation
        {
            get => isAnimation;
            set
            {
                if (isAnimation != value)
                {
                    isAnimation = value;

                    OnPropertyChanged();
                }
            }
        }

        public Advertisement Advertisement
        {
            get => _advertisement;

            set
            {
                if (_advertisement != value)
                {
                    _advertisement = value;

                    OnPropertyChanged();
                }
            }
        }

        public Image Image
        {
            get => _image;

            set
            {
                if (_image != value)
                {
                    _image = value;

                    OnPropertyChanged();

                    ReloadImage();
                }
            }
        }

        public CreateGeolocationViewModel(IGeolocationAdService geolocationAdService, IAdvertisementService advertisementService)
        {
            this.geolocationAdService = geolocationAdService;

            this.advertisementService = advertisementService;

            Advertisement = new Advertisement();

            Image = new Image();

            Image.IsAnimationPlaying = false;

            SubmitCommand = new Command(OnSubmitButtonClicked);

            FileUploadCommand = new Command(OnUploadCommandExecuted);
        }

        private async void OnSubmitButtonClicked()
        {
            IsLoading = true;

            this.Advertisement.Content = this.fileBytes;

            this.Advertisement.CreateDate = DateTime.Now;

            var _apiResponse = await this.advertisementService.Add(this.Advertisement);

            if (_apiResponse.IsSuccess)
            {
                Advertisement = new Advertisement();

                Image.Source = null;

                await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
            }

            IsLoading = false;
        }

        private async void OnUploadCommandExecuted()
        {
            try
            {
                IsAnimation = false;

                var fileTypes = new Dictionary<DevicePlatform, IEnumerable<string>>();

                fileTypes.Add(DevicePlatform.Android, new[] { "image/gif", "image/png", "image/jpeg" });

                var customFileTypes = new FilePickerFileType(fileTypes);

                // Pick image
                FileResult imageResult = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = customFileTypes,
                    PickerTitle = "Select an image"
                });

                // Pick video
                //FileResult videoResult = await FilePicker.PickAsync(new PickOptions
                //{
                //    FileTypes = FilePickerFileType.Videos,
                //    PickerTitle = "Select a video"
                //});

                //FileResult result = imageResult ?? videoResult;

                FileResult result = imageResult;

                if (result != null)
                {
                    fileBytes = await CommonsTool.GetFileBytesAsync(result);

                    //SelectedFileLabel.Text = result.FileName;

                    //// Check if the selected file is an image or video
                    //bool isImage = result.FileName.ToLower().EndsWith(".jpg") ||
                    //               result.FileName.ToLower().EndsWith(".png") ||
                    //               result.FileName.ToLower().EndsWith(".jpeg");

                    //bool isVideo = result.FileName.ToLower().EndsWith(".mp4") ||
                    //               result.FileName.ToLower().EndsWith(".avi") ||
                    //               result.FileName.ToLower().EndsWith(".mov");

                    //if (isImage || isVideo)
                    //{
                    //    // Convert selected file to bytes
                    //    using (var stream = await result.OpenReadAsync())
                    //    {
                    //        using (MemoryStream ms = new MemoryStream())
                    //        {
                    //            await stream.CopyToAsync(ms);
                    //            fileBytes = ms.ToArray();
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    // File is neither an image nor a video
                    //    await DisplayAlert("Invalid File Type", "Please select an image or video file.", "OK");
                    //}

                    isImageSelected = result.FileName.ToLower().EndsWith(".jpg") ||
                                     result.FileName.ToLower().EndsWith(".png") ||
                                      result.FileName.ToLower().EndsWith(".gif") ||
                                     result.FileName.ToLower().EndsWith(".jpeg");

                    if (isImageSelected)
                    {
                        //Image.IsAnimationPlaying = false;                       

                        Image.Source = ImageSource.FromStream(() => new MemoryStream(fileBytes));

                        Image.IsAnimationPlaying = true;

                        IsAnimation = true;

                        //if (result.FileName.ToLower().EndsWith(".gif"))
                        //{
                        //    IsAnimation = true;
                        //}
                        //Im.IsVisible = true;
                        //SelectedVideo.IsVisible = false;
                    }
                    else
                    {
                        //// Display video using WebView
                        //SelectedVideo.Source = new HtmlWebViewSource
                        //{
                        //    Html = $"<html><body><video width='100%' height='100%' controls><source src='data:video/mp4;base64,{Convert.ToBase64String(fileBytes)}' type='video/mp4' /></video></body></html>"
                        //};
                        //SelectedVideo.IsVisible = true;
                        //SelectedImage.IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception

                //SelectedFileLabel.Text = "Error selecting file: " + ex.Message;

                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void ReloadImage()
        {
            var oldSource = Image.Source;

            Image.Source = null;

            Image.Source = oldSource;

            Image.IsAnimationPlaying = true;
        }
    }
}