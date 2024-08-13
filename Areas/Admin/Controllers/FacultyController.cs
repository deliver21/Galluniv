using Microsoft.AspNetCore.Mvc;
using GallUni.Repository;
using GallUni.Models;
using Microsoft.AspNetCore.Hosting;

namespace GallUni.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FacultyController : Controller
    {
        private readonly IUnitOfWork _unit;
        public FacultyController(IUnitOfWork unit) {
            _unit=unit;
        }
        public IActionResult Index()
        {           
            List<Faculty> faculties= new List<Faculty>();
            faculties=_unit.Faculty.GetAll().ToList();
            return View(faculties);
        }
        public IActionResult Upsert(int ? id)
        {
            if(ModelState.IsValid)
            {
                Faculty faculty = new();
                if (id == null || id == 0)
                {
                    return View(faculty);
                }
                else
                {
                    faculty = _unit.Faculty.Get(u => u.Id == id);
                    return View(faculty);
                }
            }            
                return RedirectToAction(nameof(Index));    
        }
        [HttpPost]
        public IActionResult Upsert(Faculty faculty)
        {   
            if(String.IsNullOrEmpty(faculty.FacultyName) || String.IsNullOrWhiteSpace(faculty.FacultyName))
            {
                ModelState.AddModelError("FacultyName", "The input field is empty");
            }
            if(ModelState.IsValid)
            {
                if(faculty.Id==0)
                {
                    _unit.Faculty.Add(faculty);
                    _unit.Save();
                    TempData["success"] = "Faculty is successfully added";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    _unit.Faculty.Update(faculty);
                    _unit.Save();
                    TempData["success"] = "Faculty is successfully updated";
                    return RedirectToAction(nameof(Index));
                }                
            }
            return View(faculty);
        }
        #region API 
        public IActionResult GetAll()
        {
            List<Faculty> obj = _unit.Faculty.GetAll().ToList();
            return View("Index",obj);
        }
        public IActionResult Delete(int? id)
        {
            Faculty facultyTobeDelete=_unit.Faculty.Get(u=>u.Id==id);
            if(facultyTobeDelete == null)
            {
                return Json(new { success = false, message = "Error While deleting" });
            }
            else
            {
                _unit.Faculty.Remove(facultyTobeDelete);
                _unit.Save();
                List<Faculty> obj = _unit.Faculty.GetAll().ToList();
            }
            return Json(new { success = true, message = "Deletion successfully performed" });
        }
        #endregion
    }
}
