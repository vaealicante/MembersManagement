using Microsoft.AspNetCore.Mvc;
using MembersManagement.Web.Models;
using System.Diagnostics;
using System.Linq;
using MembersManagement.Domain.DomMemberModule.Entities;
using MembersManagement.Application.AppMemberModule.ApplicationInterface;

namespace MembersManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMemberService _memberService;

        public HomeController(ILogger<HomeController> logger, IMemberService memberService)
        {
            _logger = logger;
            _memberService = memberService;
        }

        public IActionResult Index()
        {
            var allMembers = _memberService.GetDashboardData().ToList();

            // Use explicit boolean checks
            ViewBag.TotalMembers = allMembers.Count;
            ViewBag.ActiveMembers = allMembers.Count(m => m.IsActive == true);
            ViewBag.InactiveMembers = allMembers.Count(m => m.IsActive == false); // Try explicit == false

            // Count branches
            // The '?' ensures Trim() only runs if Branch is not null
            ViewBag.BranchCount = allMembers
                .Where(m => !string.IsNullOrWhiteSpace(m.Branch))
                .Select(m => m.Branch?.Trim().ToUpper())
                .Distinct()
                .Count();

            var recentMembers = allMembers
                .Where(m => m.IsActive == true)
                .OrderByDescending(m => m.MemberID)
                .Take(5)
                .ToList();

            return View(recentMembers);
        }


        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}