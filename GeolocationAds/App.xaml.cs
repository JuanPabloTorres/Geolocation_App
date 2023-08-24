using GeolocationAds.ViewModels;

namespace GeolocationAds;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        //var serviceProvider = new ServiceCollection()
        //         .AddTransient<IUserService, UserService>()
        //         .AddTransient<User>()
        //         .AddSingleton<LogUserPerfilTool>()
        //         .BuildServiceProvider();

        //var userService = serviceProvider.GetRequiredService<IUserService>();

        //var user = serviceProvider.GetRequiredService<User>();

        //var logUserPerfil = serviceProvider.GetRequiredService<LogUserPerfilTool>();

        var viewModel = new AppShellViewModel();

        MainPage = new AppShell(viewModel);
    }
}