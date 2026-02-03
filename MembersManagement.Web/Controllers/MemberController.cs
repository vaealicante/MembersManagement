using Microsoft.AspNetCore.Mvc;
using MembersManagement.Application.ApplicationInterface;
using MembersManagement.Domain.Entities;
using MembersManagement.Web.ViewModels;
using System;
using System.Linq;

namespace MembersManagement.Web.Controllers
{
    public class MemberController(IMemberService memberService) : Controller
    {
        private readonly IMemberService _memberService = memberService;

        // ================= INDEX (With Dynamic PageSize) =================
        public IActionResult Index(string? search, string? branch, int page = 1, int pageSize = 5)
        {
            var members = _memberService.GetMembers();

            //  Apply Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                members = members.Where(m =>
                    m.LastName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            //  Apply Branch Filter
            if (!string.IsNullOrWhiteSpace(branch))
            {
                members = members.Where(m =>
                    m.Branch != null &&
                    m.Branch.Equals(branch, StringComparison.OrdinalIgnoreCase));
            }

            //  Pagination Calculation
            int totalMembers = members.Count();
            int originalPageSize = pageSize;
            // Check if "All" was selected (we'll use 0 or a large number)
            if (pageSize == 0)
            {
                pageSize = totalMembers > 0 ? totalMembers : 1;
                page = 1; // Reset to page 1 if showing all
            }

            int totalPages = (int)Math.Ceiling(totalMembers / (double)pageSize);

            // filter changed and made the current page invalid, reset to 1
            if (page > totalPages) page = 1;
            if (page < 1) page = 1;

            var membersToShow = members
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(m => new MemberViewModel
        {
            MemberID = m.MemberID,
            FirstName = m.FirstName,
            LastName = m.LastName,
            // Convert DateOnly to DateTime so the ViewModel can read it
            BirthDate = m.BirthDate.ToDateTime(TimeOnly.MinValue),
            Address = m.Address ?? "",
            Branch = m.Branch ?? "",
            ContactNo = m.ContactNo ?? "",
            Email = m.Email ?? "",
            IsActive = m.IsActive
        })
        .ToList();

            var branchList = _memberService.GetMembers()
                .Where(m => m.IsActive && !string.IsNullOrEmpty(m.Branch))
                .Select(m => m.Branch!)
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            // Pass everything to ViewBag
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize; // If they picked "All", this will be the total count
            ViewBag.SearchTerm = search;
            ViewBag.CurrentBranch = branch;
            ViewBag.Branches = branchList;
            ViewBag.PageSize = originalPageSize;

            return View(membersToShow);
        }

        // ================= DETAILS =================
        [HttpGet]
        public IActionResult Details(int id)
        {
            var member = _memberService.GetMember(id);
            if (member == null) return NotFound();

            var model = new MemberViewModel
            {
                MemberID = member.MemberID,
                FirstName = member.FirstName,
                LastName = member.LastName,
                BirthDate = member.BirthDate.ToDateTime(TimeOnly.MinValue),
                Address = member.Address ?? "",
                Branch = member.Branch ?? "",
                ContactNo = member.ContactNo ?? "",
                Email = member.Email ?? "",
                IsActive = member.IsActive,

                // --- MATCHING YOUR DOMAIN ENTITY ---
                CreatedDate = member.DateCreated
            };

            return View(model);
        }

        // ================= CREATE (GET) =================
        [HttpGet]
        public IActionResult Create()
        {
            PopulateBranches();
            return View();
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
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    BirthDate = DateOnly.FromDateTime(model.BirthDate),
                    Address = model.Address,
                    Branch = model.Branch,
                    ContactNo = model.ContactNo,
                    Email = model.Email,
                    IsActive = true
                };

                _memberService.CreateMember(member);
                TempData["SuccessMessage"] = "Member created successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (FluentValidation.ValidationException ex)
            {
                // Add FluentValidation errors to the UI ModelState
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                PopulateBranches();
                return View(model);
            }
        }

        // ================= EDIT (GET) =================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var member = _memberService.GetMember(id);
            if (member == null) return NotFound();

            var model = new MemberViewModel
            {
                MemberID = member.MemberID,
                FirstName = member.FirstName,
                LastName = member.LastName,
                BirthDate = member.BirthDate.ToDateTime(TimeOnly.MinValue),
                Address = member.Address!,
                Branch = member.Branch!,
                ContactNo = member.ContactNo!,
                Email = member.Email!,
                IsActive = member.IsActive
            };

            PopulateBranches();
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
                return View(model);
            }

            try
            {
                var member = _memberService.GetMember(model.MemberID);
                if (member == null) return NotFound();

                member.FirstName = model.FirstName;
                member.LastName = model.LastName;
                member.BirthDate = DateOnly.FromDateTime(model.BirthDate);
                member.Address = model.Address;
                member.Branch = model.Branch;
                member.ContactNo = model.ContactNo;
                member.Email = model.Email;
                member.IsActive = model.IsActive;

                _memberService.UpdateMember(member);
                TempData["SuccessMessage"] = "Member updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (FluentValidation.ValidationException ex)
            {
                // Add FluentValidation errors to the UI ModelState
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                PopulateBranches();
                return View(model);
            }
        }

        // ================= DELETE =================
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _memberService.DeleteMember(id);
                TempData["SuccessMessage"] = "Member deleted successfully.";
            }
            catch (KeyNotFoundException)
            {
                TempData["ErrorMessage"] = "Member not found.";
            }
            return RedirectToAction(nameof(Index));
        }

        // Helper to avoid repeated code
        private void PopulateBranches()
        {
            ViewBag.Branches = _memberService.GetMembers()
                .Where(m => m.IsActive && !string.IsNullOrEmpty(m.Branch))
                .Select(m => m.Branch!)
                .Distinct()
                .OrderBy(b => b)
                .ToList();
        }
    }
}