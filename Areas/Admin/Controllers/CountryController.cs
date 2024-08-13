using GallUni.Models;
using GallUni.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace GallUni.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountryController : Controller
    {
        private readonly IUnitOfWork _unit;
        public CountryController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        public IActionResult Index()
        {
            List<Country> countries = _unit.Country.GetAll().ToList();
            return View(countries);
        }

        public IActionResult Upsert(int? id)
        {            
                Country country = new();
                if (id == null || id == 0)
                {
                    return View(country);
                }
                else
                {
                    country = _unit.Country.Get(u => u.Id == id);
                    return View(country);
                }
        }
        public static string FormatCountryName(string CountryName)
        {
            string [] chuncks = CountryName.Split();
            string formatedString="";
            foreach(var subS in chuncks)
            {
                if(!String.IsNullOrEmpty(subS))
                {
                    string chunk = subS[0].ToString().ToUpper() + subS.Remove(0, 1).ToString().ToLower() + " ";
                    formatedString += chunk;
                }                
            }
            return formatedString;
        }
        [HttpPost]
        public IActionResult Upsert(Country country)
        {
            if (String.IsNullOrEmpty(country.CountryName) || String.IsNullOrWhiteSpace(country.CountryName))
            {
                ModelState.AddModelError("CountryName", "The input field is required");
            }
            if (ModelState.IsValid)
            {
                if(country.CountryCode!=null)
                {
                    country.CountryCode = country.CountryCode.ToUpper();
                }  
                if(country.CountryName!=null)
                {
                    country.CountryName = FormatCountryName(country.CountryName);
                }
                if (country.Id == 0)
                {
                    _unit.Country.Add(country);
                    _unit.Save();
                    TempData["success"] = "Country is successfully added";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    _unit.Country.Update(country);
                    _unit.Save();
                    TempData["success"] = "Country is successfully updated";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(country);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Country> obj = _unit.Country.GetAll().ToList();
            return Json(new { data = obj });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Country productToBeDeleted = _unit.Country.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            else
            {
                _unit.Country.Remove(productToBeDeleted);
                _unit.Save();
            }
            return Json(new { success = true, message = "Country succesfully deleted" });
        }
        #endregion
    }
}
