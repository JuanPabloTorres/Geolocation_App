using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ToolsLibrary.Enums;

namespace ToolsLibrary.Models
{
    public partial class Login : BaseModel
    {
        public Login()
        {
        }

        public string? GoogleId { get; set; }
        
        public string? FacebookId { get; set; }

        public string HashPassword { get; set; }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = $"{nameof(Password)} is required.")]
        public string? _password;

        public Providers Provider { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }

        public int UserId { get; set; }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = $"{nameof(Username)} is required.")]
        public string? _username;

    }
}