using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GallUni.Models
{
    public class EventImage
    {
        public int Id { get; set; }
        [Required]
        public string? EventImageUrl { get; set; }
        public int? EventImageId { get; set; }
        [ForeignKey(nameof(EventImageId))]
        [ValidateNever]
        public Event Event { get; set; }
    }
}
