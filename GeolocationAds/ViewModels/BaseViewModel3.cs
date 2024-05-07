using CommunityToolkit.Mvvm.ComponentModel;
using GeolocationAds.PopUps;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using ToolsLibrary.Tools;
using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Reflection;

namespace GeolocationAds.ViewModels
{
    public partial class BaseViewModel3<T, S> : ObservableObject, IQueryAttributable
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

        public string ID { get; private set; }

        public ObservableCollection<T> CollectionModel { get; set; } = new ObservableCollection<T>();

        public ObservableCollection<ValidationResult> ValidationResults { get; set; } = new ObservableCollection<ValidationResult>();

        private ICollection<ValidationContext> ValidationContexts = new List<ValidationContext>();

        public delegate void ApplyQueryAttributesEventHandler(object sender, EventArgs e);

        public event ApplyQueryAttributesEventHandler ApplyQueryAttributesCompleted;

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

        //protected virtual async Task Get(int id)
        //{
        //    try
        //    {
        //        MethodInfo getMethod = this.service.GetType().GetMethod(nameof(Get));

        //        if (getMethod != null)
        //        {
        //            // Parameters to pass to the "Add" method
        //            object[] parameters = new object[] { id };

        //            // Call the "Add" method on the userService instance
        //            Task<ResponseTool<T>> getTask = Task.Run(async () =>
        //            {
        //                return await (Task<ResponseTool<T>>)getMethod.Invoke(this.service, parameters);
        //            });

        //            // Wait for the asynchronous task to complete
        //            ResponseTool<T> _apiResponse = await getTask;

        //            if (_apiResponse.IsSuccess)
        //            {
        //                this.Model = _apiResponse.Data;
        //            }
        //            else
        //            {
        //                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await CommonsTool.DisplayAlert("Error", ex.Message);
        //    }
        //}

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

        [RelayCommand]
        public virtual async Task Submit(T obj)
        {
            IsLoading = true;

            ValidationResults.Clear();

            try
            {
                DateTime now = DateTime.Now;

                ToolsLibrary.Tools.GenericTool<T>.SetPropertyValueOnObject(obj, nameof(BaseModel.CreateDate), now);

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

                    if (addMethod != null)
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

                                WeakReferenceMessenger.Default.Send(new CleanOnSubmitMessage<T>(this.Model));
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error", $"Type {typeof(T).FullName} does not have a parameterless constructor.", "OK");
                            }

                            await Shell.Current.DisplayAlert("Notification", apiResponse.Message, "OK");
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
    }
}