using LifeQuest.BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeQuest.BLL.Services.Interfaces
{
    public interface ILevelService
    {
        Task<IEnumerable<LevelDto>> GetAllLevelsAsync();
        Task<LevelDto?> GetLevelByIdAsync(int id);
        Task<bool> AddLevelAsync(LevelDto dto);
        Task<bool> UpdateLevelAsync(LevelDto dto);
        Task<bool> DeleteLevelAsync(int id);
    }
}
