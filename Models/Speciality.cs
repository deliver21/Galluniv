using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GallUni.Models
{
    public class Speciality
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Speciality's Name")]
        public string SpecialityName { get; set; }
        [Required]
        [DisplayName("Faculty's Name")]
        public int FacultyId { get; set; }
        [ForeignKey(nameof(FacultyId))]
        [ValidateNever]
        public Faculty Faculty { get; set; }

    }
}
