using Microsoft.AspNetCore.Mvc;
using MembersManagement.Infrastructure.AppDbContext;
using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Web.ViewModels;

namespace MembersManagement.Web.Controllers
{
    public class BranchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BranchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Branch/Index
        public IActionResult Index()
        {
            var branches = _context.Branches
                                   .Where(b => b.IsActive) // Only show active branches
                                   .Select(b => new BranchViewModel
                                   {
                                       BranchId = b.BranchId,
                                       BranchName = b.BranchName,
                                       Location = b.Location,
                                       IsActive = b.IsActive,
                                       DateCreated = b.DateCreated
                                   })
                                   .ToList();

            return View("BranchIndex", branches);
        }


        // GET: Branch/CreateBranch
        public IActionResult CreateBranch()
        {
            return View("BranchCreate");
        }

        // POST: Branch/CreateBranch
        [HttpPost]
        public IActionResult CreateBranch(BranchViewModel model)
        {
            if (!ModelState.IsValid)
                return View("BranchCreate", model);

            var branch = new Branch
            {
                BranchName = model.BranchName,
                Location = model.Location,
                IsActive = model.IsActive,
                DateCreated = DateTime.Now
            };

            _context.Branches.Add(branch);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Branch/Edit/5
        public IActionResult Edit(int id)
        {
            var branch = _context.Branches.FirstOrDefault(b => b.BranchId == id);
            if (branch == null) return NotFound();

            var model = new BranchViewModel
            {
                BranchId = branch.BranchId,
                BranchName = branch.BranchName,
                Location = branch.Location,
                IsActive = branch.IsActive
            };

            return View("BranchEdit", model);
        }

        // POST: Branch/Edit/5
        [HttpPost]
        public IActionResult Edit(BranchViewModel model)
        {
            if (!ModelState.IsValid)
                return View("BranchEdit", model);

            var branch = _context.Branches.FirstOrDefault(b => b.BranchId == model.BranchId);
            if (branch == null) return NotFound();

            branch.BranchName = model.BranchName;
            branch.Location = model.Location;
            branch.IsActive = model.IsActive;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: Branch/SoftDelete/5
        [HttpPost]
        public IActionResult SoftDelete(int id)
        {
            var branch = _context.Branches.FirstOrDefault(b => b.BranchId == id);
            if (branch == null) return NotFound();

            branch.IsActive = false; // soft delete
            _context.SaveChanges();

            // ✅ Set TempData before redirect
            TempData["SuccessMessage"] = $"Branch '{branch.BranchName}' deleted successfully.";

            return RedirectToAction("Index");
        }

    }
}
