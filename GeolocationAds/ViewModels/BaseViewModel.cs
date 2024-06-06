using CommunityToolkit.Mvvm.ComponentModel;

namespace GeolocationAds.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        //public ICommand RemoveCommand { get; set; }

        //public ICommand CreateCommand { get; set; }

        //public ICommand UpdateCommand { get; set; }

        //public ICommand GetCommand { get; set; }

        [ObservableProperty]
        private bool isLoading;

        //public delegate void RemoveItemEventHandler(object sender, EventArgs e);

        //public event RemoveItemEventHandler ItemDeleted;

        //protected virtual void OnDeleteItem(EventArgs e)
        //{
        //    ItemDeleted?.Invoke(this, e);
        //}
    }
}