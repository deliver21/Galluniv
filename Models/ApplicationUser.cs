using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GallUni.Models
{
    public class ApplicationUser:IdentityUser
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required field")]
        [MinLength(3, ErrorMessage = "The surname is too short ")]
        [MaxLength(15, ErrorMessage = "The surname too long")]
        public string Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required field")]
        [MinLength(3,ErrorMessage ="The surname is too short ")]
        [MaxLength(15,ErrorMessage ="The surname too long")]
        public string Surname { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required field")]
        public int? CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        [ValidateNever]
        public Country? Country { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? City { get; set; }
        public string? MotherTongue { get; set; }
        public DateOnly? Birthday { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string Gender { get; set; }
        public string? SocialMediaId { get; set; }

        public int? FacultyId { get; set; }
        [ForeignKey(nameof(FacultyId))]
        [ValidateNever]
        public Faculty? Faculty { get; set; }
        
        public int? SpecialityId { get; set; }
        [ForeignKey(nameof(SpecialityId))]
        [ValidateNever]
        public Speciality? Speciality { get; set; }

        public string? Status { get; set; }
        [ValidateNever]
        public List<UserImage>? UserImages { get; set; }
        public int? TagId { get; set; }
        [ForeignKey(nameof(TagId))]
        [ValidateNever]
        public Tag? Tag { get; set; }
    }
}
