using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;

namespace GeolocationAds.ViewModels
{
    public partial class RegisterViewModel : BaseViewModel<User, IUserService>
    {
        [ObservableProperty]
        private User _newUser = new User();

      

        public RegisterViewModel(User user, IUserService service) : base(user, service)
        {
            NewUser = new User();

            TestDataDefault();


            WeakReferenceMessenger.Default.Register<CleanOnSubmitMessage<User>>(this, (r, m) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    TestDataDefault();
                });
            });
        }

        private void TestDataDefault()
        {
            // Asignar valores de prueba
            this.Model.FullName = "Test";

            this.Model.Phone = "111-111-1111";

            this.Model.Email = "test@gmail.com";

            // ✅ Asegurar que Login no sea null
            //if (this.Model.Login.IsObjectNull())
            //    this.Model.Login = new ToolsLibrary.Models.Login();

            // ✅ Actualizar propiedades directamente
            this.Model.Login.Username = "test";

            this.Model.Login.Password = "12345";

            // Limpiar imagen de perfil
            this.Model.ProfileImageBytes = null;

            // Limpiar ImageSource enlazado en la vista
            ProfileImage = null;

            // Actualizar flag de visibilidad
            HasProfileImage = false;
        }


        [RelayCommand]
        private void ClearData()
        {
            this.Model.FullName = string.Empty;

            this.Model.Phone = string.Empty;

            this.Model.Email = string.Empty;

            this.Model.Login.Username = string.Empty;

            this.Model.Login.Password = string.Empty;
        }

        public string Avatar => !string.IsNullOrWhiteSpace(Model.FullName)    ? Model.FullName.Trim()[0].ToString().ToUpper(): "?";

        //[RelayCommand]
        //private async Task SelectProfileImageAsync()
        //{
        //    await RunWithLoadingIndicator(async () =>
        //    {
        //        var pickOptions = new PickOptions
        //        {
        //            PickerTitle = "Selecciona una imagen",
        //            FileTypes = FilePickerFileType.Images
        //        };

        //        var result = await FilePicker.PickAsync(pickOptions);

        //        if (result.IsObjectNull())
        //            return;

        //        await using var stream = await result.OpenReadAsync();

        //        using var memoryStream = new MemoryStream();

        //        await stream.CopyToAsync(memoryStream);

        //        Model.ProfileImageBytes = memoryStream.ToArray();

        //        ProfileImage = ImageSource.FromStream(() => new MemoryStream(Model.ProfileImageBytes));

        //        HasProfileImage = true;
        //    });
        //}


    }
}