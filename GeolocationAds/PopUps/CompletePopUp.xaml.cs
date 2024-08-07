using CommunityToolkit.Maui.Views;

namespace GeolocationAds.PopUps;

public partial class CompletePopUp : Popup
{
    public CompletePopUp()
    {
        InitializeComponent();

        //var a = new SKFileLottieImageSource();

        //a.File = "complete_1.json";



        //SKLottieView sKLottieAnimation = new SKLottieView()
        //{
        //    Source = a,
        //    RepeatCount = 0,
        //    HeightRequest = 200,
        //    WidthRequest = 200,
        //    IsAnimationEnabled=true


        //};

        //sKLottieAnimation.AnimationCompleted += SKLottieView_AnimationCompleted;

        //lottieContainer.Children.Add(sKLottieAnimation);


    }



    private async void SKLottieView_AnimationCompleted(object sender, EventArgs e)
    {
        await CloseAsync(false);
    }

    //public async Task ShowStarAnimation()
    //{
    //    await Task.Delay(5000);

    //    complete.IsAnimationEnabled = true;


    //}
}