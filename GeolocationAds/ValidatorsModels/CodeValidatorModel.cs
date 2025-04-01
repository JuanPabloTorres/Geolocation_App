using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocationAds.ValidatorsModels
{
    public class CodeValidatorModel
    {
        [Required(ErrorMessage = "Code is required.")]
        [MinLength(4, ErrorMessage = "Code must be at least 4 characters.")]
        [StringLength(4, ErrorMessage = "Code must not exceed 4 characters.")]
        public string Code { get; set; }
    }
}
