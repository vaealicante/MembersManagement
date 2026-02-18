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

        // ================= INDEX (List & Filter) =================
        public IActionResult Index(string? search, string? branch, int page = 1, int pageSize = 5)
        {
            var allMembers = _memberService.GetMembers().ToList();

            ViewBag.BranchesList = _branchService.GetAllBranches()
                .Select(b => b.BranchName.Trim().ToUpper())
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            var members = allMembers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                members = members.Where(m =>
                    m.LastName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

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

        // ================= BRANCH INDEX =================
        [HttpGet]
        public IActionResult BranchIndex()
        {
            var branches = _branchService.GetAllBranches()
                .OrderBy(b => b.BranchName)
                .ToList();

            // Tell MVC the full path
            return View("~/Views/Branch/BranchIndex.cshtml", branches);
        }


        // ================= CREATE MEMBER =================
        [HttpGet]
        public IActionResult Create()
        {
            PopulateBranches();
            return View(new MemberViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MemberViewModel model)
        {
            var selectedBranch = GetBranchByName(model.Branch);

            if (selectedBranch == null)
                ModelState.AddModelError("Branch", "The branch name entered is invalid. Please select from the list.");

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
                    BranchId = selectedBranch!.BranchId,
                    ContactNo = model.ContactNo,
                    Email = model.Email,
                    IsActive = true,
                    DateCreated = DateTime.UtcNow
                };

                _memberService.CreateMember(member);
                TempData["SuccessMessage"] = "Member created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Database Error: " + ex.InnerException?.Message ?? ex.Message);
                PopulateBranches();
                return View(model);
            }
        }

        // ================= EDIT MEMBER =================
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
                Branch = member.Branch?.BranchName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MemberViewModel model)
        {
            var selectedBranch = GetBranchByName(model.Branch);

            if (selectedBranch == null && !string.IsNullOrEmpty(model.Branch))
                ModelState.AddModelError("Branch", "Selected branch does not exist.");

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
            member.BranchId = selectedBranch?.BranchId ?? member.BranchId;
            member.IsActive = model.IsActive;

            _memberService.UpdateMember(member);
            TempData["SuccessMessage"] = "Member updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // ================= SOFT DELETE =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var member = _memberService.GetMember(id);
            if (member == null) return NotFound();

            member.IsActive = false;
            _memberService.UpdateMember(member);

            TempData["SuccessMessage"] = "Member deactivated.";
            return RedirectToAction(nameof(Index));
        }

        // ================= HELPERS =================
        private void PopulateBranches()
        {
            ViewBag.Branches = _branchService.GetAllBranches()
                .Select(b => b.BranchName)
                .OrderBy(b => b)
                .ToList();
        }

        private Branch? GetBranchByName(string? name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return _branchService.GetAllBranches()
                .FirstOrDefault(b => b.BranchName.Trim().Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
