using Microsoft.AspNetCore.Mvc;
using MembersManagement.Domain.Entities;
using MembersManagement.Application.ApplicationInterface;

namespace MembersManagement.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _service;

        public MemberController(IMemberService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var members = _service.GetMembers();
            return View(members);
        }

        [HttpPost]
        public IActionResult Create(Member member)
        {
            if (!ModelState.IsValid)
                return View(member);

            _service.CreateMember(member);
            return RedirectToAction(nameof(Index));
        }
    }
}
