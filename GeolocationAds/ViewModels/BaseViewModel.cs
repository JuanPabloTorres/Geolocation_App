using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GeolocationAds.ViewModels
{
    public partial class BaseViewModel : INotifyPropertyChanged
    {
        public ICommand RemoveCommand { get; set; }

        public ICommand CreateCommand { get; set; }

        public ICommand UpdateCommand { get; set; }

        public ICommand GetCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public delegate void RemoveItemEventHandler(object sender, EventArgs e);

        public event RemoveItemEventHandler ItemDeleted;

        protected virtual void OnDeleteItem(EventArgs e)
        {
            ItemDeleted?.Invoke(this, e);
        }
    }
}