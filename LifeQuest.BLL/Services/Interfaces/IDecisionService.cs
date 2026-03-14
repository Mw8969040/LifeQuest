using LifeQuest.BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeQuest.BLL.Services.Interfaces
{
    public interface IDecisionService
    {
        Task<bool> AddDecisionAsync(DecisionDto dto);
        Task<bool> UpdateDecisionResultAsync(int decisionId, bool isSuccess);
        Task<IEnumerable<DecisionDto>> GetUserDecisionsAsync(int userId);
        Task<DecisionDto?> GetDecisionDetailsAsync(int decisionId);
        Task<bool> DeleteDecisionAsync(int decisionId);
    }
}
