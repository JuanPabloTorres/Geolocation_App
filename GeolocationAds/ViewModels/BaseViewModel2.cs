using GeolocationAds.Services;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ToolsLibrary.Models;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class BaseViewModel2<T, Q> : INotifyPropertyChanged
    {
        private ObservableCollection<ValidationResult> _validationResults;

        private bool isLoading;

        public BaseViewModel2(T model, Q service)
        {
            this.model = model;

            this.service = service;

            this.ValidationResults = new ObservableCollection<ValidationResult>();

            SubmitCommand = new Command<T>(OnSubmit);
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        protected ObservableCollection<ValidationResult> ValidationResults
        {
            get => _validationResults;
            set
            {
                if (_validationResults != value)
                {
                    _validationResults = value;

                    OnPropertyChanged();
                }
            }
        }

        private T model { get; set; }

        private Q service { get; set; }

        public ICommand SubmitCommand { get; set; }

        private IUserService userService { get; set; }

        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public async void OnSubmit(T obj)
        {
            IsLoading = true;

            try
            {
                DateTime now = DateTime.Now;

                ToolsLibrary.Tools.GenericTool<T>.SetPropertyOnObject(obj, nameof(BaseModel.CreateDate), now);

                //SetPropertyOnObject(obj, nameof(BaseModel.CreateDate), now);

                var validationContext = new ValidationContext(obj);

                ValidationResults.Clear();

                if (Validator.TryValidateObject(obj, validationContext, ValidationResults, true))
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
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }

            IsLoading = false;
        }
    }
}