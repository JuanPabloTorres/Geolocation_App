using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.PopUps;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GeolocationAds.ViewModels
{
    public partial class BaseViewModel3<T, S> : RootBaseViewModel
    {
        [ObservableProperty]
        private T _model;

        protected FilterPopUp _filterPopUp;

        protected FilterPopUpForSearch _filterPopUpForSearch;

        protected MetaDataPopUp _metaDataPopUp;

        protected CompletePopUp _completePopUp;

        protected FilterPopUpViewModel2 filterPopUpViewModel;

        protected S service { get; set; }

        public ObservableCollection<T> CollectionModel { get; set; } = new ObservableCollection<T>();

        public BaseViewModel3(T model, S service, LogUserPerfilTool logUserPerfil = null) : base(logUserPerfil)
        {
            Model = model;

            this.service = service;

            LogUserPerfilTool = logUserPerfil;
        }

        protected override async Task Get(int id)
        {
            await RunWithLoadingIndicator(async () =>
            {
                var getMethod = this.service.GetType().GetMethod(nameof(Get), new System.Type[] { typeof(int) });

                if (getMethod != null)
                {
                    object[] parameters = new object[] { id };

                    // Invoke the method directly and await the response
                    var responseTask = (Task<ResponseTool<T>>)getMethod.Invoke(this.service, parameters);

                    var _apiResponse = await responseTask;

                    if (!_apiResponse.IsSuccess)
                    {
                        throw new Exception(_apiResponse.Message);
                    }

                    this.Model = _apiResponse.Data;
                }
            });
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

        protected void SetUpdateMetadata(T obj)
        {
            DateTime now = DateTime.Now;

            ToolsLibrary.Tools.GenericTool<T>.SetPropertyValueOnObject(obj, nameof(BaseModel.UpdateDate), now);

            ToolsLibrary.Tools.GenericTool<T>.SetPropertyValueOnObject(obj, nameof(BaseModel.UpdateBy), this.LogUserPerfilTool.LogUser.ID);
        }

        [RelayCommand]
        public virtual async Task Submit(T obj)
        {
            await RunWithLoadingIndicator(async () =>
            {
                if (this.ValidationResults.Any())
                {
                    this.ValidationResults.Clear();
                }

                DateTime now = DateTime.Now;

                var validationContextCurrentType = new ValidationContext(obj);

                bool isObjValid = Validator.TryValidateObject(obj, validationContextCurrentType, ValidationResults, true);

                var propertyInstances = ToolsLibrary.Tools.GenericTool<T>.GetSubPropertiesOfWithForeignKeyAttribute(obj);

                bool allSubPropertyValid = true;

                foreach (var item in propertyInstances)
                {
                    if (!item.IsObjectNull())
                    {
                        var tempValidationResultsSubProperty = new List<ValidationResult>();

                        var validationContextSubProperty = new ValidationContext(item);

                        this.ValidationContexts.Add(validationContextSubProperty);

                        if (!Validator.TryValidateObject(item, validationContextSubProperty, tempValidationResultsSubProperty, true))
                        {
                            allSubPropertyValid = false;
                        }
                        ValidationResults.AddRange(tempValidationResultsSubProperty);
                    }
                }

                if (isObjValid && allSubPropertyValid)
                {
                    var addMethod = this.service.GetType().GetMethod("Add");

                    if (!addMethod.IsObjectNull())
                    {
                        ResponseTool<T> apiResponse = await (Task<ResponseTool<T>>)addMethod.Invoke(this.service, new object[] { obj });

                        if (apiResponse.IsSuccess)
                        {
                            if (Activator.CreateInstance<T>() is T newInstance)
                            {
                                if (this.ValidationResults.Any())
                                {
                                    this.ValidationResults.Clear();
                                }

                                this.IsLoading = false;

                                //_completePopUp = new CompletePopUp();

                                await Shell.Current.CurrentPage.ShowPopupAsync(new CompletePopUp());

                                //await _completePopUp.ShowStarAnimation();

                                //this.Model = default(T);

                                WeakReferenceMessenger.Default.Send(new CleanOnSubmitMessage<T>(this.Model));
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error", $"Type {typeof(T).FullName} does not have a parameterless constructor.", "OK");
                            }
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Error", apiResponse.Message, "OK");
                        }
                    }
                }
            });
        }

        [RelayCommand]
        public virtual async Task SubmitUpdate(T model)
        {
            await RunWithLoadingIndicator(async () =>
            {
                ValidationResults.Clear();

                SetUpdateMetadata(model);

                var validationContextCurrentType = new ValidationContext(model);

                bool isValidObj = Validator.TryValidateObject(model, validationContextCurrentType, ValidationResults, true);

                var propertyInstances = ToolsLibrary.Tools.GenericTool<T>.GetSubPropertiesOfWithForeignKeyAttribute(model);

                var validatedSubProperties = propertyInstances
                    .Where(item => !item.IsObjectNull())
                    .Select(item =>
                    {
                        var tempValidationResults = new ObservableCollection<ValidationResult>();

                        var validationContextSubProperty = new ValidationContext(item);

                        ValidationContexts.Add(validationContextSubProperty);

                        bool isValid = Validator.TryValidateObject(item, validationContextSubProperty, tempValidationResults, true);

                        ValidationResults.AddRange(tempValidationResults);

                        return isValid;
                    }).ToList();

                bool allSubPropertiesValid = validatedSubProperties.All(v => v);

                if (isValidObj && allSubPropertiesValid)
                {
                    MethodInfo updateMethod = service.GetType().GetMethod("Update");

                    if (updateMethod == null) throw new Exception("Método 'Update' no encontrado en el servicio.");

                    var _id = model.GetType().GetProperty("ID")?.GetValue(model);

                    object[] parameters = new object[] { model, _id };

                    var apiResponse = await (Task<ResponseTool<T>>)updateMethod.Invoke(service, parameters);

                    if (!apiResponse.IsSuccess) throw new Exception(apiResponse.Message);

                    await Shell.Current.DisplayAlert("Notification", apiResponse.Message, "OK");

                    WeakReferenceMessenger.Default.Send(new UpdateMessage<T>(model));

                    await Shell.Current.Navigation.PopToRootAsync();
                }
                else if (isValidObj)
                {
                    MethodInfo addMethod = service.GetType().GetMethod("Add");

                    if (addMethod == null) throw new Exception("Método 'Add' no encontrado en el servicio.");

                    var apiResponse = await (Task<ResponseTool<T>>)addMethod.Invoke(service, new object[] { model });

                    if (!apiResponse.IsSuccess) throw new Exception(apiResponse.Message);

                    if (typeof(T).GetConstructor(System.Type.EmptyTypes) == null)
                    {
                        throw new NotSupportedException($"Type {typeof(T).FullName} does not have a parameterless constructor.");
                    }

                    Activator.CreateInstance<T>();

                    await CommonsTool.DisplayAlert("Notification", apiResponse.Message);
                }
            });
        }

        [RelayCommand]
        public virtual async Task OpenFilterPopUpForSearch()
        {
            try
            {
                this._filterPopUpForSearch = new FilterPopUpForSearch(this.filterPopUpViewModel);

                await Shell.Current.CurrentPage.ShowPopupAsync(this._filterPopUpForSearch);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }
    }
}