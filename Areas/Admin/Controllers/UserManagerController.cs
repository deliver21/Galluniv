using GallUni.Models;
using GallUni.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GallUni.AppUtilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace GallUni.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserManagerController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<IdentityUser> _userManager;
        public UserManagerController (IUnitOfWork unit, UserManager<IdentityUser> userManager) 
        { 
            _unit = unit;
            _userManager = userManager;
        }
        public IActionResult Index()
        {            
            return View();
        }
        #region API Calls
        public IActionResult GetAll()
        {
            //Necessary to Display faculty in Speciality table
            IEnumerable<SelectListItem> FacultyList = _unit.Faculty.GetAll().Select(u => new SelectListItem
            {
                Text = u.FacultyName,
                Value = u.Id.ToString()
            });
            IEnumerable<SelectListItem> CountryList = _unit.Country.GetAll().Select(u => new SelectListItem
            {
                Text = u.CountryName,
                Value = u.Id.ToString()
            });
            IEnumerable<SelectListItem> SpecialityList = _unit.Speciality.GetAll().Select(u => new SelectListItem
            {
                Text = u.SpecialityName,
                Value = u.Id.ToString()
            });

            List<ApplicationUser> users = new List<ApplicationUser>();
            users = _unit.ApplicationUser.GetAll(includeproperties:"Country,Faculty,Speciality").ToList();
            if (users != null)
            {
                List<ApplicationUser> students = new List<ApplicationUser>();
                foreach (var user in users)
                {
                    if (_userManager.IsInRoleAsync(user, SD.Role_Student).Result)
                    {                        
                        students.Add(user);
                    }
                }                

                return Json(new { data = students });
            }
            return NotFound();
        }
        #endregion
    }
}
