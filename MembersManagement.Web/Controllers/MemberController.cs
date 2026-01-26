using Microsoft.AspNetCore.Mvc;
using MembersManagement.Application.ApplicationInterface;
using MembersManagement.Domain.Entities;
using MembersManagement.Web.ViewModels;
using FluentValidation;

public class MemberController : Controller
{
    private readonly IMemberService _memberService;

    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    public IActionResult Index(string? search)
    {
        var members = _memberService.GetMembers();

        if (!string.IsNullOrWhiteSpace(search))
        {
            members = members.Where(m =>
                m.LastName.Contains(search, StringComparison.OrdinalIgnoreCase)
            );
        }

        var vm = members.Select(m => new MemberViewModel
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
        });

        return View(vm);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        try
        {
            _memberService.DeleteMember(id); // Soft delete
            TempData["SuccessMessage"] = "Member deleted successfully.";
        }
        catch (KeyNotFoundException)
        {
            TempData["ErrorMessage"] = "Member not found.";
        }

        return RedirectToAction(nameof(Index));
    }
}
