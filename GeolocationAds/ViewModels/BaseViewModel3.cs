using CommunityToolkit.Mvvm.ComponentModel;
using GeolocationAds.PopUps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class BaseViewModel3<T, S> : ObservableObject
    {
        [ObservableProperty]
        private T _model;

        [ObservableProperty]
        private bool isLoading;

        protected FilterPopUp _filterPopUp;

        protected FilterPopUpForSearch _filterPopUpForSearch;

        protected MetaDataPopUp _metaDataPopUp;

        protected LogUserPerfilTool LogUserPerfilTool { get; set; }

        public static int PageIndex { get; set; } = 1;

        protected S service { get; set; }

        public ObservableCollection<T> CollectionModel { get; set; } = new ObservableCollection<T>();

        public virtual async Task OpenFilterPopUp()
        {
            try
            {
                await Shell.Current.DisplayAlert("Notification", "Pop Up Must Override.", "OK");
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        public BaseViewModel3(T model, S service, LogUserPerfilTool logUserPerfil = null)
        {
            Model = model;

            this.service = service;

            LogUserPerfilTool = logUserPerfil;
        }

        protected virtual async Task LoadData(int? pageIndex = 1)
        {
            this.IsLoading = true;

            try
            {
                await Shell.Current.DisplayAlert("Error", "To Load Data Logic...", "OK");
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }

            this.IsLoading = false;
        }

        protected virtual async Task LoadData(object extraData, int? pageIndex = 1)
        {
            this.IsLoading = true;

            try
            {
                await Shell.Current.DisplayAlert("Error", "To Load Data Logic with extra data...", "OK");
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }

            this.IsLoading = false;
        }

        protected virtual async Task OpenFilterPopUpAsync()
        {
            try
            {
                await Shell.Current.DisplayAlert("Notification", "Pop Up Must Override.", "OK");
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        public virtual async Task OpenMetaDataPopUp()
        {
            try
            {
                await Shell.Current.DisplayAlert("Notification", "Pop Up Must Override.", "OK");
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}