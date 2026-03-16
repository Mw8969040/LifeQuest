using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LIfeQuest.PL.Controllers
{
    public class BadgeController : Controller
    {
        private readonly IBadgeService _badgeService;

        public BadgeController(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var badges = await _badgeService.GetAllBadgesAsync();
            return View(badges);
        }

        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
                return BadRequest();

            var badge = await _badgeService.GetBadgeByIdAsync(id);

            if (badge == null)
                return NotFound();

            return View(badge);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BadgeDTO badgeDTO)
        {
            if (!ModelState.IsValid)
                return View(badgeDTO);

            var exist = await _badgeService.GetBadgeByNameAsync(badgeDTO.Name);

            if (exist != null)
            {
                ModelState.AddModelError("Name", "Badge already exists");
                return View(badgeDTO);
            }

            await _badgeService.AddBadgeAsync(badgeDTO);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
                return BadRequest();

            var badge = await _badgeService.GetBadgeByIdAsync(id);

            if (badge == null)
            {
                ModelState.AddModelError("", "Not found");
                return View();
            }

            return View(badge);
        }

        // POST: Badge/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BadgeDTO badgeDTO)
        {
            if (!ModelState.IsValid)
               {
                ModelState.AddModelError("", "Invalid Badge");
                return View(badgeDTO);
               }

            var badge = await _badgeService.GetBadgeByIdAsync(badgeDTO.Id);

            if (badge == null)
            {
                ModelState.AddModelError("", "Not found");
            }

            await _badgeService.UpdateBadgeAsync(badgeDTO);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest();

            var badge = await _badgeService.GetBadgeByIdAsync(id);

            if (badge == null)
            {
                ModelState.AddModelError("", "Not found");
                return View();
            }

            return View(badge);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id <= 0)
                return BadRequest();

            await _badgeService.DeleteBadgeAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}