using GallUni.Models;
using GallUni.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace GallUni.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class GalleryController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public GalleryController(IUnitOfWork unit, IWebHostEnvironment webHostEnvironment)
        {
            _unit = unit;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser UserInfo = new();
            UserInfo = _unit.ApplicationUser.Get(u => u.Id == userId, includeproperties: "UserImages") ;
            UserInfo.UserImages=new List<UserImage>();
            UserInfo.UserImages=_unit.UserImage.GetAll(u=>u.ApplicationUserStudentId==userId).ToList();
            return View(UserInfo);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Index(ApplicationUser obj, List<IFormFile>? files)
        {
            //Create The Image File
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (files.Count > 0 && files != null)
            {
                foreach (var file in files)
                {
                    //Guid.NewGuid().ToString() is used to randomly name files while saving them in the folder
                    //Path.GetExtension(file.FileName) is to get the same extension that the uploaded image
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string userPath = @"images\UserImages\user-" + userId;
                    string filnalPath = Path.Combine(wwwRootPath, userPath);
                    if(!Directory.Exists(filnalPath)) 
                    { 
                        Directory.CreateDirectory(filnalPath);
                    }
                    // Create file
                    using (var fileStream = new FileStream(Path.Combine(filnalPath,filename),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    UserImage userImage = new()
                    {
                        UserImageUrl=@"\"+userPath+@"\"+filename,
                        ApplicationUserStudentId=userId,
                    };
                    //Get user from the code down cause we didn't use the VM User 
                    ApplicationUser user = _unit.ApplicationUser.Get(u => u.Id == userId);
                    // check if user is not null
                    if (user.UserImages==null)
                    {
                        user.UserImages= new List<UserImage>();
                    }
                    user.UserImages.Add(userImage);
                    //Save image in userImage table in Db
                    _unit.UserImage.Add(userImage);
                }
                _unit.Save();
                TempData["success"] = $"{files.Count} new file(s) uploaded";
                RedirectToAction("Index");
            }
            return View();
        }
        [Authorize]
        public IActionResult DeleteImage(int imageId)
        {
            var imgToBeDeleted = _unit.UserImage.Get(u => u.Id == imageId);
            if(imgToBeDeleted != null)
            {
                if(!String.IsNullOrEmpty(imgToBeDeleted.UserImageUrl))
                {
                    var oldImagePath=Path.Combine(_webHostEnvironment.WebRootPath,imgToBeDeleted.UserImageUrl.TrimStart('\\'));
                    if(System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                        _unit.UserImage.Remove(imgToBeDeleted);
                        TempData["success"] = "Image is successfully deleted";
                    }                    
                    _unit.Save();
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
