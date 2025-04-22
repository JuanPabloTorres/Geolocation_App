using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToolsLibrary.Attributes.ValidationAttributes;

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

    public partial class User : BaseModel
    {
        // 🔹 Correo electrónico con validación
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = $"{nameof(Email)} is required.")]
        [EmailValidation(ErrorMessage = "Invalid email format.")]
        public string email;

        // 🔹 Nombre completo con validación
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = $"{nameof(FullName)} is required.")]
        public string fullName;

        // 🔹 Número de teléfono con validación
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = $"{nameof(Phone)} is required.")]
        public string phone;

        // Imagen como arreglo de bytes
        [ObservableProperty]
        public byte[]? profileImageBytes;

        // 🔹 Estado del usuario
        [ObservableProperty]
        public UserStatus userStatus;

        private int? loginId;

        public User()
        {
            Advertisements = new List<Advertisement>();

            Login = new Login();
        }

        // 🔹 Lista de anuncios del usuario
        public ICollection<Advertisement> Advertisements { get; set; }

        // 🔹 ID de login (clave foránea)
        [ForeignKey("LoginId")]
        public virtual Login Login { get; set; }

        // 🔹 Relación con AppSetting para el rol
        [ForeignKey("UserRoleId")]
        public AppSetting? UserRole { get; set; }

        public int? UserRoleId { get; set; }

        public string GetUserRoleName() => UserRole?.Value ?? "Undefined";

        public bool HasProfileImage()
        {
            return profileImageBytes != null || profileImageBytes?.Length > 0;
        }

        public bool HasRole(string roleValue)
        {
            return string.Equals(this.UserRole?.Value, roleValue, StringComparison.OrdinalIgnoreCase);
        }

        public string GetRoleName()
        {
            return this.UserRole?.Value ?? "Undefined";
        }
    }
}