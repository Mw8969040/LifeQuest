using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LIfeQuest.PL.Controllers
{
    public class UserChallengeController : Controller
    {
        private readonly IUserChallengeService _userChallengeService;

        public UserChallengeController(IUserChallengeService userChallengeService)
        {
            _userChallengeService = userChallengeService;
        }

        [HttpGet]
        public async Task<IActionResult> UserChallenges(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid user.");

            var challenges = await _userChallengeService.GetUserChallengesAsync(userId);

            if (challenges == null || !challenges.Any())
                ViewBag.Message = "No challenges joined yet.";

            return View(challenges);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int userId, int challengeId)
        {
            if (userId <= 0 || challengeId <= 0)
                return BadRequest("Invalid userId or challengeId.");

            var challenge = await _userChallengeService.GetChallengeDetailsAsync(userId, challengeId);
            return View(challenge);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int userId, int challengeId)
        {
            if (userId <= 0 || challengeId <= 0)
                return BadRequest("Invalid userId or challengeId.");

            await _userChallengeService.JoinChallengeAsync(userId, challengeId);

            TempData["SuccessMessage"] = "Successfully joined the challenge!";
            return RedirectToAction(nameof(UserChallenges), new { userId });
        }
    }
}