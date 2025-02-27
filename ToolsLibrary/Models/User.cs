using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToolsLibrary.Models
{
    public enum UserStatus
    {
        ResetPassword = 1,
        InvalidUser = 2,
        ValidUser = 3,
        Active = 4,
        CancelSubscription = 5,
        Subscribe = 6
    }

    //public partial class User : BaseModel
    //{
    //    public ICollection<Advertisement> Advertisements { get; set; }

    //    [Required(ErrorMessage = $"{nameof(Email)} is required.")]
    //    public string Email { get; set; }

    //    [Required(ErrorMessage = $"{nameof(FullName)} is required.")]
    //    public string FullName { get; set; }

    //    [ForeignKey("LoginId")]
    //    public virtual Login Login { get; set; }

    //    public int? LoginId { get; set; }

    //    [ObservableProperty]
    //    [NotifyDataErrorInfo]
    //    [Required(ErrorMessage = $"{nameof(Phone)} is required.")]
    //    public string _phone;

    //    public UserStatus UserStatus { get; set; }

    //    public User()
    //    {
    //        Advertisements = new List<Advertisement>();
    //    }
    //}

 

public partial class User : BaseModel
    {
        public User()
        {
            Advertisements = new List<Advertisement>();
        }

        // 🔹 Lista de anuncios del usuario
        public ICollection<Advertisement> Advertisements { get; set; }

        // 🔹 ID de login (clave foránea)
        [ForeignKey("LoginId")]
        public virtual Login Login { get; set; }

        [ObservableProperty]
        private int? loginId;

        // 🔹 Nombre completo con validación
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = $"{nameof(FullName)} is required.")]
        private string fullName;

        // 🔹 Correo electrónico con validación
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = $"{nameof(Email)} is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        private string email;

        // 🔹 Número de teléfono con validación
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = $"{nameof(Phone)} is required.")]
        private string phone;

        // 🔹 Estado del usuario
        [ObservableProperty]
        private UserStatus userStatus;
    }

}