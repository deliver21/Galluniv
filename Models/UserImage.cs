using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace GallUni.Models
{
    public class UserImage
    {
        public int Id { get; set; }
        [Required]
        public string? UserImageUrl { get; set; }
        public string? ApplicationUserStudentId { get; set; }
        [ForeignKey(nameof(ApplicationUserStudentId))]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
