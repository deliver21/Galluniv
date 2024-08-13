using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GallUni.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        public string? Description { get; set; }
        public DateOnly? EventDate { get; set; } 
        public DateTime? EventPostDate { get; set; } 
        public int EventCategoryId { get; set; }
        [ForeignKey(nameof(EventCategoryId))]
        [ValidateNever]
        public EventCategory? EventCategory { get; set; }
        public List<EventImage>? EventImages { get; set; }
    }
}
