using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using GeolocationAds.PopUps;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary.Tools;

namespace GeolocationAds.ViewModels
{
    public partial class RootBaseViewModel : ObservableValidator, IQueryAttributable
    {
        [ObservableProperty]
        public bool isLoading;

        public static int PageIndex { get; set; } = 1;

        public string ID { get; private set; }

        public string Avatar;

        public ObservableCollection<ValidationResult> ValidationResults { get; set; } = new();

        protected ICollection<ValidationContext> ValidationContexts = new List<ValidationContext>();

        protected LogUserPerfilTool LogUserPerfilTool { get; set; }

        [ObservableProperty]
        public bool hasProfileImage;

        [ObservableProperty]
        public ImageSource profileImage;

        [ObservableProperty]
        public bool isInternalUser;

        public RootBaseViewModel(LogUserPerfilTool logUserPerfilTool)
        {
            LogUserPerfilTool = logUserPerfilTool;
        }

        public RootBaseViewModel()
        {
        }

        /// <summary>
        /// Acción que se ejecuta cuando ApplyQueryAttributes finaliza.
        /// </summary>
        public Action ApplyQueryAttributesCompleted { get; set; }

        /// <summary>
        /// Método para ejecutar una tarea con indicador de carga activado.
        /// </summary>
        public async Task RunWithLoadingIndicator(Func<Task> action, Action<Exception> onError = null)
        {
            try
            {
                this.IsLoading = true;

                await action();
            }
            catch (Exception ex)
            {
                if (onError != null)
                {
                    onError(ex); // 🔹 Manejo personalizado de errores
                }
                else
                {
                    //await CommonsTool.DisplayAlert("Error", ex.Message);

                    await Shell.Current.CurrentPage.ShowPopupAsync(new NotFoundPopup(ex.Message));
                }
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        /// <summary>
        /// Procesa atributos de consulta y ejecuta la acción de finalización.
        /// </summary>
        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            await RunWithLoadingIndicator(async () =>
            {
                if (query.ContainsKey("ID") && !string.IsNullOrEmpty(query["ID"].ToString()))
                {
                    int _itemId = Convert.ToInt32(query["ID"].ToString());

                    if (_itemId > 0)
                    {
                        await Get(_itemId);

                        ApplyQueryAttributesCompleted?.Invoke(); // Ejecuta la acción
                    }
                }
            });
        }

        /// <summary>
        /// Método virtual para obtener datos basado en ID (para ser sobreescrito en los ViewModels).
        /// </summary>
        protected virtual async Task Get(int id) => await Task.CompletedTask;

        /// <summary>
        /// Método de validación global para cualquier modelo.
        /// </summary>
        protected bool ValidateModel(object obj)
        {
            if (ValidationResults.Any())
                ValidationResults.Clear();

            var validationContext = new ValidationContext(obj);

            return Validator.TryValidateObject(obj, validationContext, ValidationResults, true);
        }
    }
}