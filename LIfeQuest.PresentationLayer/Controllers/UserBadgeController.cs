using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LIfeQuest.PL.Controllers
{
    public class UserBadgeController : Controller
    {
        private readonly IUserBadgeService _userBadgeService;

        public UserBadgeController(IUserBadgeService userBadgeService)
        {
            _userBadgeService = userBadgeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid userId.");

            var badges = await _userBadgeService.GetUserBadgesAsync(userId);

            if (badges == null || !badges.Any())
            {
                ViewBag.Message = "No badges awarded yet.";
                return View();
            }

            return View(badges);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckAndAward(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid userId.");

            await _userBadgeService.CheckAndAwardBadgesAsync(userId);

            return RedirectToAction(nameof(Index), new { userId });
        }


    }
}