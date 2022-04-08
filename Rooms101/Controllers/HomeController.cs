using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rooms101.Models;
using Rooms101.Services;
using System.Diagnostics;

namespace Rooms101.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMeetingsService _meetingsService;

        public HomeController(ILogger<HomeController> logger,
                            IMeetingsService meetingsService)
        {
            _logger = logger;
            _meetingsService = meetingsService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var meetings = await _meetingsService.GetMeetingsJsonAsync();

            ViewData["calendarJson"] = meetings;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}