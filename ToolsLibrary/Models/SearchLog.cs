using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolsLibrary.Models
{
    public class SearchLog : BaseModel
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string SearchedType { get; set; }

        public DateTime SearchDate { get; set; } = DateTime.UtcNow;
    }
}