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

        //public M Model
        //{
        //    get => _model;
        //    set
        //    {
        //        if (!EqualityComparer<M>.Default.Equals(_model, value))
        //        {
        //            _model = value;

        //            OnPropertyChanged();
        //        }
        //    }
        //}

        protected S service { get; set; }

        public TemplateCardViewModel(M model, S service)
        {
            this.Model = model;

            this.service = service;

            //this.RemoveCommand = new Command<M>(Remove);
        }

        [RelayCommand]
        public async Task Remove(M item)
        {
            try
            {
                this.IsLoading = true;

                var _removeAlertResponse = await Shell.Current.DisplayAlert("Notification", $"Are you sure you want to remove this location?", "Yes", "No");

                if (_removeAlertResponse)
                {
                    var _id = GenericTool<M>.GetPropertyValueFromObject<int>(item, nameof(BaseModel.ID));

                    object[] parameters = new object[] { _id };

                    var _apiResponse = await GenericTool<ResponseTool<M>>.InvokeMethodName<ResponseTool<M>>(this.service, nameof(BaseService<M>.Remove), parameters);

                    if (_apiResponse.IsSuccess)
                    {
                        await CommonsTool.DisplayAlert("Notification", _apiResponse.Message);



                        EventManager.Instance.Publish(this.Model);

                    }
                    else
                    {
                        await CommonsTool.DisplayAlert("Error", _apiResponse.Message);
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

        //public void OnItemDeleted2()
        //{
        //    // Do some work here.
        //    // When the item is deleted, publish an event.
        //    EventManager.Instance.Publish(this.Model);
        //}
    }
}