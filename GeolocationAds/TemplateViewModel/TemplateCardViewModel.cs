using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeolocationAds.Services;
using GeolocationAds.ViewModels;
using ToolsLibrary.Managers;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.TemplateViewModel
{
    public partial class TemplateCardViewModel<M, S> : BaseViewModel where M : class
    {
        [ObservableProperty]
        private M model;

        protected S service { get; set; }

        public TemplateCardViewModel(M model, S service)
        {
            this.Model = model;

            this.service = service;
        }

        [RelayCommand]
        public async Task Remove(M item)
        {
            try
            {
                this.IsLoading = true;

                bool isConfirmed = await Shell.Current.DisplayAlert("Notification", "Are you sure you want to remove this location?", "Yes", "No");

                if (isConfirmed)
                {
                    int id = GenericTool<M>.GetPropertyValueFromObject<int>(item, nameof(BaseModel.ID));

                    var response = await RemoveItemAsync(id);

                    if (response.IsSuccess)
                    {
                        EventManager2.Instance.Publish(this, CurrentPageContext);
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

        private async Task<ResponseTool<M>> RemoveItemAsync(int id)
        {
            object[] parameters = { id };

            return await GenericTool<ResponseTool<M>>.InvokeMethodName<ResponseTool<M>>(this.service, nameof(BaseService<M>.Remove), parameters);
        }

    }
}