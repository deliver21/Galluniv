using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GallUni.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Faculty's Name")]
        [MinLength(3, ErrorMessage = "The Faculty Name is too Short [At least 3 letters are required]")]
        public string FacultyName { get; set; }
        [DisplayName("Dean's Name")]
        public string? FacultyDean { get; set; }
    }
}
