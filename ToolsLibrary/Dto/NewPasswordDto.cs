using ToolsLibrary.Attributes.ValidationAttributes;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace ToolsLibrary.Dto
{
    public class NewPasswordDto
    {
        [Required(ErrorMessage = "Password is required")]
        public string NewPassword { get; set; }

        [ConfirmPassword(nameof(NewPassword), ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }

        public string Code { get; set; }
    }
}