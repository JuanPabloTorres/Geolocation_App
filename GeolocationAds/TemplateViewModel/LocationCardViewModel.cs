using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.Services;
using GeolocationAds.ViewModels;
using ToolsLibrary.Managers;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class LocationCardViewModel<T, S> : TemplateBaseViewModel2<T> where T : class
    {
        [ObservableProperty]
        private T model;

        protected S service { get; set; }

        public Action<T> ItemDeleted { get; set; }  // ✅ Se usa Action en vez de event para borrar.

        public LocationCardViewModel(T model, S service, Action<T> onDelete)
        {
            this.Model = model;

            this.service = service;

            ItemDeleted = onDelete;
        }

        [RelayCommand]
        public async Task Remove(T item)
        {
            try
            {
                this.IsLoading = true;

                bool isConfirmed = await Shell.Current.DisplayAlert("Notification", "Are you sure you want to remove this location?", "Yes", "No");

                if (isConfirmed)
                {
                    int id = GenericTool<T>.GetPropertyValueFromObject<int>(item, nameof(BaseModel.ID));

                    var response = await RemoveItemAsync(id);

                    if (response.IsSuccess)
                    {
                        //EventManager2.Instance.Publish(this, CurrentPageContext);

                        ItemDeleted?.Invoke(item);
                    }
                    else
                    {
                        await CommonsTool.DisplayAlert("Error", response.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private async Task<ResponseTool<T>> RemoveItemAsync(int id)
        {
            object[] parameters = { id };

            return await GenericTool<ResponseTool<T>>.InvokeMethodName<ResponseTool<T>>(this.service, nameof(BaseService<T>.Remove), parameters);
        }

    }
}