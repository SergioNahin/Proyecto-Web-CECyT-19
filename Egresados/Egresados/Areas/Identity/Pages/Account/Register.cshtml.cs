// Licensed to the .NET Foundation under one or more agreements.
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
using Egresados.Models;
using Egresados.Utilidades;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Egresados.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ILogger<RegisterModel> _logger;
        //private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager
            /*IEmailSender emailSender*/)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            //_emailSender = emailSender;
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
            [StringLength(100, ErrorMessage = "Ingrese su boleta como contraseña", MinimumLength = 10)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirma Contraseña")]
            [Compare("Password", ErrorMessage = "La contraseña y confirmación no son iguales.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "El Nombre es Obligatorio")]
            [MinLength(8, ErrorMessage = "Ingrese el Nombre Completo")]
            [Display(Name = "Nombre")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "La Boleta es Obligatoria")]
            [MinLength(10, ErrorMessage = "Ingrese su Boleta Completa")]
            [Display(Name = "Boleta")]
            public string Boleta { get; set; }

            [Required(ErrorMessage = "El Grupo es Obligatorio")]
            [MinLength(5, ErrorMessage = "Faltan Datos")]
            [Display(Name = "Grupo")]
            public string Grupo { get; set; }

            [Required(ErrorMessage = "La Generación es Obligatoria")]
            [Display(Name = "Generación")]
            [MinLength(9, ErrorMessage = "Ingrese Formato Completo")]
            public string Generacion { get; set; }

            [Required(ErrorMessage = "El CURP es Obligatorio")]
            [RegularExpression(@"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$", ErrorMessage = "El formato del CURP no es válido.")]
            [Display(Name = "CURP")]
            public string CURP { get; set; }

            [Required(ErrorMessage = "La Fecha de Nacimiento es Obligatoria")]
            [Display(Name = "Fecha de Nacimiento")]
            public string FechaNacimiento { get; set; }

            [Required(ErrorMessage = "La Edad es Obligatoria")]
            [Range(17, 35, ErrorMessage = "La edad debe estar en el rango de 17 a 35 años.")]
            [Display(Name = "Edad")]
            public string Edad { get; set; }

            [Required(ErrorMessage = "El Sexo es Obligatorio")]
            [Display(Name = "Sexo")]
            public string Sexo { get; set; }

            //Email institucional
            [Required(ErrorMessage = "El Correo Institucional es Obligatorio")]
            [Display(Name = "Correo Institucional")]
            public string Correo_Institucional { get; set; }

            //Campos domicilio
            [Required(ErrorMessage = "El Domicilio es Obligatorio")]
            [Display(Name = "Domicilio Completo")]
            [MinLength(20, ErrorMessage = "Ingrese Domicilio Correcto")]
            public string Domicilio { get; set; }

            [Required(ErrorMessage = "El celular es Obligatorio")]
            [Display(Name = "Celular")]
            [MinLength(10, ErrorMessage = "Ingrese Celular Completo")]
            public string Celular { get; set; }

            [Display(Name = "Telefono")]
            public string Telefono { get; set; }

            [Display(Name = "Folio")]
            public string Folio { get; set; }

            //Datos Escolares
            [Required(ErrorMessage = "La Carrera es Obligatoria")]
            [Display(Name = "Carrera Egreso")]
            [MinLength(8, ErrorMessage = "Datos Incompletos")]
            public string Carrera { get; set; }

            [Display(Name = "Superior")]
            public string Escuela { get; set; }
            public string Escuela2 { get; set; }
            public string Escuela3 { get; set; }

            [Display(Name = "Carrera")]
            public string CarreraSuperior { get; set; }
            public string CarreraSuperior2 { get; set; }
            public string CarreraSuperior3 { get; set; }

            [Display(Name = "Escuela Ajena")]
            public string EscuelaExterna { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();


                //Campos personalizados guardados
                user.Nombre = Input.Nombre;
                user.Boleta = Input.Boleta;
                user.Grupo = Input.Grupo;
                user.Generacion = Input.Generacion;
                user.CURP = Input.CURP;
                user.FechaNacimiento = Input.FechaNacimiento;
                user.Sexo = Input.Sexo;
                user.Domicilio = Input.Domicilio;
                user.Celular = Input.Celular;
                user.Telefono = Input.Telefono;
                user.Carrera = Input.Carrera;
                user.Folio = Input.Folio;
                user.Escuela = Input.Escuela;
                user.Escuela2 = Input.Escuela2;
                user.Escuela3 = Input.Escuela3;
                user.EscuelaExterna = Input.EscuelaExterna;
                user.CarreraSuperior = Input.CarreraSuperior;
                user.CarreraSuperior2 = Input.CarreraSuperior2;
                user.CarreraSuperior3 = Input.CarreraSuperior3;
                user.Correo_Institucional = Input.Correo_Institucional;
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                 //Aquí validamos si los roles existen sino se crean
                 if (!await _roleManager.RoleExistsAsync(CNT.Admin))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(CNT.Admin));
                            await _roleManager.CreateAsync(new IdentityRole(CNT.Usuario));
                        }

                        //Obtenemos el rol seleccionado
                        string rol = Request.Form["radUsuarioRole"].ToString();

                        //Validamos si el rol seleccionado es Admin y si lo es lo agregamos
                        if (rol == CNT.Admin)
                        {
                            await _userManager.AddToRoleAsync(user, CNT.Admin);
                        }
                        else
                        {
                            if (rol == CNT.Usuario)
                            {
                                await _userManager.AddToRoleAsync(user, CNT.Usuario);
                            }
                            else
                            {
                                await _userManager.AddToRoleAsync(user, CNT.Usuario);
                            }
                        }
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
                    throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                        $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                        $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
                }
            }

            private IUserEmailStore<ApplicationUser> GetEmailStore()
            {
                if (!_userManager.SupportsUserEmail)
                {
                    throw new NotSupportedException("The default UI requires a user store with email support.");
                }
                return (IUserEmailStore<ApplicationUser>)_userStore;
            }
        }
    }
