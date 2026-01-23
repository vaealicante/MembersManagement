using Microsoft.AspNetCore.Mvc;
using MembersManagement.Application.ApplicationInterface;
using MembersManagement.Web.VMC.ViewModels;

namespace MembersManagement.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // GET: List all members
        public IActionResult Index()
        {
            var members = _memberService.GetMembers();

            // Map domain entity to ViewModel
            var memberVMs = members.Select(m => new MemberViewModel
            {
                MemberID = m.MemberID,
                FirstName = m.FirstName,
                LastName = m.LastName,
                BirthDate = m.BirthDate,
                Address = m.Address,
                Branch = m.Branch,
                ContactNo = m.ContactNo,
                Email = m.Email,
                IsActive = m.IsActive,
                DateCreated = m.DateCreated
            }).ToList();

            return View(memberVMs);
        }

        // GET: Show create form
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create member (delegates all logic to service)
        [HttpPost]
        public IActionResult Create(MemberViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            // Delegate creation to service
            _memberService.CreateMember(new MembersManagement.Domain.Entities.Member
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                BirthDate = vm.BirthDate,
                Address = vm.Address,
                Branch = vm.Branch,
                ContactNo = vm.ContactNo,
                Email = vm.Email
            });

            return RedirectToAction(nameof(Index));
        }

        // GET: Show edit form
        public IActionResult Edit(int id)
        {
            var member = _memberService.GetMember(id);
            if (member == null) return NotFound();

            var vm = new MemberViewModel
            {
                MemberID = member.MemberID,
                FirstName = member.FirstName,
                LastName = member.LastName,
                BirthDate = member.BirthDate,
                Address = member.Address,
                Branch = member.Branch,
                ContactNo = member.ContactNo,
                Email = member.Email,
                IsActive = member.IsActive,
                DateCreated = member.DateCreated
            };

            return View(vm);
        }

        // POST: Update member (delegates to service)
        [HttpPost]
        public IActionResult Edit(MemberViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            _memberService.UpdateMember(new MembersManagement.Domain.Entities.Member
            {
                MemberID = vm.MemberID,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                BirthDate = vm.BirthDate,
                Address = vm.Address,
                Branch = vm.Branch,
                ContactNo = vm.ContactNo,
                Email = vm.Email,
                IsActive = vm.IsActive
            });

            return RedirectToAction(nameof(Index));
        }

        // GET: Show delete confirmation
        public IActionResult Delete(int id)
        {
            var member = _memberService.GetMember(id);
            if (member == null) return NotFound();

            var vm = new MemberViewModel
            {
                MemberID = member.MemberID,
                FirstName = member.FirstName,
                LastName = member.LastName,
                BirthDate = member.BirthDate,
                Address = member.Address,
                Branch = member.Branch,
                ContactNo = member.ContactNo,
                Email = member.Email,
                IsActive = member.IsActive,
                DateCreated = member.DateCreated
            };

            return View(vm);
        }

        // POST: Delete member
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var member = _memberService.GetMember(id);
            if (member == null) return NotFound();

            _memberService.DeleteMember(member);
            return RedirectToAction(nameof(Index));
        }
    }
}
