using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using Microsoft.Toolkit.Mvvm.Input;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class EditAdvertismentViewModel : BaseViewModel2<Advertisement, IAdvertisementService>
    {
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

        private byte[] fileBytes;

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

        public EditAdvertismentViewModel(Advertisement model, IAdvertisementService service, LogUserPerfilTool logUserPerfil) : base(model, service, logUserPerfil)
        {
            this.Image = new Image();

            WeakReferenceMessenger.Default.Register<UpdateMessage<Advertisement>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    SetCurrentImage();
                });
            });
        }

        private void SetCurrentImage()
        {
            this.IsAnimation = true;

            Image.Source = ImageSource.FromStream(() => new MemoryStream(this.Model.Content));
        }

        [ICommand]
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

                    var isImageSelected = result.FileName.ToLower().EndsWith(".jpg") ||
                                       result.FileName.ToLower().EndsWith(".png") ||
                                        result.FileName.ToLower().EndsWith(".gif") ||
                                       result.FileName.ToLower().EndsWith(".jpeg");

                    if (isImageSelected)
                    {
                        //Image.IsAnimationPlaying = false;

                        Image.Source = ImageSource.FromStream(() => new MemoryStream(fileBytes));

                        //Image.IsAnimationPlaying = true;

                        IsAnimation = true;

                        this.Model.Content = fileBytes;

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
    }
}