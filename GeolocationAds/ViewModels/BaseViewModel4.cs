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
    /// <summary>
    /// Clase base genérica para ViewModels que proporciona lógica reutilizable para operaciones comunes.
    /// </summary>
    /// <typeparam name="T">
    /// Tipo del modelo manejado por el ViewModel.
    /// </typeparam>
    /// <typeparam name="S">
    /// Tipo del servicio asociado al modelo.
    /// </typeparam>
    public abstract partial class BaseViewModel4<T, S> : ObservableObject, IQueryAttributable
    {
        #region Propiedades protegidas (no observables directamente)

        /// <summary>
        /// Modelo principal manejado por el ViewModel. Los derivados deben sincronizarlo con su
        /// propia propiedad observable si es necesario.
        /// </summary>
        protected T Model { get; set; }

        /// <summary>
        /// Servicio asociado al modelo.
        /// </summary>
        protected S Service { get; }

        /// <summary>
        /// Herramienta para manejar el perfil del usuario logueado.
        /// </summary>
        protected LogUserPerfilTool LogUserPerfilTool { get; }

        /// <summary>
        /// Colección de modelos para listas o resultados paginados.
        /// </summary>
        public ObservableCollection<T> CollectionModel { get; } = new ObservableCollection<T>();

        /// <summary>
        /// Resultados de validación para el modelo y sus subpropiedades.
        /// </summary>
        public ObservableCollection<ValidationResult> ValidationResults { get; } = new ObservableCollection<ValidationResult>();

        #endregion Propiedades protegidas (no observables directamente)

        #region Propiedades observables

        [ObservableProperty]
        private bool isLoading;

        #endregion Propiedades observables

        #region Campos protegidos para popups

        protected FilterPopUp FilterPopUp;

        protected FilterPopUpForSearch FilterPopUpForSearch;

        protected MetaDataPopUp MetaDataPopUp;

        protected CompletePopUp CompletePopUp;

        protected FilterPopUpViewModel2 FilterPopUpViewModel;

        #endregion Campos protegidos para popups

        #region Eventos

        public event EventHandler<EventArgs> ApplyQueryAttributesCompleted;

        #endregion Eventos

        #region Constructor

        /// <summary>
        /// Constructor base para inicializar el ViewModel.
        /// </summary>
        /// <param name="service">
        /// Servicio para operaciones de datos.
        /// </param>
        /// <param name="logUserPerfilTool">
        /// Herramienta de perfil de usuario (opcional).
        /// </param>
        protected BaseViewModel4(S service, LogUserPerfilTool logUserPerfilTool = null)
        {
            Service = service;

            LogUserPerfilTool = logUserPerfilTool;
        }

        /// <summary>
        /// Constructor base para inicializar el ViewModel.
        /// </summary>
        /// <param name="service">
        /// Servicio para operaciones de datos.
        /// </param>
        protected BaseViewModel4(S service)
        {
            Service = service;
        }

        #endregion Constructor

        #region Métodos de navegación y queries

        /// <summary>
        /// Aplica parámetros de navegación desde una query.
        /// </summary>
        public virtual async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                IsLoading = true;
                if (query.TryGetValue("ID", out var idValue) && int.TryParse(idValue?.ToString(), out int id) && id > 0)
                {
                    await Get(id);
                    OnApplyQueryAttributesCompleted(EventArgs.Empty);
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

        protected virtual void OnApplyQueryAttributesCompleted(EventArgs e)
        {
            ApplyQueryAttributesCompleted?.Invoke(this, e);
        }

        #endregion Métodos de navegación y queries

        #region Métodos de operaciones de datos

        /// <summary>
        /// Obtiene un modelo específico por ID.
        /// </summary>
        protected virtual async Task Get(int id)
        {
            try
            {
                IsLoading = true;

                var getMethod = Service.GetType().GetMethod(nameof(Get));

                if (getMethod != null)
                {
                    var responseTask = (Task<ResponseTool<T>>)getMethod.Invoke(Service, new object[] { id });

                    var response = await responseTask;

                    if (response.IsSuccess)
                    {
                        Model = response.Data;
                    }
                    else
                    {
                       throw new Exception(response.Message);
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

        /// <summary>
        /// Carga datos en la colección, opcionalmente con paginación.
        /// </summary>
        protected virtual async Task LoadData(int? pageIndex = 1)
        {
            IsLoading = true;
            try
            {
                // Implementación por defecto (puedes sobreescribirla en derivados)
                await Task.CompletedTask;
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

        /// <summary>
        /// Carga datos con información adicional.
        /// </summary>
        protected virtual async Task LoadData(object extraData, int? pageIndex = 1)
        {
            IsLoading = true;
            try
            {
                // Implementación por defecto
                await Task.CompletedTask;
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

        #endregion Métodos de operaciones de datos

        #region Métodos de popups

        /// <summary>
        /// Abre un popup de metadatos (debe ser sobreescrito si es específico).
        /// </summary>
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
        public virtual async Task OpenFilterPopUpForSearch()
        {
            try
            {
                FilterPopUpForSearch = new FilterPopUpForSearch(FilterPopUpViewModel);
                await Shell.Current.CurrentPage.ShowPopupAsync(FilterPopUpForSearch);
            }
            catch (Exception ex)
            {
                await CommonsTool.DisplayAlert("Error", ex.Message);
            }
        }

        #endregion Métodos de popups

        #region Métodos de actualización y envío

        /// <summary>
        /// Establece metadatos de actualización en el modelo.
        /// </summary>
        protected void SetUpdateMetadata(T obj)
        {
            var now = DateTime.Now;
            GenericTool<T>.SetPropertyValueOnObject(obj, nameof(BaseModel.UpdateDate), now);
            GenericTool<T>.SetPropertyValueOnObject(obj, nameof(BaseModel.UpdateBy), LogUserPerfilTool?.LogUser.ID ?? 0);
        }

        /// <summary>
        /// Envía un nuevo modelo al servicio.
        /// </summary>
        [RelayCommand]
        public virtual async Task Submit(T obj)
        {
            try
            {
                IsLoading = true;

                ValidationResults.Clear();

                var validationContext = new ValidationContext(obj);

                bool isValid = Validator.TryValidateObject(obj, validationContext, ValidationResults, true);

                var subProperties = GenericTool<T>.GetSubPropertiesOfWithForeignKeyAttribute(obj);

                bool allSubValid = ValidateSubProperties(subProperties);

                if (isValid && allSubValid)
                {
                    var addMethod = Service.GetType().GetMethod("Add");

                    if (addMethod != null)
                    {
                        var response = await (Task<ResponseTool<T>>)addMethod.Invoke(Service, new object[] { obj });

                        if (response.IsSuccess)
                        {
                            await Shell.Current.CurrentPage.ShowPopupAsync(new CompletePopUp());

                            WeakReferenceMessenger.Default.Send(new CleanOnSubmitMessage<T>(Model));

                            await Shell.Current.Navigation.PopToRootAsync();
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Error", response.Message, "OK");
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

        [RelayCommand]
        public virtual async Task SubmitUpdate(T obj)
        {
            try
            {
                IsLoading = true;

                ValidationResults.Clear();

                SetUpdateMetadata(obj);

                var validationContext = new ValidationContext(obj);

                bool isValid = Validator.TryValidateObject(obj, validationContext, ValidationResults, true);

                var subProperties = GenericTool<T>.GetSubPropertiesOfWithForeignKeyAttribute(obj);

                bool allSubValid = ValidateSubProperties(subProperties);

                if (isValid && allSubValid)
                {
                    var updateMethod = Service.GetType().GetMethod("Update");

                    if (updateMethod != null)
                    {
                        var id = obj.GetType().GetProperty("ID")?.GetValue(obj);

                        var response = await (Task<ResponseTool<T>>)updateMethod.Invoke(Service, new object[] { obj, id });

                        if (response.IsSuccess)
                        {
                            await Shell.Current.DisplayAlert("Notification", response.Message, "OK");

                            WeakReferenceMessenger.Default.Send(new UpdateMessage<T>(Model));

                            await Shell.Current.Navigation.PopToRootAsync();
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Error", response.Message, "OK");
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

        private bool ValidateSubProperties(IEnumerable<object> subProperties)
        {
            bool allValid = true;
            foreach (var item in subProperties)
            {
                if (!item.IsObjectNull())
                {
                    var tempResults = new List<ValidationResult>();
                    var subContext = new ValidationContext(item);
                    if (!Validator.TryValidateObject(item, subContext, tempResults, true))
                    {
                        allValid = false;
                    }
                    ValidationResults.AddRange(tempResults);
                }
            }
            return allValid;
        }

        #endregion Métodos de actualización y envío
    }
}