using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsLibrary.Dto
{
    /// <summary>
    /// Modelo de datos para almacenar la información del usuario autenticado con Google.
    /// </summary>
    public class GoogleUserInfoDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
    }
}
