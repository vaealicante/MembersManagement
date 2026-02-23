using MembersManagement.Application.AppMembershipModule.MembershipApplicationInterface;
using MembersManagement.Domain.DomMembershipModule.MembershipEntities;
using MembersManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MembersManagement.Web.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        // ---------- INDEX ----------
        [HttpGet]
        public IActionResult Index()
        {
            var memberships = _membershipService.GetAllMemberships()
                .Where(m => m.IsActive) // only active memberships
                .Select(m => new MembershipViewModel
                {
                    MembershipId = m.MembershipId,
                    MembershipName = m.MembershipName,
                    IsActive = m.IsActive,
                    DateCreated = m.DateCreated
                })
                .ToList();

            return View(memberships);
        }

        // ---------- CREATE ----------
        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MembershipViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var membership = new Membership
            {
                MembershipName = model.MembershipName,
                IsActive = model.IsActive,
                DateCreated = DateTime.UtcNow
            };

            _membershipService.CreateMembership(membership);

            TempData["SuccessMessage"] = "Membership created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ---------- EDIT ----------
        // GET: Membership/Edit/5
        public IActionResult Edit(int id)
        {
            var membership = _membershipService.GetMembership(id);
            if (membership == null) return NotFound();

            var vm = new MembershipViewModel
            {
                MembershipId = membership.MembershipId,
                MembershipName = membership.MembershipName,
                IsActive = membership.IsActive,
                DateCreated = membership.DateCreated
            };

            return View(vm);
        }

        // POST: Membership/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MembershipViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var membership = _membershipService.GetMembership(vm.MembershipId);
            if (membership == null) return NotFound();

            // Update properties
            membership.MembershipName = vm.MembershipName;
            membership.IsActive = vm.IsActive;

            _membershipService.UpdateMembership(membership);

            TempData["SuccessMessage"] = "Membership updated successfully!";
            return RedirectToAction(nameof(Index));
        }
        // ---------- DELETE ----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var membership = _membershipService.GetMembership(id);
            if (membership == null) return NotFound();

            _membershipService.DeleteMembership(id);
            TempData["SuccessMessage"] = "Membership deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}