using GeolocationAds.ViewModels;

namespace GeolocationAds.Pages;

public partial class CreateAdvertisment : ContentPage
{
    private byte[] fileBytes;

    private bool isImageSelected;

    public CreateAdvertisment(CreateGeolocationViewModel createGeolocationViewModel)
    {
        InitializeComponent();

        BindingContext = createGeolocationViewModel;
    }

    private async Task<byte[]> GetFileBytesAsync(FileResult fileResult)
    {
        if (fileResult != null)
        {
            using (var stream = await fileResult.OpenReadAsync())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);
                    return ms.ToArray();
                }
            }
        }
        return null;
    }

    //private async void OnUploadButtonClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        // Pick image
    //        FileResult imageResult = await FilePicker.PickAsync(new PickOptions
    //        {
    //            FileTypes = FilePickerFileType.Images,
    //            PickerTitle = "Select an image"
    //        });

    //        // Pick video
    //        FileResult videoResult = await FilePicker.PickAsync(new PickOptions
    //        {
    //            FileTypes = FilePickerFileType.Videos,
    //            PickerTitle = "Select a video"
    //        });

    //        FileResult result = imageResult ?? videoResult;

    //        if (result != null)
    //        {
    //            fileBytes = await GetFileBytesAsync(result);

    //            isImageSelected = result.FileName.ToLower().EndsWith(".jpg") ||
    //                             result.FileName.ToLower().EndsWith(".png") ||
    //                             result.FileName.ToLower().EndsWith(".jpeg");

    //            if (isImageSelected)
    //            {
    //                SelectedImage.Source = ImageSource.FromStream(() => new MemoryStream(fileBytes));
    //                SelectedImage.IsVisible = true;
    //                SelectedVideo.IsVisible = false;
    //            }
    //            else
    //            {
    //                // Display video using WebView
    //                SelectedVideo.Source = new HtmlWebViewSource
    //                {
    //                    Html = $"<html><body><video width='100%' height='100%' controls><source src='data:video/mp4;base64,{Convert.ToBase64String(fileBytes)}' type='video/mp4' /></video></body></html>"
    //                };
    //                SelectedVideo.IsVisible = true;
    //                SelectedImage.IsVisible = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        // Handle exception
    //        SelectedFileLabel.Text = "Error selecting file: " + ex.Message;
    //    }
    //}


}