using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace GallUni.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        public string ? TagName { get; set; }
        public List<ApplicationUser> ? ApplicationUsers { get; set; }
        public int? EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        [ValidateNever]
        public Event Event { get; set; }
    }
}
