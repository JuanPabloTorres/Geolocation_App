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

namespace GeolocationAds.ViewModels
{
    public partial class BaseViewModel3<T, S> : ObservableObject, IQueryAttributable
    {
        [ObservableProperty]
        private T model;

        [ObservableProperty]
        private bool isLoading;

        protected FilterPopUp _filterPopUp;

        protected FilterPopUpForSearch _filterPopUpForSearch;

        protected MetaDataPopUp _metaDataPopUp;

        protected CompletePopUp _completePopUp;

        protected FilterPopUpViewModel2 filterPopUpViewModel;

        protected LogUserPerfilTool LogUserPerfilTool { get; set; }

        public static int PageIndex { get; set; } = 1;

        protected S service { get; set; }

        public string ID { get; private set; }

        public ObservableCollection<T> CollectionModel { get; set; } = new ObservableCollection<T>();

        public ObservableCollection<ValidationResult> ValidationResults { get; set; } = new ObservableCollection<ValidationResult>();

        private ICollection<ValidationContext> ValidationContexts = new List<ValidationContext>();

        public delegate void ApplyQueryAttributesEventHandler(object sender, EventArgs e);

        public event ApplyQueryAttributesEventHandler ApplyQueryAttributesCompleted;

        public BaseViewModel3(T model, S service, LogUserPerfilTool logUserPerfil = null)
        {
            Model = model;

            this.service = service;

            LogUserPerfilTool = logUserPerfil;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                this.IsLoading = true;

                if (query.ContainsKey("ID") && !string.IsNullOrEmpty(query["ID"].ToString()))
                {
                    var _itemId = Convert.ToInt32(query["ID"].ToString());

                    if (_itemId > 0)
                    {
                        await this.Get(_itemId);

                        OnApplyQueryAttributesCompleted(EventArgs.Empty);
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

        protected virtual void OnApplyQueryAttributesCompleted(EventArgs e)
        {
            ApplyQueryAttributesCompleted?.Invoke(this, e);
        }

        protected virtual async Task Get(int id)
        {
            try
            {
                this.IsLoading = true;

                // Cache the MethodInfo if this method is called frequently
                MethodInfo getMethod = this.service.GetType().GetMethod(nameof(Get));

                if (getMethod != null)
                {
                    object[] parameters = new object[] { id };

                    // Invoke the method directly and await the response
                    var responseTask = (Task<ResponseTool<T>>)getMethod.Invoke(this.service, parameters);

                    ResponseTool<T> _apiResponse = await responseTask;

                    if (!_apiResponse.IsSuccess)
                    {
                        await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");

                        return;
                    }

                    this.Model = _apiResponse.Data;
                }
            }
            catch (TargetInvocationException tie)
            {
                // Handle invocation-specific exceptions
                await CommonsTool.DisplayAlert("Error", tie.InnerException?.Message ?? tie.Message);
            }
            catch (Exception ex)
            {
                // General exception handling
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                this.IsLoading = false;
            }
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

        private void SetUpdateMetadata(T obj)
        {
            DateTime now = DateTime.Now;
            ToolsLibrary.Tools.GenericTool<T>.SetPropertyValueOnObject(obj, nameof(BaseModel.UpdateDate), now);
            ToolsLibrary.Tools.GenericTool<T>.SetPropertyValueOnObject(obj, nameof(BaseModel.UpdateBy), this.LogUserPerfilTool.LogUser.ID);
        }

        [RelayCommand]
        public virtual async Task Submit(T obj)
        {
            IsLoading = true;

            ValidationResults.Clear();

            try
            {
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
                                if (this.ValidationResults.Count > 0)
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
            }
            catch (Exception ex)
            {
                // Consider using a logging framework to log the full details of the exception.
                //Debug.WriteLine($"An error occurred: {ex}");

                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public virtual async Task SubmitUpdate(T obj)
        {
            IsLoading = true;

            try
            {
                ValidationResults.Clear();

                SetUpdateMetadata(obj);

                var validationContextCurrentType = new ValidationContext(obj);

                var isValiteObj = Validator.TryValidateObject(obj, validationContextCurrentType, ValidationResults, true);

                var _propertyIntances = ToolsLibrary.Tools.GenericTool<T>.GetSubPropertiesOfWithForeignKeyAttribute(obj);

                var _validatedSubProperty = new List<bool>();

                foreach (var item in _propertyIntances)
                {
                    if (!item.IsObjectNull())
                    {
                        var _tempValidationResultsSubProperty = new ObservableCollection<ValidationResult>();

                        var validationContextSubProperty = new ValidationContext(item);

                        this.ValidationContexts.Add(validationContextSubProperty);

                        _validatedSubProperty.Add(Validator.TryValidateObject(item, validationContextSubProperty, _tempValidationResultsSubProperty, true));

                        this.ValidationResults.AddRange(_tempValidationResultsSubProperty);
                    }
                }

                if (_validatedSubProperty.Count >= 0)
                {
                    bool _allSubPropetyValueAreValid = _validatedSubProperty.All(v => v == true);

                    if (isValiteObj && _allSubPropetyValueAreValid)
                    {
                        MethodInfo updateMethod = this.service.GetType().GetMethod("Update");

                        if (updateMethod != null)
                        {
                            // Parameters to pass to the "Add" method

                            var _id = obj.GetType().GetProperties().Where(p => p.Name == "ID").FirstOrDefault().GetValue(obj);

                            object[] parameters = new object[] { obj, _id };

                            // Call the "Add" method on the userService instance
                            Task<ResponseTool<T>> updateTask = Task.Run(async () =>
                            {
                                return await (Task<ResponseTool<T>>)updateMethod.Invoke(this.service, parameters);
                            });

                            // Wait for the asynchronous task to complete
                            ResponseTool<T> _apiResponse = await updateTask;

                            if (_apiResponse.IsSuccess)
                            {
                                await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");

                                WeakReferenceMessenger.Default.Send(new UpdateMessage<T>(this.Model));

                                await Shell.Current.Navigation.PopToRootAsync();
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                            }
                        }
                    }
                }
                else
                {
                    if (isValiteObj)
                    {
                        MethodInfo addMethod = this.service.GetType().GetMethod("Add");

                        if (addMethod != null)
                        {
                            // Parameters to pass to the "Add" method
                            object[] parameters = new object[] { obj };

                            // Call the "Add" method on the userService instance
                            Task<ResponseTool<T>> addTask = Task.Run(async () =>
                            {
                                return await (Task<ResponseTool<T>>)addMethod.Invoke(this.service, parameters);
                            });

                            // Wait for the asynchronous task to complete
                            ResponseTool<T> _apiResponse = await addTask;

                            if (_apiResponse.IsSuccess)
                            {
                                if (typeof(T).GetConstructor(System.Type.EmptyTypes) != null)
                                {
                                    Activator.CreateInstance<T>();
                                }
                                else
                                {
                                    // Handle cases where T doesn't have a parameterless constructor
                                    throw new NotSupportedException($"Type {typeof(T).FullName} does not have a parameterless constructor.");
                                }

                                //this.Image.Source = null;

                                await CommonsTool.DisplayAlert("Notification", _apiResponse.Message);
                            }
                            else
                            {
                                await CommonsTool.DisplayAlert("Error", _apiResponse.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }

            IsLoading = false;
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