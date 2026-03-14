using LifeQuest.BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeQuest.BLL.Services.Interfaces
{
    public interface IChallengeService
    {
        public Task CreateChallengeAsync(ChallengeDto dto);
        public Task<List<ChallengeDto>> GetAllChallengesAsync();
        public Task<ChallengeDto?> GetChallengeByIdAsync(int id);
        public Task<bool> UpdateChallengeAsync(ChallengeDto dto);
        public Task<bool> DeleteChallengeAsync(int id);
        public Task<IEnumerable<ChallengeDto>> GetChallengesByCategoryAsync(int categoryId);
    }
}
