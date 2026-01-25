using MembersManagement.Application.ApplicationInterface;
using MembersManagement.Domain.Entities;
using MembersManagement.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class MemberController : Controller
{
    private readonly IMemberService _memberService;

    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    // Index
    public IActionResult Index()
    {
        var members = _memberService.GetMembers()
            .Select(m => new MemberViewModel
            {
                MemberID = m.MemberID,
                FirstName = m.FirstName,
                LastName = m.LastName,
                BirthDate = m.BirthDate.ToDateTime(TimeOnly.MinValue),
                Address = m.Address,
                Branch = m.Branch,
                ContactNo = m.ContactNo,
                Email = m.Email,
                IsActive = m.IsActive
            });

        return View(members);
    }

    // Create GET
    public IActionResult Create() => View();

    // Create POST
    [HttpPost]
    public IActionResult Create(MemberViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var member = new Member
        {
            FirstName = vm.FirstName,
            LastName = vm.LastName,
            BirthDate = DateOnly.FromDateTime(vm.BirthDate),
            Address = vm.Address,
            Branch = vm.Branch,
            ContactNo = vm.ContactNo,
            Email = vm.Email,
            IsActive = vm.IsActive,
            DateCreated = DateTime.UtcNow
        };

        try
        {
            _memberService.CreateMember(member);
        }
        catch (FluentValidation.ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(vm);
        }

        return RedirectToAction(nameof(Index));
    }

    // Edit GET
    public IActionResult Edit(int id)
    {
        var member = _memberService.GetMember(id);
        if (member == null) return NotFound();

        var vm = new MemberViewModel
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

        return View(vm);
    }

    // Edit POST
    [HttpPost]
    public IActionResult Edit(MemberViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var member = _memberService.GetMember(vm.MemberID);
        if (member == null)
            return NotFound();

        member.FirstName = vm.FirstName;
        member.LastName = vm.LastName;
        member.BirthDate = DateOnly.FromDateTime(vm.BirthDate);
        member.Address = vm.Address;
        member.Branch = vm.Branch;
        member.ContactNo = vm.ContactNo;
        member.Email = vm.Email;
        member.IsActive = vm.IsActive;

        try
        {
            _memberService.UpdateMember(member);
        }
        catch (FluentValidation.ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(vm);
        }

        return RedirectToAction(nameof(Index));
    }
}
