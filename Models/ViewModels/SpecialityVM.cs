using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GallUni.Models.ViewModels
{
    public class SpecialityVM
    {
        public Speciality Speciality { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> FacultiesList { get; set; }   
    }
}
