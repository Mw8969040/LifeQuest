using Azure.Core;
using LifeQuest.BLL.Services.Interfaces;
using LifeQuest.DAL.Models;
using LIfeQuest.BLL.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Bcpg.Sig;
using Org.BouncyCastle.Utilities.Encoders;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;


namespace LifeQuest         
{
    public class ApplicationUseRegisterService :IApplicationUserRegisterService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSenderService _emailSender;
        public ApplicationUseRegisterService(UserManager<ApplicationUser> userManager,IEmailSenderService emailSender)
        {
            _userManager = userManager;
        }
        public async Task<bool> IsUnique(string UserName)
        {
            var user = GetByNameAsync(UserName);
            if (user != null)  return false;

            return true;
        }

        public async Task<ApplicationUser> GetByNameAsync(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            return user;
        }

                            /*tuple*/
        public async Task<(ApplicationUser user ,List<string> error)> CreateAsync(RegisterationDTO AppUser, string PlainPassword)
        {
            var user = new ApplicationUser { UserName = AppUser.Email, Email = AppUser.Email };

            var ExistingUser = await GetByNameAsync(AppUser.UserName);

            if (ExistingUser != null)
            {
                return (null, new List<string> { "Username already exists" });
            }

            var NewUser = new ApplicationUser
            {
                Name = AppUser.Name,
                UserName = AppUser.UserName,
                Email = AppUser.Email,
                Country = AppUser.Country
            };

            var result = await _userManager.CreateAsync(NewUser, PlainPassword);


            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e=>e.Description).ToList();
                return (null, errors);
            }

            if (NewUser == null)
            {
                return (null, new List<string>() { "An error occurred while creating the user." });
            }

            return (NewUser, new List<string>());
          
        }


        public async Task<List<string>> Countries()
        {
            var countries = CultureInfo
                .GetCultures(CultureTypes.SpecificCultures)
                .Select(c => new RegionInfo(c.Name).EnglishName)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
            return countries;
        }

    }
}
