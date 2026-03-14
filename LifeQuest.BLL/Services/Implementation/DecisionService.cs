using AutoMapper;
using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using LifeQuest.DAL.Models;
using LifeQuest.DAL.UOW.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LifeQuest.BLL.Services.Implementation
{
    public class DecisionService : IDecisionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMetricsCalcService _metricsService;

        public DecisionService(IUnitOfWork unitOfWork, IMapper mapper, IMetricsCalcService metricsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _metricsService = metricsService;
        }

        public async Task<bool> AddDecisionAsync(DecisionDto dto)
        {
            // التحقق من وجود المستخدم
            var user = await _unitOfWork.Repository<ApplicationUser>().GetByIdAsync(dto.UserId);
            if (user == null) throw new Exception("User not found!");

            // إنشاء قرار جديد
            var decision = _mapper.Map<Decision>(dto);
            decision.CreateAt = DateTime.Now;
            decision.UpdateAt = DateTime.Now;

            await _unitOfWork.Repository<Decision>().AddAsync(decision);
            await _unitOfWork.CompleteAsync(); // حفظ للقرار للحصول على ال ID

            // حساب المؤشرات وحفظها
            var metrics = await _metricsService.CalculateUserMetricsAsync(dto.UserId, decision.Id);
            await _unitOfWork.Repository<MetricsCalc>().AddAsync(metrics);
            
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateDecisionResultAsync(int decisionId, bool isSuccess)
        {
            // جلب القرار
            var decision = await _unitOfWork.Repository<Decision>().GetByIdAsync(decisionId);
            if (decision == null) throw new Exception("Decision not found!");

            // تحديث النتيجة
            decision.IsSuccess = isSuccess;
            decision.UpdateAt = DateTime.Now;
            _unitOfWork.Repository<Decision>().Update(decision);
            await _unitOfWork.CompleteAsync();

            // إعادة حساب المؤشرات وحفظها مع هذا القرار
            var metrics = await _metricsService.CalculateUserMetricsAsync(decision.UserId, decision.Id);
            
            // نتحقق إذا كان فيه Metrics قديمة للقرار ده نحدثها، وإلا نكريت جديدة
            var existingMetrics = await _unitOfWork.Repository<MetricsCalc>().GetByIdWithIncludeAsync(m => m.DecisionId == decisionId);
            if (existingMetrics != null)
            {
                existingMetrics.SuccessRate = metrics.SuccessRate;
                existingMetrics.ConfidenceAccuracy = metrics.ConfidenceAccuracy;
                existingMetrics.OverConfidenceIndex = metrics.OverConfidenceIndex;
                existingMetrics.RiskPattern = metrics.RiskPattern;
                existingMetrics.UpdateAt = DateTime.Now;
                _unitOfWork.Repository<MetricsCalc>().Update(existingMetrics);
            }
            else
            {
                await _unitOfWork.Repository<MetricsCalc>().AddAsync(metrics);
            }

            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<IEnumerable<DecisionDto>> GetUserDecisionsAsync(int userId)
        {
            // جلب كل قرارات المستخدم
            var decisions = await _unitOfWork.Repository<Decision>()
                .GetAllWithIncludesAsync(d => d.UserId == userId, "Metrics");
            
            return _mapper.Map<IEnumerable<DecisionDto>>(decisions);
        }

        public async Task<DecisionDto?> GetDecisionDetailsAsync(int decisionId)
        {
            // جلب تفاصيل القرار مع المؤشرات
            var decision = await _unitOfWork.Repository<Decision>()
                .GetByIdWithIncludeAsync(d => d.Id == decisionId, "Metrics");
            
            return _mapper.Map<DecisionDto>(decision);
        }

        public async Task<bool> DeleteDecisionAsync(int decisionId)
        {
            // حذف القرار والمؤشرات المرتبطة به (لو قاعدة البيانات مش بتهندل ال cascade)
            var metrics = await _unitOfWork.Repository<MetricsCalc>().GetByIdWithIncludeAsync(m => m.DecisionId == decisionId);
            if (metrics != null)
            {
                await _unitOfWork.Repository<MetricsCalc>().Delete(metrics.Id);
            }

            await _unitOfWork.Repository<Decision>().Delete(decisionId);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
