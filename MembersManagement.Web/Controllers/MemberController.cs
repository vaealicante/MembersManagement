using Microsoft.AspNetCore.Mvc;
using MembersManagement.Domain.DomMemberModule.Entities;
using MembersManagement.Web.ViewModels;
using System;
using System.Linq;
using MembersManagement.Application.AppMemberModule.ApplicationInterface;
using MembersManagement.Application.AppBranchModule.BranchApplicationInterface;

namespace MembersManagement.Web.Controllers
{
    // Using the Primary Constructor syntax to inject both required services
    public class MemberController(
        IMemberService memberService,
        IBranchService branchService) : Controller
    {
        private readonly IMemberService _memberService = memberService;
        private readonly IBranchService _branchService = branchService;

        // ================= INDEX =================
        public IActionResult Index(string? search, string? branch, int page = 1, int pageSize = 5)
        {
            var allMembers = _memberService.GetMembers().ToList();

            // Use the Branch service to get all possible branches for the dropdown
            ViewBag.Branches = _branchService.GetAllBranches()
                .Select(b => b.BranchName.Trim().ToUpper())
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            var members = allMembers.AsEnumerable();

            // Search Logic
            if (!string.IsNullOrWhiteSpace(search))
            {
                members = members.Where(m =>
                    m.LastName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Filter by Branch Logic
            if (!string.IsNullOrWhiteSpace(branch))
            {
                members = members.Where(m =>
                    m.Branch != null &&
                    m.Branch.BranchName.Equals(branch, StringComparison.OrdinalIgnoreCase));
            }

            int totalMembers = members.Count();
            int totalPages = (int)Math.Ceiling(totalMembers / (double)pageSize);

            var membersToShow = members
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MemberViewModel
                {
                    MemberID = m.MemberID,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    BirthDate = m.BirthDate?.ToDateTime(TimeOnly.MinValue),
                    Address = m.Address ?? "",
                    // Display the name of the branch in the list
                    Branch = m.Branch?.BranchName ?? "N/A",
                    ContactNo = m.ContactNo ?? "",
                    Email = m.Email ?? "",
                    IsActive = m.IsActive
                })
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = search;
            ViewBag.CurrentBranch = branch;
            ViewBag.PageSize = pageSize;

            return View(membersToShow);
        }

        // ================= CREATE (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateBranches();
                return View(model);
            }

            try
            {
                var member = new Member
                {
                    FirstName = model.FirstName ?? string.Empty,
                    LastName = model.LastName ?? string.Empty,
                    BirthDate = model.BirthDate.HasValue ? DateOnly.FromDateTime(model.BirthDate.Value) : null,
                    Address = model.Address,
                    // Map the Foreign Key ID from the ViewModel
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Internal Error: " + ex.Message);
                PopulateBranches();
                return View(model);
            }
        }

        // ================= EDIT (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MemberViewModel model)
        {
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
            member.BranchId = model.BranchId;
            member.ContactNo = model.ContactNo;
            member.Email = model.Email;
            member.IsActive = model.IsActive;

            _memberService.UpdateMember(member);
            TempData["SuccessMessage"] = "Member updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        private void PopulateBranches()
        {
            // Fetch list of branches to populate dropdowns in Create/Edit views
            ViewBag.Branches = _branchService.GetAllBranches()
                .OrderBy(b => b.BranchName)
                .ToList();
        }
    }
}