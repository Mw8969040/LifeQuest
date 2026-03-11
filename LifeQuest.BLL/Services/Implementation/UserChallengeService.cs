using AutoMapper;
using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using LifeQuest.DAL.Models;
using LifeQuest.DAL.UOW.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeQuest.BLL.Services.Implementation
{
    internal class UserChallengeService : IUserChallengeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserChallengeService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public Task<UserChallengeDTO> GetChallengeDetailsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserChallengeDTO>> GetUserChallengesAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task JoinChallengeAsync(int userId, int ChallengeId)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().GetByIdAsync(userId);

            if (user == null) throw new KeyNotFoundException("User accounts was not found.");

            var challenge = _unitOfWork.Repository<Challenge>().GetByIdAsync(ChallengeId);

            if (challenge == null) throw new KeyNotFoundException("The selected challenge no longer exists.");


            return null;


        }
    }
}
