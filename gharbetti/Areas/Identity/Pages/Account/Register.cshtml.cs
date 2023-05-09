// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Gharbetti.Data;
using Gharbetti.Models;
using Gharbetti.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gharbetti.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly ApplicationDbContext _db;


        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment environment,
            ApplicationDbContext db
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _environment = environment;
            _db = db;
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

            [Required]
            public string FirstName { get; set; }

            public string? MiddleName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required]
            public string MobileNumber { get; set; }

            public string PhoneNumber { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime Dob { get; set; }

            public string? Identification { get; set; }

            public string? PhotoId { get; set; }

            public byte Status { get; set; }

            [Required]
            public string AddressLine1 { get; set; }

            public string? AddressLine2 { get; set; }

            public string? AddressLine3 { get; set; }

            [Required]
            public string City { get; set; }

            [Required]
            public string PostalCode { get; set; }

            public string? County { get; set; }

            [Required]
            public string Country { get; set; }


            [BindProperty]
            [Required(ErrorMessage = "Please select a file.")]
            [DataType(DataType.Upload)]
            public IFormFile IdentificationFile { get; set; }


            [BindProperty]
            [Required(ErrorMessage = "Please select a file.")]
            [DataType(DataType.Upload)]
            [MaxFileSize(5 * 1024 * 1024)]
            [AllowedExtensions(new string[] { ".jpg", ".png" })]
            public IFormFile PhotoFile { get; set; }

            [ValidateNever]
            public IEnumerable<SelectListItem> CountryList { get; set; }
            
            
            [ValidateNever]
            public IEnumerable<SelectListItem> RoomList { get; set; }

            public int? HouseRoomId { get; set; }
            public string? StayLength { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!await _roleManager.RoleExistsAsync(StaticDetail.Role_Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(StaticDetail.Role_Admin));
                await _roleManager.CreateAsync(new IdentityRole(StaticDetail.Role_PendingTenant));
                await _roleManager.CreateAsync(new IdentityRole(StaticDetail.Role_Tenant));
            }
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                       
            var roomList = await (from r in _db.Rooms
                            join hr in _db.HouseRooms on r.Id equals hr.RoomId
                            join h in _db.Houses on hr.HouseId equals h.Id
                            select new HouseRoomViewModel
                            {
                                Id = hr.Id,
                                HouseId = h.Id,
                                RoomId = r.Id,
                                HouseName = h.Name,
                                RoomName = r.RoomNo
                            }).ToListAsync();
            
            
            Input = new InputModel()
            {
                CountryList = GetCountryList().Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                }),
                RoomList = roomList.Select(x => new SelectListItem
                {
                    Text = x.HouseName + "-> " + x.RoomName,
                    Value = x.Id.ToString()
                })
            };
           
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var dateTimeTick = DateTime.Now.Ticks.ToString();

                var tickIdentification = $"{dateTimeTick}.{Input.IdentificationFile.FileName.Split(".")[1]}";
                var file1 = Path.Combine(_environment.ContentRootPath, @"wwwroot\uploads", $"{tickIdentification}");
                using (var fileStream = new FileStream(file1, FileMode.Create))
                {
                    await Input.IdentificationFile.CopyToAsync(fileStream);
                }

                dateTimeTick = DateTime.Now.Ticks.ToString();
                var tickPhotoFile = $"{dateTimeTick}.{Input.PhotoFile.FileName.Split(".")[1]}";
                var file2 = Path.Combine(_environment.ContentRootPath, @"wwwroot\uploads", $"{tickPhotoFile}");
                using (var fileStream = new FileStream(file2, FileMode.Create))
                {
                    await Input.PhotoFile.CopyToAsync(fileStream);
                }


                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.FirstName = Input.FirstName;
                user.MiddleName = Input.MiddleName;
                user.LastName = Input.LastName;
                user.Dob = Input.Dob;
                user.PhoneNumber = Input.PhoneNumber;
                user.AddressLine1 = Input.AddressLine1;
                user.AddressLine2 = Input.AddressLine2;
                user.AddressLine3 = Input.AddressLine3;
                user.City = Input.City;
                user.PostalCode = Input.PostalCode;
                user.Country = Input.Country;
                user.County = Input.County;
                user.MobileNumber = Input.MobileNumber;
                user.Identification = tickIdentification;
                user.PhotoId = tickPhotoFile;
                user.HouseRoomId = Input.HouseRoomId;
                user.StayLength = Input.StayLength;

                var result = await _userManager.CreateAsync(user, Input.Password);



                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, StaticDetail.Role_PendingTenant);

                    return RedirectToAction("Index", "Home");

                    //var userId = await _userManager.GetUserIdAsync(user);
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    //}
                    //else
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //    return LocalRedirect(returnUrl);
                    //}
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

        public static List<string> GetCountryList()
        {
            List<string> cultureList = new List<string>();

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo culture in cultures)
            {
                RegionInfo region = new RegionInfo(culture.LCID);

                if (!(cultureList.Contains(region.EnglishName)))
                {
                    cultureList.Add(region.EnglishName);
                }
            }
            return cultureList;
        }

        public class MaxFileSizeAttribute : ValidationAttribute
        {
            private readonly int _maxFileSize;
            public MaxFileSizeAttribute(int maxFileSize)
            {
                _maxFileSize = maxFileSize;
            }

            protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
            {
                var file = value as IFormFile;
                if (file != null)
                {
                    if (file.Length > _maxFileSize)
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }

                return ValidationResult.Success;
            }

            public string GetErrorMessage()
            {
                return $"Maximum allowed file size is {_maxFileSize} bytes.";
            }
        }

        public class AllowedExtensionsAttribute : ValidationAttribute
        {
            private readonly string[] _extensions;
            public AllowedExtensionsAttribute(string[] extensions)
            {
                _extensions = extensions;
            }

            protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
            {
                var file = value as IFormFile;
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }

                return ValidationResult.Success;
            }

            public string GetErrorMessage()
            {
                return $"This photo extension is not allowed!";
            }
        }
    }
}
