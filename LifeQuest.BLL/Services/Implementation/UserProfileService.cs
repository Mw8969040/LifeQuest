using AutoMapper;
using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using LifeQuest.DAL.Models;
using LifeQuest.DAL.UOW.Interface;

namespace LifeQuest.BLL.Services.Implementation
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserProfileService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(int userId)
        {
            // هجيب بيانات الملف الشخصي بتاع اليوزر بكل الحاجات اللي معاه (badges, challenges, level)
            var userProfile = await _unitOfWork.Repository<UserProfile>()
                .GetByIdWithIncludeAsync(up => up.UserId == userId, "User", "Level", "User.UserBadges", "User.UserChallenges");

            if (userProfile == null) return null;

            var dto = _mapper.Map<UserProfileDTO>(userProfile);

            // هحسب عدد البادجات والتحديات اللي لسه شغالة يدوي عشان نطلع بيانات دقيقة
            dto.TotalBadges = userProfile.User?.UserBadges.Count ?? 0;
            dto.ActiveChallenges = userProfile.User?.UserChallenges.Count(uc => uc.Status == "InProgress") ?? 0;

            // هحسب النقاط اللي ناقصة عشان يطلع للمستوى اللي بعده
            var nextLevel = (await _unitOfWork.Repository<Level>().GetAllAsync())
                .Where(l => l.Point > userProfile.TotalPoints)
                .OrderBy(l => l.Point)
                .FirstOrDefault();

            if (nextLevel != null)
            {
                dto.RemainingPointsForNextLevel = nextLevel.Point - userProfile.TotalPoints;
            }
            else
            {
                dto.RemainingPointsForNextLevel = 0; // وصل لأعلى مستوى خلاص
            }

            return dto;
        }

        public async Task UpdateUserProfileAsync(UserProfileDTO dto)
        {
            // هعدل بيانات الملف الشخصي (زي الـ Bio)
            var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(dto.UserId);
            if (userProfile != null)
            {
                userProfile.Bio = dto.Bio;
                _unitOfWork.Repository<UserProfile>().Update(userProfile);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task AddPointsToUserAsync(int userId, int points)
        {
            // هزود نقاط لليوزر وهشوف لو يستاهل يترقى لمستوى أعلى
            var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(userId);
            if (userProfile != null)
            {
                userProfile.TotalPoints += points;
                _unitOfWork.Repository<UserProfile>().Update(userProfile);
                
                // هعمل تشيك على الليفل بعد ما زودنا النقاط
                await CheckLevelUpAsync(userId);
                
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task UpdateSuccessRateAsync(int userId)
        {
            // هحسب نسبة النجاح بتاعته بناءً على التحديات اللي خلصها صح
            var userChallenges = await _unitOfWork.Repository<UserChallenge>()
                .GetAllWithIncludesAsync(uc => uc.UserId == userId);

            int totalChallenges = userChallenges.Count();
            if (totalChallenges > 0)
            {
                int successfulChallenges = userChallenges.Count(uc => uc.IsSuccess);
                var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(userId);
                if (userProfile != null)
                {
                    userProfile.SuccessRate = (int)((double)successfulChallenges / totalChallenges * 100);
                    _unitOfWork.Repository<UserProfile>().Update(userProfile);
                    await _unitOfWork.CompleteAsync();
                }
            }
        }

        public async Task CheckLevelUpAsync(int userId)
        {
            // هشوف إجمالي نقاط اليوزر وهرقيه للمستوى المناسب لنقاطه
            var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdWithIncludeAsync(up => up.UserId == userId, "Level");
            if (userProfile != null)
            {
                var levels = await _unitOfWork.Repository<Level>().GetAllAsync();
                var nextLevel = levels
                    .Where(l => l.Point <= userProfile.TotalPoints)
                    .OrderByDescending(l => l.Point)
                    .FirstOrDefault();

                if (nextLevel != null && nextLevel.Id != userProfile.LevelId)
                {
                    userProfile.LevelId = nextLevel.Id;
                    _unitOfWork.Repository<UserProfile>().Update(userProfile);
                    // هنا مش هنعمل CompleteAsync عشان لو الميثود دي اتنادت من جوا AddPoints يبقى السيف يحصل مرة واحدة
                }
            }
        }
    }
}
