using AutoMapper;
using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using LifeQuest.DAL.Models;
using LifeQuest.DAL.UOW.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LifeQuest.BLL.Services.Implementation
{
    public class ChallengeService : IChallengeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChallengeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateChallengeAsync(ChallengeDto dto)
        {
            // انشاء تحدى جديد
            var challenge = _mapper.Map<Challenge>(dto);
            
            // تعيين بعض القيم الافتراضية او المحسوبة إذا لم تكن موجودة في DTO
            challenge.CategoryId = dto.CategoryId == 0 ? 1 : dto.CategoryId; // Default to 1 for now if not set
            challenge.Duration = (dto.EndDate - dto.StartDate).Days;
            
            await _unitOfWork.Repository<Challenge>().AddAsync(challenge);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<ChallengeDto>> GetAllChallengesAsync()
        {
            // جلب كل التحديات مع بيانات القسم
            var challenges = await _unitOfWork.Repository<Challenge>().GetAllWithIncludesAsync(c => true, "Category");
            return _mapper.Map<List<ChallengeDto>>(challenges.ToList());
        }

        public async Task<ChallengeDto?> GetChallengeByIdAsync(int id)
        {
            // جلب تحدى معين بال ID مع بيانات القسم
            var challenge = await _unitOfWork.Repository<Challenge>().GetByIdWithIncludeAsync(c => c.Id == id, "Category");
            return _mapper.Map<ChallengeDto>(challenge);
        }

        public async Task<bool> UpdateChallengeAsync(ChallengeDto dto)
        {
            // تحديث بيانات التحدى
            var challenge = _mapper.Map<Challenge>(dto);
            challenge.Duration = (dto.EndDate - dto.StartDate).Days;
            _unitOfWork.Repository<Challenge>().Update(challenge);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> DeleteChallengeAsync(int id)
        {
            // حذف التحدى
            await _unitOfWork.Repository<Challenge>().Delete(id);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<IEnumerable<ChallengeDto>> GetChallengesByCategoryAsync(int categoryId)
        {
            // جلب التحديات التابعه لقسم معين
            var challenges = await _unitOfWork.Repository<Challenge>()
                .GetAllWithIncludesAsync(c => c.CategoryId == categoryId, "Category");
            
            return _mapper.Map<IEnumerable<ChallengeDto>>(challenges);
        }
    }
}
