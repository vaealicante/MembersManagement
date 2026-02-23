using MembersManagement.Application.AppBranchModule.BranchApplicationInterface;
using MembersManagement.Application.AppMemberModule.ApplicationInterface;
using MembersManagement.Application.AppMembershipModule.MembershipApplicationInterface;
using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Domain.DomMemberModule.Entities;
using MembersManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace MembersManagement.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IBranchService _branchService;
        private readonly IMembershipService _membershipService;

        // ✅ FIXED CONSTRUCTOR
        public MemberController(
            IMemberService memberService,
            IBranchService branchService,
            IMembershipService membershipService)
        {
            _memberService = memberService;
            _branchService = branchService;
            _membershipService = membershipService;
        }

        // ================= INDEX =================
        public IActionResult Index(string? search, string? branch, int page = 1, int pageSize = 5)
        {
            var allMembers = _memberService.GetMembers().ToList();

            ViewBag.BranchesList = _branchService.GetAllBranches()
                .Select(b => b.BranchName.Trim())
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            var members = allMembers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                members = members.Where(m =>
                    m.LastName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    m.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(branch))
            {
                members = members.Where(m =>
                    m.Branch != null &&
                    m.Branch.BranchName.Equals(branch, StringComparison.OrdinalIgnoreCase));
            }

            int totalMembers = members.Count();
            bool showAll = pageSize == 0;

            int totalPages = showAll
                ? 1
                : (int)Math.Ceiling(totalMembers / (double)pageSize);

            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var pagedMembers = showAll
                ? members
                : members.Skip((page - 1) * pageSize).Take(pageSize);

            var membersToShow = pagedMembers
                .Select(m => new MemberViewModel
                {
                    MemberID = m.MemberID,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    BirthDate = m.BirthDate?.ToDateTime(TimeOnly.MinValue),
                    Address = m.Address ?? "",
                    BranchId = m.BranchId,
                    Branch = m.Branch?.BranchName ?? "",
                    Membership = m.Membership != null ? m.Membership.MembershipName : "",
                    ContactNo = m.ContactNo ?? "",
                    Email = m.Email ?? "",
                    IsActive = m.IsActive,
                    CreatedDate = m.DateCreated
                })
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = search;
            ViewBag.CurrentBranch = branch;
            ViewBag.PageSize = pageSize;

            return View(membersToShow);
        }

        // ================= CREATE (GET) =================
        [HttpGet]
        public IActionResult Create()
        {
            PopulateBranches();
            PopulateMemberships();
            return View(new MemberViewModel());
        }

        // ================= CREATE (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateBranches();
                PopulateMemberships();
                return View(model);
            }

            var member = new Member
            {
                FirstName = model.FirstName ?? "",
                LastName = model.LastName ?? "",
                BirthDate = model.BirthDate.HasValue
                    ? DateOnly.FromDateTime(model.BirthDate.Value)
                    : null,
                Address = model.Address,
                MembershipId = model.MembershipId,
                BranchId = model.BranchId,
                ContactNo = model.ContactNo,
                Email = model.Email,
                IsActive = true,
                DateCreated = DateTime.UtcNow
            };

            _memberService.CreateMember(member);
            TempData["SuccessMessage"] = "Member created successfully.";

            return RedirectToAction(nameof(Index));
        }


        // ================= EDIT (GET) =================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var member = _memberService.GetMember(id);
            if (member == null) return NotFound();

            PopulateBranches();
            PopulateMemberships();

            var model = new MemberViewModel
            {
                MemberID = member.MemberID,
                FirstName = member.FirstName,
                LastName = member.LastName,
                BirthDate = member.BirthDate?.ToDateTime(TimeOnly.MinValue),
                Address = member.Address,
                ContactNo = member.ContactNo,
                Email = member.Email,
                IsActive = member.IsActive,
                BranchId = member.BranchId,
                MembershipId = member.MembershipId
            };

            return View(model);
        }

        // ================= EDIT (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateBranches();
                PopulateMemberships();
                return View(model);
            }

            var member = _memberService.GetMember(model.MemberID);
            if (member == null) return NotFound();

            member.FirstName = model.FirstName ?? "";
            member.LastName = model.LastName ?? "";
            member.BirthDate = model.BirthDate.HasValue
                ? DateOnly.FromDateTime(model.BirthDate.Value)
                : null;
            member.Address = model.Address;
            member.ContactNo = model.ContactNo;
            member.Email = model.Email;
            member.BranchId = model.BranchId;
            member.MembershipId = model.MembershipId;
            member.IsActive = model.IsActive;

            _memberService.UpdateMember(member);
            TempData["SuccessMessage"] = "Member updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _memberService.DeleteMember(id);
            TempData["SuccessMessage"] = "Member deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ================= HELPERS =================
        private void PopulateBranches()
        {
            ViewBag.BranchesList = _branchService.GetAllBranches()
                .Where(b => b.IsActive)
                .OrderBy(b => b.BranchName)
                .Select(b => new SelectListItem
                {
                    Value = b.BranchId.ToString(),
                    Text = b.BranchName
                })
                .ToList();
        }

        private void PopulateMemberships()
        {
            ViewBag.Memberships = _membershipService
                .GetAllMemberships()
                .Where(m => m.IsActive)
                .OrderBy(m => m.MembershipName)
                .Select(m => new SelectListItem
                {
                    Value = m.MembershipId.ToString(),
                    Text = m.MembershipName
                })
                .ToList();
        }
    }
}
