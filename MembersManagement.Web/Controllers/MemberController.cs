using Microsoft.AspNetCore.Mvc;
using MembersManagement.Application.ApplicationInterface;
using MembersManagement.Domain.Entities;
using MembersManagement.Web.ViewModels;
using System;
using System.Linq;
using System.Globalization;

namespace MembersManagement.Web.Controllers
{
    public class MemberController(IMemberService memberService) : Controller
    {
        private readonly IMemberService _memberService = memberService;

        // ================= INDEX (With Dynamic PageSize) =================
        public IActionResult Index(string? search, string? branch, int page = 1, int pageSize = 5)
        {
            var allMembers = _memberService.GetMembers();

            // GET ALL BRANCHES (Active or Inactive) from the database
            // Normalize to handle "Virac" vs "virac" duplicates
            ViewBag.Branches = allMembers
                .Where(m => !string.IsNullOrEmpty(m.Branch))
                .Select(m => m.Branch!.Trim().ToUpper())
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            var members = allMembers;

            // 2. Apply Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                members = members.Where(m =>
                    m.LastName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // 3. Apply Branch Filter
            if (!string.IsNullOrWhiteSpace(branch))
            {
                members = members.Where(m =>
                    m.Branch != null &&
                    m.Branch.Equals(branch, StringComparison.OrdinalIgnoreCase));
            }

            // 4. Pagination Calculation
            int totalMembers = members.Count();
            int originalPageSize = pageSize;

            if (pageSize == 0)
            {
                pageSize = totalMembers > 0 ? totalMembers : 1;
                page = 1;
            }

            int totalPages = (int)Math.Ceiling(totalMembers / (double)pageSize);

            if (page > totalPages) page = 1;
            if (page < 1) page = 1;

            // 5. Projecting to ViewModel (Fixing 'membersToShow' context error)
            var membersToShow = members
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MemberViewModel
                {
                    MemberID = m.MemberID,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    BirthDate = m.BirthDate.HasValue
                              ? m.BirthDate.Value.ToDateTime(TimeOnly.MinValue)
                                 : default,
                    Address = m.Address ?? "",
                    Branch = m.Branch ?? "",
                    ContactNo = m.ContactNo ?? "",
                    Email = m.Email ?? "",
                    IsActive = m.IsActive
                })
                .ToList();

            // 6. Pass everything to ViewBag
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = search;
            ViewBag.CurrentBranch = branch;
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
                BirthDate = member.BirthDate?.ToDateTime(TimeOnly.MinValue),
                Address = member.Address ?? "",
                Branch = member.Branch ?? "",
                ContactNo = member.ContactNo ?? "",
                Email = member.Email ?? "",
                IsActive = member.IsActive,
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
        // MemberController.cs
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
                    // Use .HasValue check for nullable Date
                    BirthDate = model.BirthDate.HasValue
                                ? DateOnly.FromDateTime(model.BirthDate.Value)
                                : null,
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
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                // Handle database or unexpected errors
                ModelState.AddModelError("", "An unexpected error occurred: " + ex.Message);
            }

            // This return ensures that if any catch block is hit, 
            // the user is sent back to the form with their data and error messages.
            PopulateBranches();
            return View(model);
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
                BirthDate = member.BirthDate?.ToDateTime(TimeOnly.MinValue),
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

            var member = _memberService.GetMember(model.MemberID);
            if (member == null) return NotFound();

            member.FirstName = model.FirstName;
            member.LastName = model.LastName;

            member.BirthDate = model.BirthDate.HasValue
                                ? DateOnly.FromDateTime(model.BirthDate.Value)
                                : null;

            member.Address = model.Address;
            member.Branch = model.Branch;
            member.ContactNo = model.ContactNo;
            member.Email = model.Email;
            member.IsActive = model.IsActive;

            _memberService.UpdateMember(member);

        // ========= SUCCESS MESSAGE ==========
            TempData["SuccessMessage"] = "Member updated successfully.";

            return RedirectToAction(nameof(Index));
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

        // Helper to ensure clean, distinct branches (Title Case)
        private void PopulateBranches()
        {
            // Same logic for Create/Edit screens
            ViewBag.Branches = _memberService.GetMembers()
                .Where(m => !string.IsNullOrEmpty(m.Branch))
                .Select(m => m.Branch!.Trim().ToUpper())
                .Distinct()
                .OrderBy(b => b)
                .ToList();
        }
    }
}
