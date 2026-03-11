using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuest.BLL.Services.Implementation
{
    internal class BadgeService : IBadgeService
    {
        public Task<IEnumerable<BadgeDTO>> GetAllBadgesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BadgeDTO> GetBadgeByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
