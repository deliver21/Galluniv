// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using GallUni.AppUtilities;
using GallUni.Models;
using GallUni.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace GallUni.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        //WebHost Interface
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unit;
        private readonly SignInManager<IdentityUser> _signInManager;
        //Inject Role Manager
        private readonly RoleManager<IdentityRole> _roleManager;  
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            IUnitOfWork unit, IWebHostEnvironment webHostEnvironment,
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,RoleManager<IdentityRole> roleManager)
        {
            _webHostEnvironment = webHostEnvironment;
            _unit = unit;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            // Adding more property to the inputmodel and to eventually get the role
            [ValidateNever]
            public string? Role { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; } 

            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required field")]
            [MinLength(3, ErrorMessage = "The surname is too short ")]
            [MaxLength(15, ErrorMessage = "The surname too long")]            
            public string? Name { get; set; }

            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required field")]
            [MinLength(3, ErrorMessage = "The surname is too short ")]
            [MaxLength(15, ErrorMessage = "The surname too long")]
            public string? Surname { get; set; }

            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Required field")]
            public int? CountryId { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> CountryList { get; set; }
            public string? City { get; set; }
            public string? ProfilePictureUrl { get; set; }
            public string? MotherTongue { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> MotherTongueList { get; set; }
            public DateOnly? Birthday { get; set; }
            [Required]
            public string Gender { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> GenderList { get; set; }
            public string? SocialMediaId { get; set; }

            [DataType(DataType.PhoneNumber)]
            public string? PhoneNumber { get; set; }
            public int? SpecialityId { get; set; }
            [ValidateNever]
            public IEnumerable<SelectListItem> SpecialityList { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            //Add Roles
            if (!_roleManager.RoleExistsAsync(SD.Role_Student).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Student)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
            }

            Input = new InputModel()
            {
                RoleList = _roleManager.Roles.Select(x => x.Name).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }),
                CountryList=_unit.Country.GetAll().Select(u=> new SelectListItem
                {
                    Value=u.Id.ToString(),
                    Text=u.CountryName
                }),
                GenderList= new List<SelectListItem>()
                {
                    new SelectListItem{Value="M",Text="Male" },
                    new SelectListItem{Value="F",Text="Female"}
                },
                SpecialityList= _unit.Speciality.GetAll().Select(u=> new SelectListItem
                {
                    Value=u.Id.ToString(),
                    Text=u.SpecialityName
                }),
                MotherTongueList= new List<SelectListItem>()
                {
                    new SelectListItem{Value="fr",Text="French"},
                    new SelectListItem{Value="ar",Text="Arabic"},
                    new SelectListItem{Value="zh",Text="Chinese"},
                    new SelectListItem{Value="en",Text="English"},
                    new SelectListItem{Value="ru",Text="Russian"},
                    new SelectListItem{Value="es",Text="Spanish"},
                    new SelectListItem{Value="tk",Text="Turkmen"},
                    new SelectListItem{Value="tr",Text="Turkish"},
                    new SelectListItem{Value="uk",Text="Ukrainian"},
                    new SelectListItem{Value="uz",Text="Uzbek"},
                    new SelectListItem{Value="hi",Text="Hindi"},
                    new SelectListItem{Value="pl",Text="Polish"},
                    new SelectListItem{Value="de",Text="German"}
                }
            };
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null ,IFormFile ? file=null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                //Populate User(applicationUser)
                user.Name = Input.Name;
                user.Surname=Input.Surname;
                user.Birthday = Input.Birthday;
                user.PhoneNumber = Input.PhoneNumber;
                user.SocialMediaId = Input.SocialMediaId;
                user.SpecialityId = Input.SpecialityId;
                if (Input.SpecialityId != null) { user.FacultyId = _unit.Speciality.Get(u => u.Id == Input.SpecialityId).FacultyId; }                                            
                user.CountryId= Input.CountryId;               
                user.MotherTongue=Input.MotherTongue;
                user.Gender=Input.Gender;
                //Create The Image File
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    //Guid.NewGuid().ToString() is used to randomly name files while saving them in the folder
                    //Path.GetExtension(file.FileName) is to get the same extension that the uploaded image
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string profilePath = Path.Combine(wwwRootPath, @"images\UserProfilePictures\");
                    if (!String.IsNullOrEmpty(Input.ProfilePictureUrl))
                    {
                        //Delete Old Image
                        var oldPath = Path.Combine(wwwRootPath, Input.ProfilePictureUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }
                    // Copy file to the web application repository
                    using (var filestream = new FileStream(Path.Combine(profilePath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    user.ProfilePictureUrl = @"\images\UserProfilePictures\" + filename;
                }


            var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    // Verification of the Input.role Value
                    if (!String.IsNullOrEmpty(Input.Role))
                    {
                        //Add Role to User
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }
                    else
                    {
                        //Defaul Role
                        await _userManager.AddToRoleAsync(user, SD.Role_Student);
                    }
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
