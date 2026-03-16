using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LIfeQuest.PL.Controllers
{
    public class DailyLogController : Controller
    {
        private readonly IDailyLogService _dailyLogService;

        public DailyLogController(IDailyLogService dailyLogService)
        {
            _dailyLogService = dailyLogService;
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DailyLogDTO dto, int userId)
        {
            if (!ModelState.IsValid) return View(dto);

            var success = await _dailyLogService.AddLogAsync(dto, userId);

            if (!success)
            {
                ModelState.AddModelError("", "Failed to add daily log (maybe already exists or invalid user)");
                return View(dto);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Progress(int userChallengeId)
        {
            if (userChallengeId <= 0) return BadRequest();

            var percentage = await _dailyLogService.GetChallengeProcessPrecentageAsync(userChallengeId);

            if (percentage < 0)
            {
                ViewBag.ErrorMessage = "Challenge not found or invalid!";
                return View();
            }

            ViewBag.ProgressPercentage = percentage;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LogsByDate(int userId, DateTime date)
        {
            if (userId <= 0) return BadRequest();

            var logs = await _dailyLogService.GetLogsByDateAsync(userId, date);
            if (logs == null || !logs.Any())
            {
                ViewBag.ErrorMessage = "No logs found for this date";
                return View();
            }

            return View(logs);
        }

        [HttpGet]
        public async Task<IActionResult> Streak(int userId)
        {
            if (userId <= 0) return BadRequest();

            var streak = await _dailyLogService.GetUserSteakAsync(userId);
            ViewBag.Streak = streak;
            return View();
        }
    }
}