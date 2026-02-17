using FluentValidation;
using FluentValidation.AspNetCore;
using MembersManagement.Application.AppBranchModule.BranchApplicationInterface;
using MembersManagement.Application.AppBranchModule.BranchBusinessLogic;
using MembersManagement.Application.AppBranchModule.BranchServices;
using MembersManagement.Application.AppBranchModule.BranchValidators;
using MembersManagement.Application.AppMemberModule.ApplicationInterface;
using MembersManagement.Application.AppMemberModule.BusinessLogic;
using MembersManagement.Application.AppMemberModule.Services;
using MembersManagement.Application.AppMemberModule.Validators;
using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Domain.DomBranchModule.BranchInterfaces;
using MembersManagement.Domain.DomMemberModule.Entities;
using MembersManagement.Domain.DomMemberModule.Interfaces;
using MembersManagement.Infrastructure.AppDbContext;
using MembersManagement.Infrastructure.InfraBranchModule.BranchRepositoryImplementation;
using MembersManagement.Infrastructure.InfraMemberModule.RepositoryImplementation;
using MembersManagement.Web.ValidatorsVM;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<MemberValidation>();           // Domain
        fv.RegisterValidatorsFromAssemblyContaining<MemberViewModelValidator>();  // UI
    });

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();

// Business Logic
builder.Services.AddScoped<MemberManager>();
builder.Services.AddScoped<BranchManager>();

// FluentValidation
builder.Services.AddScoped<IValidator<Member>, MemberValidation>();
builder.Services.AddScoped<IValidator<Branch>, BranchValidation>();

//Service
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IBranchService, BranchService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Member}/{action=Index}/{id?}");

app.Run();
