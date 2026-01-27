using Microsoft.AspNetCore.Mvc;
using MembersManagement.Application.ApplicationInterface;
using MembersManagement.Domain.Entities;
using MembersManagement.Web.ViewModels;
using System;
using System.Linq;

namespace MembersManagement.Web.Controllers
{
    // Controller class must inherit from Controller
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private const int PageSize = 5; // members per page

        // Constructor injects the service
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // ================= INDEX =================
        public IActionResult Index(string? search, string? branch, int page = 1)
        {
            var members = _memberService.GetMembers();

            // Apply search
            if (!string.IsNullOrWhiteSpace(search))
            {
                members = members.Where(m =>
                    m.FirstName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    m.LastName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    (m.Email != null && m.Email.Contains(search, StringComparison.OrdinalIgnoreCase))
                );
            }

            // Apply branch filter
            if (!string.IsNullOrWhiteSpace(branch))
            {
                members = members.Where(m =>
                    m.Branch != null &&
                    m.Branch.Equals(branch, StringComparison.OrdinalIgnoreCase));
            }

            // Pagination
            int totalMembers = members.Count();
            int totalPages = (int)Math.Ceiling(totalMembers / (double)PageSize);

            var membersToShow = members
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .Select(m => new MemberViewModel
                {
                    MemberID = m.MemberID,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    BirthDate = m.BirthDate.ToDateTime(TimeOnly.MinValue),
                    Branch = m.Branch,
                    Address = m.Address,
                    ContactNo = m.ContactNo,
                    IsActive = m.IsActive
                })
                .ToList();

            // Branch dropdown
            ViewBag.Branches = _memberService.GetMembers()
                .Where(m => m.IsActive && !string.IsNullOrEmpty(m.Branch))
                .Select(m => m.Branch!)
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            ViewBag.CurrentBranch = branch;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchTerm = search;

            return View(membersToShow);
        }

        // ================= CREATE (GET) =================
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Branches = _memberService.GetMembers()
                .Where(m => m.IsActive && !string.IsNullOrEmpty(m.Branch))
                .Select(m => m.Branch!)
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            return View();
        }

        // ================= CREATE (POST) =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate Branch dropdown
                ViewBag.Branches = _memberService.GetMembers()
                    .Where(m => m.IsActive && !string.IsNullOrEmpty(m.Branch))
                    .Select(m => m.Branch!)
                    .Distinct()
                    .OrderBy(b => b)
                    .ToList();

                return View(model);
            }

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

        // ================= EDIT (GET) =================
        // Loads the Edit page with existing member data
        // URL: /Member/Edit/3
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Get member from database
            var member = _memberService.GetMember(id);

            // If member not found, return 404
            if (member == null)
            {
                return NotFound();
            }

            // Map Entity -> ViewModel
            var model = new MemberViewModel
            {
                MemberID = member.MemberID,
                FirstName = member.FirstName,
                LastName = member.LastName,
                BirthDate = member.BirthDate.ToDateTime(TimeOnly.MinValue),
                Address = member.Address,
                Branch = member.Branch,
                ContactNo = member.ContactNo,
                Email = member.Email,
                IsActive = member.IsActive
            };

            // Populate Branch dropdown
            ViewBag.Branches = _memberService.GetMembers()
                .Where(m => m.IsActive && !string.IsNullOrEmpty(m.Branch))
                .Select(m => m.Branch!)
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            return View(model);
        }

        // ================= EDIT (POST) =================
        // Saves the updated member data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate Branch dropdown if validation fails
                ViewBag.Branches = _memberService.GetMembers()
                    .Where(m => m.IsActive && !string.IsNullOrEmpty(m.Branch))
                    .Select(m => m.Branch!)
                    .Distinct()
                    .OrderBy(b => b)
                    .ToList();

                return View(model);
            }

            // Get existing member
            var member = _memberService.GetMember(model.MemberID);

            if (member == null)
            {
                return NotFound();
            }

            // Update fields
            member.FirstName = model.FirstName;
            member.LastName = model.LastName;
            member.BirthDate = DateOnly.FromDateTime(model.BirthDate);
            member.Address = model.Address;
            member.Branch = model.Branch;
            member.ContactNo = model.ContactNo;
            member.Email = model.Email;
            member.IsActive = model.IsActive;

            // Save changes
            _memberService.UpdateMember(member);

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
    }
}
