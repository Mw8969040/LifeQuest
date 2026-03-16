using LifeQuest.BLL.DTOs;
using LifeQuest.BLL.Services.Interfaces;
using LIfeQuest.PresentationLayer.ViewModels.Challenges;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace LIfeQuest.PresentationLayer.Controllers
{
    public class ChallengesController : Controller
    {
        private readonly IChallengeService _challengeService;

        public ChallengesController(IChallengeService challengeService)
        {
            _challengeService = challengeService;
        }

        // GET: Challenges
        public async Task<IActionResult> Index()
        {
            var challenges = await _challengeService.GetAllChallengesAsync();
            var viewModel = challenges.Select(c => new ChallengeViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Points = c.Points,
                Duration = c.Duration,
                IsPublic = c.IsPublic,
                Difficulty = c.Difficulty,
                CreatedAt = c.CreatedAt
            }).ToList();

            return View(viewModel);
        }

        // GET: Challenges/Create
        public IActionResult Create()
        {
            return View(new CreateChallengeViewModel());
        }

        // POST: Challenges/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateChallengeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dto = new ChallengeDTO
                {
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate,
                    Points = viewModel.Points,
                    Difficulty = viewModel.Difficulty,
                    IsPublic = viewModel.IsPublic
                };

                await _challengeService.CreateChallengeAsync(dto);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public IActionResult Details()
        {
            return View();
        }

        [Route("Challenges/Join-Challenge")]
        [Route("Challenges/Join")]
        public IActionResult Join()
        {
            return View();
        }

        [Route("Challenges/Daily-CheckIn")]
        [Route("Challenges/DailyCheckIn")]
        public IActionResult DailyCheckIn()
        {
            return View();
        }
    }
}
