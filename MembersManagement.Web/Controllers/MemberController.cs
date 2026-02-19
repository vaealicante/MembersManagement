using MembersManagement.Application.AppBranchModule.BranchApplicationInterface;
using MembersManagement.Application.AppMemberModule.ApplicationInterface;
using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Domain.DomMemberModule.Entities;
using MembersManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace MembersManagement.Web.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IBranchService _branchService;

        public MemberController(IMemberService memberService, IBranchService branchService)
        {
            _memberService = memberService;
            _branchService = branchService;
        }

        // ================= INDEX =================
        public IActionResult Index(string? search, string? branch, int page = 1, int pageSize = 5)
        {
            var allMembers = _memberService.GetMembers().ToList();

            // Branch dropdown
            ViewBag.BranchesList = _branchService.GetAllBranches()
                .Select(b => b.BranchName.Trim())
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            var members = allMembers.AsEnumerable();

            // SEARCH FILTER
            if (!string.IsNullOrWhiteSpace(search))
            {
                members = members.Where(m =>
                    m.LastName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    m.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // BRANCH FILTER
            if (!string.IsNullOrWhiteSpace(branch))
            {
                members = members.Where(m =>
                    m.Branch != null &&
                    m.Branch.BranchName.Equals(branch, StringComparison.OrdinalIgnoreCase));
            }

            int totalMembers = members.Count();

            // HANDLE "ALL"
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
            return View(new MemberViewModel());
        }

        // ================= CREATE (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MemberViewModel model)
        {
            var selectedBranch = model.BranchId.HasValue
                ? _branchService.GetAllBranches().FirstOrDefault(b => b.BranchId == model.BranchId)
                : GetBranchByName(model.Branch);

            if (!ModelState.IsValid)
            {
                PopulateBranches();
                return View(model);
            }

            var member = new Member
            {
                FirstName = model.FirstName ?? string.Empty,
                LastName = model.LastName ?? string.Empty,
                BirthDate = model.BirthDate.HasValue ? DateOnly.FromDateTime(model.BirthDate.Value) : null,
                Address = model.Address,
                BranchId = selectedBranch?.BranchId,
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
                Branch = member.Branch?.BranchName
            };

            return View(model);
        }

        // ================= EDIT (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MemberViewModel model)
        {
            var selectedBranch = model.BranchId.HasValue ?
                _branchService.GetAllBranches().FirstOrDefault(b => b.BranchId == model.BranchId) : GetBranchByName(model.Branch);

            if (!ModelState.IsValid)
            {
                PopulateBranches();
                return View(model);
            }

            var member = _memberService.GetMember(model.MemberID);
            if (member == null) return NotFound();

            member.FirstName = model.FirstName ?? string.Empty;
            member.LastName = model.LastName ?? string.Empty;
            member.BirthDate = model.BirthDate.HasValue ? DateOnly.FromDateTime(model.BirthDate.Value) : null;
            member.Address = model.Address;
            member.ContactNo = model.ContactNo;
            member.Email = model.Email;
            member.BranchId = selectedBranch?.BranchId;
            member.IsActive = model.IsActive;

            _memberService.UpdateMember(member);
            TempData["SuccessMessage"] = "Member updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE (POST) =================
        // POST: Member/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var member = _memberService.GetMember(id);
            if (member == null)
            {
                return NotFound();
            }

            // This calls the Manager -> Repository -> SQL Update
            _memberService.DeleteMember(id);

            TempData["SuccessMessage"] = $"Member '{member.FirstName} {member.LastName}' deleted successfully.";

            // CRITICAL: This sends the user back to the list
            return RedirectToAction(nameof(Index));
        }

        // ================= HELPERS =================
        private void PopulateBranches()
        {
            ViewBag.Branches = _branchService.GetAllBranches()
                .Where(b => b.IsActive)
                .OrderBy(b => b.BranchName)
                .ToList();
        }

        private Branch? GetBranchByName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;

            return _branchService.GetAllBranches()
                .FirstOrDefault(b => b.BranchName.Trim().Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}