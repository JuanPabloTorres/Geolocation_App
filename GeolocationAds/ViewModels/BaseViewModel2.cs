using CommunityToolkit.Mvvm.Messaging;
using GeolocationAds.Messages;
using GeolocationAds.PopUps;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ToolsLibrary.Extensions;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class BaseViewModel2<T, S> : INotifyPropertyChanged, IQueryAttributable
    {
        //public ObservableCollection<ValidationResult> ValidationResults { get; set; }

        //public ObservableCollection<T> CollectionModel { get; set; }

        protected FilterPopUp _filterPopUp;
        protected FilterPopUpForSearch _filterPopUpForSearch;

        private T _model;

        private bool isLoading;

        private ICollection<ValidationContext> ValidationContexts = new List<ValidationContext>();
        public BaseViewModel2(T model, S service, LogUserPerfilTool logUserPerfil = null)
        {
            Model = model;

            this.service = service;

            LogUserPerfilTool = logUserPerfil;

            InitializeCommands();
        }

        public delegate void ApplyQueryAttributesEventHandler(object sender, EventArgs e);

        public event ApplyQueryAttributesEventHandler ApplyQueryAttributesCompleted;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<T> CollectionModel { get; set; } = new ObservableCollection<T>();
        public string ID { get; private set; }
        //protected FilterPopUp _filterPopUp;
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

        //protected FilterPopUpForSearch _filterPopUpForSearch;
        public T Model
        {
            get => _model;
            set
            {
                if (!EqualityComparer<T>.Default.Equals(_model, value))
                {
                    _model = value;

                    OnPropertyChanged();
                }
            }
        }

        public ICommand OpenFilterPopUpCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand SubmitUpdateCommand { get; set; }

        public ObservableCollection<ValidationResult> ValidationResults { get; set; } = new ObservableCollection<ValidationResult>();

        protected LogUserPerfilTool LogUserPerfilTool { get; set; }

        //private ICollection<ValidationContext> ValidationContexts;
        protected S service { get; set; }
        //public BaseViewModel2(T model, S service)
        //{
        //    this.Model = model;

        //    this.service = service;

        //    this.ValidationResults = new ObservableCollection<ValidationResult>();

        //    this.CollectionModel = new ObservableCollection<T>();

        //    this.ValidationContexts = new List<ValidationContext>();

        //    SubmitCommand = new Command<T>(OnSubmit2);
        //}

        //public BaseViewModel2(T model, S service, LogUserPerfilTool logUserPerfil)
        //{
        //    this.Model = model;

        //    this.service = service;

        //    this.ValidationResults = new ObservableCollection<ValidationResult>();

        //    this.CollectionModel = new ObservableCollection<T>();

        //    this.ValidationContexts = new List<ValidationContext>();

        //    SubmitCommand = new Command<T>(OnSubmit2);

        //    SubmitUpdateCommand = new Command<T>(OnSubmitUpdate);

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                if (query.ContainsKey("ID"))
                {
                    ID = query["ID"].ToString();

                    if (!string.IsNullOrEmpty(ID) && Convert.ToInt32(this.ID) > 0)
                    {
                        await this.Get(Convert.ToInt32(ID));

                        OnApplyQueryAttributesCompleted(EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public async void OnSubmit(T obj)
        {
            IsLoading = true;

            try
            {
                ValidationResults.Clear();

                DateTime now = DateTime.Now;

                ToolsLibrary.Tools.GenericTool<T>.SetPropertyValueOnObject(obj, nameof(BaseModel.CreateDate), now);

                var validationContextCurrentType = new ValidationContext(obj);

                PropertyInfo[] properties = ToolsLibrary.Tools.GenericTool<T>.GetPropertiesOfType(obj).ToArray();

                ValidationContext validationContextSubProperty = null;

                object _subPropertyIntance = null;

                foreach (PropertyInfo property in properties)
                {
                    var foreignKeyAttribute = property.GetCustomAttribute<ForeignKeyAttribute>();

                    if (foreignKeyAttribute != null)
                    {
                        _subPropertyIntance = property.GetValue(obj) as object;

                        validationContextSubProperty = new ValidationContext(_subPropertyIntance);
                    }
                }

                var isValiteObj = Validator.TryValidateObject(obj, validationContextCurrentType, ValidationResults, true);

                if (!validationContextSubProperty.IsObjectNull())
                {
                    var _validationResultsSubProperty = new ObservableCollection<ValidationResult>();

                    var isValidSub = Validator.TryValidateObject(_subPropertyIntance, validationContextSubProperty, _validationResultsSubProperty, true);

                    this.ValidationResults.AddRange(_validationResultsSubProperty);

                    if (isValiteObj && isValidSub)
                    {
                        //var _apiResponse = await this.advertisementService.Add(this.Advertisement);

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

                                await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");
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
                        //var _apiResponse = await this.advertisementService.Add(this.Advertisement);

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

                                await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
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

        public async Task OnSubmit2(T obj)
        {
            try
            {
                IsLoading = true;

                ValidationResults.Clear();

                DateTime now = DateTime.Now;

                ToolsLibrary.Tools.GenericTool<T>.SetPropertyValueOnObject(obj, nameof(BaseModel.CreateDate), now);

                var validationContextCurrentType = new ValidationContext(obj);

                var isValiteObj = Validator.TryValidateObject(obj, validationContextCurrentType, ValidationResults, true);

                PropertyInfo[] properties = ToolsLibrary.Tools.GenericTool<T>.GetPropertiesOfType(obj).ToArray();

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
                                    //this.Model = Activator.CreateInstance<T>();

                                    this.ValidationResults.Clear();

                                    WeakReferenceMessenger.Default.Send(new CleanOnSubmitMessage<T>(this.Model));
                                }
                                else
                                {
                                    // Handle cases where T doesn't have a parameterless constructor
                                    //throw new NotSupportedException($"Type {typeof(T).FullName} does not have a parameterless constructor.");

                                    await Shell.Current.DisplayAlert("Error", $"Type {typeof(T).FullName} does not have a parameterless constructor.", "OK");
                                }

                                await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");
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
                                    //Activator.CreateInstance<T>();

                                    this.ValidationResults.Clear();

                                    WeakReferenceMessenger.Default.Send(new CleanOnSubmitMessage<T>(this.Model));
                                }
                                else
                                {
                                    // Handle cases where T doesn't have a parameterless constructor
                                    throw new NotSupportedException($"Type {typeof(T).FullName} does not have a parameterless constructor.");
                                }

                                //this.Image.Source = null;

                                await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task OnSubmitUpdate(T obj)
        {
            IsLoading = true;

            try
            {
                ValidationResults.Clear();

                DateTime now = DateTime.Now;

                ToolsLibrary.Tools.GenericTool<T>.SetPropertyValueOnObject(obj, nameof(BaseModel.UpdateDate), now);

                ToolsLibrary.Tools.GenericTool<T>.SetPropertyValueOnObject(obj, nameof(BaseModel.UpdateBy), this.LogUserPerfilTool.LogUser.ID);

                var validationContextCurrentType = new ValidationContext(obj);

                var isValiteObj = Validator.TryValidateObject(obj, validationContextCurrentType, ValidationResults, true);

                PropertyInfo[] properties = ToolsLibrary.Tools.GenericTool<T>.GetPropertiesOfType(obj).ToArray();

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
                                WeakReferenceMessenger.Default.Send(new UpdateMessage<T>(this.Model));

                                await Shell.Current.Navigation.PopToRootAsync();

                                await Shell.Current.DisplayAlert("Notification", _apiResponse.Message, "OK");
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

        protected virtual async Task Get(int id)
        {
            try
            {
                MethodInfo getMethod = this.service.GetType().GetMethod(nameof(Get));

                if (getMethod != null)
                {
                    // Parameters to pass to the "Add" method
                    object[] parameters = new object[] { id };

                    // Call the "Add" method on the userService instance
                    Task<ResponseTool<T>> getTask = Task.Run(async () =>
                    {
                        return await (Task<ResponseTool<T>>)getMethod.Invoke(this.service, parameters);
                    });

                    // Wait for the asynchronous task to complete
                    ResponseTool<T> _apiResponse = await getTask;

                    if (_apiResponse.IsSuccess)
                    {
                        this.Model = _apiResponse.Data;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", _apiResponse.Message, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        protected virtual async Task LoadData()
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

        protected virtual async Task LoadData(object extraData)
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

        protected virtual void OnApplyQueryAttributesCompleted(EventArgs e)
        {
            ApplyQueryAttributesCompleted?.Invoke(this, e);
        }

        protected virtual async Task OpenFilterPopUpAsync()
        {
            try
            {
                await Shell.Current.DisplayAlert("Notification", "Filter Pop Up Must Override.", "OK");
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }


        private void InitializeCommands()
        {
            SubmitCommand = new Command<T>(async (obj) => await OnSubmit2(obj));
            SubmitUpdateCommand = new Command<T>(async (obj) => await OnSubmitUpdate(obj));
            // Initialize other commands similarly
        }
    }
}