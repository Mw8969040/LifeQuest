using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LIfeQuest.PL.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid userId.");

            var profile = await _userProfileService.GetUserProfileAsync(userId);

            if (profile == null)
            {
                ViewBag.ErrorMessage = "User profile not found.";
                return View();
            }

            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserProfileDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _userProfileService.UpdateUserProfileAsync(dto);

            return RedirectToAction(nameof(Details), new { userId = dto.UserId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPoints(int userId, int points)
        {
            if (userId <= 0 || points <= 0)
                return BadRequest("Invalid input.");

            await _userProfileService.AddPointsToUserAsync(userId, points);

            return RedirectToAction(nameof(Details), new { userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSuccessRate(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid userId.");

            await _userProfileService.UpdateSuccessRateAsync(userId);

            return RedirectToAction(nameof(Details), new { userId });
        }
    }
}