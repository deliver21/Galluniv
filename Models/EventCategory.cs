using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace GallUni.Models
{
    public class EventCategory
    {
        [Key]
        public int CategoryId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string? CategoryName { get; set; }    
    }
}
