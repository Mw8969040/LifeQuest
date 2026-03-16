using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LIfeQuest.PL.Controllers
{
    public class DecisionController : Controller
    {
        private readonly IDecisionService _decisionService;

        public DecisionController(IDecisionService decisionService)
        {
            _decisionService = decisionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int userId)
        {
            if (userId <= 0) return BadRequest();

            var decisions = await _decisionService.GetUserDecisionsAsync(userId);
            return View(decisions);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int decisionId)
        {
            if (decisionId <= 0) return BadRequest();

            var decision = await _decisionService.GetDecisionDetailsAsync(decisionId);
            if (decision == null)
            {
                ViewBag.ErrorMessage = "Decision not found";
                return View();
            }

            return View(decision);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DecisionDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var success = await _decisionService.AddDecisionAsync(dto);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to add decision (user may not exist or invalid data)");
                return View(dto);
            }

            return RedirectToAction(nameof(Index), new { userId = dto.UserId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int decisionId)
        {
            if (decisionId <= 0) return BadRequest();

            var decision = await _decisionService.GetDecisionDetailsAsync(decisionId);
            if (decision == null)
            {
                ViewBag.ErrorMessage = "Decision not found";
                return View();
            }

            return View(decision);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int decisionId, bool isSuccess)
        {
            if (decisionId <= 0) return BadRequest();

            var result = await _decisionService.UpdateDecisionResultAsync(decisionId, isSuccess);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to update decision result");
                return View();
            }

            return RedirectToAction(nameof(Details), new { decisionId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int decisionId)
        {
            if (decisionId <= 0) return BadRequest();

            var decision = await _decisionService.GetDecisionDetailsAsync(decisionId);
            if (decision == null)
            {
                ViewBag.ErrorMessage = "Decision not found";
                return View();
            }

            return View(decision);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int decisionId)
        {
            var result = await _decisionService.DeleteDecisionAsync(decisionId);
            if (!result)
            {
                ModelState.AddModelError("", "Failed to delete decision");
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}