using AutoMapper;
using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using LifeQuest.DAL.Models;
using LifeQuest.DAL.UOW.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeQuest.BLL.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            // جلب كل الاقسام 
            var categories = await _unitOfWork.Repository<Category>().GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            // جلب قسم معين بال ID
            var category = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> CreateCategoryAsync(CategoryDto dto)
        {
            // اضافه قسم جديد
            var category = _mapper.Map<Category>(dto);
            await _unitOfWork.Repository<Category>().AddAsync(category);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateCategoryAsync(CategoryDto dto)
        {
            // تحديث بيانات القسم
            var category = _mapper.Map<Category>(dto);
            _unitOfWork.Repository<Category>().Update(category);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            // حذف القسم
            await _unitOfWork.Repository<Category>().Delete(id);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
