using AutoMapper;
using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using LifeQuest.DAL.Models;
using LifeQuest.DAL.UOW.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeQuest.BLL.Services.Implementation
{
    public class LevelService : ILevelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LevelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LevelDto>> GetAllLevelsAsync()
        {
            // جلب كل المستويات
            var levels = await _unitOfWork.Repository<Level>().GetAllAsync();
            return _mapper.Map<IEnumerable<LevelDto>>(levels);
        }

        public async Task<LevelDto?> GetLevelByIdAsync(int id)
        {
            // جلب مستوى معين بال ID
            var level = await _unitOfWork.Repository<Level>().GetByIdAsync(id);
            return _mapper.Map<LevelDto>(level);
        }

        public async Task<bool> AddLevelAsync(LevelDto dto)
        {
            // اضافه مستوى جديد
            var level = _mapper.Map<Level>(dto);
            await _unitOfWork.Repository<Level>().AddAsync(level);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateLevelAsync(LevelDto dto)
        {
            // تحديث بيانات المستوى
            var level = _mapper.Map<Level>(dto);
            _unitOfWork.Repository<Level>().Update(level);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> DeleteLevelAsync(int id)
        {
            // حذف المستوى
            await _unitOfWork.Repository<Level>().Delete(id);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
