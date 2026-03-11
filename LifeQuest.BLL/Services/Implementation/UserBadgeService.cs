using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuest.BLL.Services.Implementation
{
    internal class UserBadgeService : IUserBadgeService
    {
        public Task CheckAndAwardBadgesAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserBadgeDTO>> GetUserBadgesAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
