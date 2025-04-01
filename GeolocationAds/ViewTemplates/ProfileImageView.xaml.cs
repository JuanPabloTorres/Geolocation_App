using System.Windows.Input;

namespace GeolocationAds.ViewTemplates;

public partial class ProfileImageView : ContentView
{
	public ProfileImageView()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty HasProfileImageProperty =
       BindableProperty.Create(nameof(HasProfileImage), typeof(bool), typeof(ProfileImageView), false);

    public static readonly BindableProperty ProfileImageProperty =
        BindableProperty.Create(nameof(ProfileImage), typeof(ImageSource), typeof(ProfileImageView), null);

    public static readonly BindableProperty AvatarProperty =
        BindableProperty.Create(nameof(Avatar), typeof(string), typeof(ProfileImageView), "?");

    public static readonly BindableProperty SelectProfileImageCommandProperty =
        BindableProperty.Create(nameof(SelectProfileImageCommand), typeof(ICommand), typeof(ProfileImageView));

    public bool HasProfileImage
    {
        get => (bool)GetValue(HasProfileImageProperty);
        set => SetValue(HasProfileImageProperty, value);
    }

    public ImageSource ProfileImage
    {
        get => (ImageSource)GetValue(ProfileImageProperty);
        set => SetValue(ProfileImageProperty, value);
    }

    public string Avatar
    {
        get => (string)GetValue(AvatarProperty);
        set => SetValue(AvatarProperty, value);
    }

    public ICommand SelectProfileImageCommand
    {
        get => (ICommand)GetValue(SelectProfileImageCommandProperty);
        set => SetValue(SelectProfileImageCommandProperty, value);
    }
}