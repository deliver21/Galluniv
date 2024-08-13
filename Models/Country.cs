using Microsoft.Build.Framework;
using System.ComponentModel;

namespace GallUni.Models
{
    public class Country
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Country's Name")]
        public string? CountryName { get; set; }
        [DisplayName("Country's Code")]
        public string? CountryCode { get; set; }
    }
}
