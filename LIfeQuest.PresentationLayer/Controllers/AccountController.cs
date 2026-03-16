using LifeQuest;
using LifeQuest.BLL.Services.Interfaces;
using LifeQuest.DAL.Models;
using LIfeQuest.BLL.DTOs;
using LIfeQuest.PL.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;
using System.Text;

namespace LIfeQuest.PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApplicationUserRegisterService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private async Task<ICollection<string>> CountriesAsync()
        { return await _applicationUserService.Countries(); }
        public AccountController(IApplicationUserRegisterService applicationUserService,
            UserManager<ApplicationUser> userManager
            , IEmailSenderService emailSenderService, SignInManager<ApplicationUser> signInManager)
        {
            _applicationUserService = applicationUserService;
            _userManager = userManager;
            _emailSenderService = emailSenderService;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            RegisterationVM registerVM = new RegisterationVM();
            registerVM.Countries = await CountriesAsync();

            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterationVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            if (registerVM.PlainPassword != registerVM.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                registerVM.Countries = await CountriesAsync();
                return View(registerVM);
            }

            //Tuple Deconstruction to get the new user and any errors from the service
            var (NewUser, errors) = await _applicationUserService.CreateAsync(registerVM.registerationDTO, registerVM.PlainPassword);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error);
                }

                registerVM.Countries = await CountriesAsync();
                return View("Register", registerVM);
            }

            if (NewUser == null)
            {
                ModelState.AddModelError("", "An error occurred while creating the user.");

                registerVM.Countries = await CountriesAsync();
                return View("Register", registerVM);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(NewUser);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var link = Url.Action("ConfirmEmail", "Account",
                new { userId = NewUser.Id, token = token }, Request.Scheme);

            // إرسال الإيميل
            await _emailSenderService.SendEmailAsync(
                NewUser.Email,
                "Confirm your email",//subject
                $"Click <a href='{link}'>here</a> to confirm your email" //ممكن نضيفها في ال appsetting او هنا  هي هنا ك hardcode 
            );


            return RedirectToAction("ConfirmEmail");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            // Decode Token
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                return View("EmailConfirmed");
            }

            return BadRequest("Error confirming email");
        }

        public IActionResult EmailConfirmed()
        {
            //MSG Confirmation View
            //button to login if the user has already confirmed their email
            //buttun to resend confirmation email if the user didn't receive it
            return View();
        }

        public async Task<IActionResult> ResendEmailConfirmation(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is Required");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found");

            if (user.EmailConfirmed)
                return BadRequest("Email is already confirmed");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("ConfirmEmail", "Account",
                new { userId = user.Id, token = token }, Request.Scheme);

            await _emailSenderService.SendEmailAsync(
                user.Email,
                "Confirm your email",
                "Click <a href='" + link + "'>here</a> to confirm your email"
                );

            return Ok("Confirmation email sent. Please check your inbox.");
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (loginVM == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(loginVM);
            }

            if (string.IsNullOrWhiteSpace(loginVM.Email))
                ModelState.AddModelError("", "Email is required.");

            if (string.IsNullOrWhiteSpace(loginVM.PlainPassword))
                ModelState.AddModelError("", "Password is required.");


            var loginUser = await _userManager.FindByEmailAsync(loginVM.Email);
            if (loginUser == null)
            {
                ModelState.AddModelError("", "Invalid Email.");
                return View(loginVM);
            }

            if (!loginUser.EmailConfirmed)
            {
                ModelState.AddModelError("", "Please confirm your email before logging in.");
                return View(loginVM);
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(loginUser, loginVM.PlainPassword);
            if (!passwordCheck)
            {
                ModelState.AddModelError("", "Invalid Password.");
                return View(loginVM);
            }



            return View();
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        public async Task<IActionResult> ForgotPassword(ForgotPasswordViM forgotPasswordViM)
        {
            if (!ModelState.IsValid)
                return View(forgotPasswordViM);

            var user = await _userManager.FindByEmailAsync(forgotPasswordViM.Email);

            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var link = Url.Action("ResetPassword", "Account",
                new { email = forgotPasswordViM.Email, token = token }, Request.Scheme);

            await _emailSenderService.SendEmailAsync(
                forgotPasswordViM.Email,
                "Reset your password",
                $"Click <a href='{link}'>here</a> to reset your password"
            );
            return View("ForgotPasswordConfirmation");
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var model = new ResetPasswordVM { Email = email, Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordVM);
            var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.PlainPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(resetPasswordVM);
        }
    }
    }

