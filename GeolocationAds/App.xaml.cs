namespace GeolocationAds;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override async void OnStart()
    {
        // Access the backend URL from the resource dictionary
        var backendUrl = Application.Current.Resources["BackendUrl"] as string;

        // Use the backendUrl to configure your services or HttpClient
        // For example:
        //var httpClient = new HttpClient { BaseAddress = new Uri(backendUrl) };

        // Rest of your app startup code...
    }
}
