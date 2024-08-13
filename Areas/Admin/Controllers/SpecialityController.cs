using GallUni.Models;
using GallUni.Models.ViewModels;
using GallUni.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;

namespace GallUni.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialityController : Controller
    {
        private readonly IUnitOfWork _unit;
        public SpecialityController(IUnitOfWork unit) 
        { 
            _unit = unit;
        }
        public IActionResult Index()
        {   
            List<Speciality> specialities = _unit.Speciality.GetAll(includeproperties:"Faculty").ToList();
            if (specialities != null)
            {
                //Necessary to Display faculty in Speciality table
                IEnumerable<SelectListItem> FacultyList = _unit.Faculty.GetAll().Select(u => new SelectListItem
                {
                    Text = u.FacultyName,
                    Value = u.Id.ToString()
                }); ;
                return View(specialities);
            }
            return View();
        }
        public IActionResult Upsert(int ? id)
        {
            SpecialityVM specialityVM = new SpecialityVM()
            {
                FacultiesList = _unit.Faculty.GetAll().Select(u=>new SelectListItem
                {
                    Text=u.FacultyName,
                    Value=u.Id.ToString()
                }),
                Speciality = new Speciality()
            };
            if(id==0 || id==null)
            {
                //Create
                return View(specialityVM);
            }
            else
            {
                //Update
                specialityVM.Speciality=_unit.Speciality.Get(u=>u.Id==id);
                return View(specialityVM);  
            }
        }
        [HttpPost]
        public IActionResult Upsert(SpecialityVM specialityVM)
        {
            if (specialityVM.Speciality.SpecialityName == null || String.IsNullOrEmpty(specialityVM.Speciality.SpecialityName) || String.IsNullOrWhiteSpace(specialityVM.Speciality.SpecialityName))
            {
                ModelState.AddModelError("SpecialityName","The speciality field is required");
            }
            if(specialityVM.Speciality.FacultyId==0)
            {
                ModelState.AddModelError("FacultyID", "The Faculty is required");
            }
            if(ModelState.IsValid)
            {
                if(specialityVM.Speciality.Id==0 )
                {
                    _unit.Speciality.Add(specialityVM.Speciality);
                    _unit.Save();
                    TempData["success"] = "Speciality is successfully added.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _unit.Speciality.Update(specialityVM.Speciality);
                    _unit.Save();
                    TempData["success"] = "Speciality is successfully updated.";
                    return RedirectToAction(nameof(Index));
                }
            }
            specialityVM.FacultiesList = _unit.Faculty.GetAll().Select(u => new SelectListItem
            {
                Text = u.FacultyName,
                Value = u.Id.ToString()
            });
            return View(specialityVM);
        }
        #region API 
        public IActionResult GetAll()
        {
            List<Speciality> obj = _unit.Speciality.GetAll(includeproperties: "Faculty").ToList();
            return View("Index",obj);
        }
        public IActionResult Delete(int? id)
        {
            Speciality specialityTobeDelete = _unit.Speciality.Get(u => u.Id == id);
            if (specialityTobeDelete == null)
            {
                return Json(new { success = false, message = "Error While deleting" });
            }
            else
            {
                _unit.Speciality.Remove(specialityTobeDelete);
                _unit.Save();
            }
            return Json(new { success = true, message = "Speciality successfully deleted" });
        }
        #endregion
    }
}
