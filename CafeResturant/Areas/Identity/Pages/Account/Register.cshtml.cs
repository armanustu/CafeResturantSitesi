﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CafeResturant.Data;
using CafeResturant.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace CafeResturant.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
       
        //Aşağıdaki iki satırı rolleme için biz ekliyoruz
          private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;


        //Altaki iki satır property eklendi sonra injection yapıyoruz
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext db;
        //Buraya injection yaptık sonra aşağıya succeed içine admin oluşturalım
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext _db
           

            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            db = _db;
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
            [EmailAddress]  //Buraya Email yazılırsa input kutucuğuna email formatte girilmesini ister
            [Display(Name = "Email")]
            public string Email { get; set; }

			[Required]			
			[Display(Name = "İsim")]
			public string Name { get; set; }//isim soyisim telefon rol klasslarını ekledik

			[Required]
			
			[Display(Name = "Soyisim")]
			public string Surname { get; set; }
			[Required]
			
			[Display(Name = "Telefon")]
			public string Telefon { get; set; }

			[Required]
			
			[Display(Name = "Rol")]
			public string Role { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]//Buraya şifreyi gizleyemeyi sağlar yukardaki şifrelem özelliği katar
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
            public string Sehir { get; set; }
            public IEnumerable<SelectListItem> Roller { get; set; }
        }

        //Burası  admin olarak giriş yapan kullanıcının veri tabanından tüm rol kayıtlarını komboboxa getir demek 
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Input = new InputModel()
            {
                Roller = _roleManager.Roles.Where(i => i.Name != diger.Role_birey).Select(x => x.Name).Select(u => new SelectListItem
                {
                    Text = u,
                    Value = u.ToString()

                })

            };
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
              //var user = CreateUser();

                //await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                //await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                //ApplicatiobnUser Clas değerleri eklendi 
                var user = new ApplicationUser()
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Surname = Input.Surname,
                    Name=Input.Name,
                    Role = Input.Role,
                    Sehir=Input.Sehir
                     
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

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
                    
                    //Buraya admin ekliyoruz sonra usermanager ile user adminrolü veriyoruz veri tabanına admin rolü kayıt ediliyor

                    if(!await _roleManager.RoleExistsAsync(diger.Role_admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(diger.Role_admin));
                    }
                  

                if (!await _roleManager.RoleExistsAsync(diger.Role_insankaynakları))
                {
                    await _roleManager.CreateAsync(new IdentityRole(diger.Role_insankaynakları));
                }
               

                if (!await _roleManager.RoleExistsAsync(diger.Role_sekreter))
                {
                    await _roleManager.CreateAsync(new IdentityRole(diger.Role_sekreter));
                }
              

                if (!await _roleManager.RoleExistsAsync(diger.Role_uzman))
                {
                    await _roleManager.CreateAsync(new IdentityRole(diger.Role_uzman));
                }
               

                if (!await _roleManager.RoleExistsAsync(diger.Role_birey))
                {
                    await _roleManager.CreateAsync(new IdentityRole(diger.Role_birey));
                }

               // await _userManager.AddToRoleAsync(user, diger.Role_admin);Bu kodu proje çalıştırırken bir seferliğine admin rolü kullanıcı oluşturmak için yazıyoruz  bu kodu yoruma çektiğimiz için bunda sonra kayıt eklerken kullanıcı olarak ekliyecek

                if (user.Role == null)
                {
                    await _userManager.AddToRoleAsync(user, diger.Role_birey);//Veri tabanına user rolü olarak kayıtlar eklendi
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, user.Role);
                }




                





                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                      if (user.Role == null)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "User", new { Area = "Admin" });

                    }

                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
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
