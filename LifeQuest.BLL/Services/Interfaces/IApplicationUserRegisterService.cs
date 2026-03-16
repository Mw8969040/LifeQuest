using LifeQuest.DAL.Models;
using LIfeQuest.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuest.BLL.Services.Interfaces
{
    public interface IApplicationUserRegisterService
    {
        Task<bool> IsUnique(string UserName);
        Task<ApplicationUser> GetByNameAsync(string UserName);
        Task<(ApplicationUser user, List<string> error)> CreateAsync(RegisterationDTO applicationUser  , string PlainPassword);
        Task<List<string>> Countries();


    }
}
