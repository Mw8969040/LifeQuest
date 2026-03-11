

using LifeQuest.BLL.DTOs;

namespace LifeQuest.BLL.Services.Interfaces
{
    public interface IBadgeService
    {
         public Task<IEnumerable<BadgeDTO>> GetAllBadgesAsync();

         public  Task<BadgeDTO> GetBadgeByIdAsync(int id);
    }
}

