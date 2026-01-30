using FluentValidation;
using FluentValidation.AspNetCore;
using MembersManagement.Application.ApplicationInterface;
using MembersManagement.Application.BusinessLogic;
using MembersManagement.Application.Services;
using MembersManagement.Application.Validators;
using MembersManagement.Domain.Entities;
using MembersManagement.Domain.Interfaces;
using MembersManagement.Infrastructure.AppDbContext;
using MembersManagement.Infrastructure.RepositoryImplementation;
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
builder.Services.AddDbContext<MemberDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository
builder.Services.AddScoped<IMemberRepository, MemberRepository>();

// Business Logic
builder.Services.AddScoped<MemberManager>();

// FluentValidation
builder.Services.AddScoped<IValidator<Member>, MemberValidation>();

//Service
builder.Services.AddScoped<IMemberService, MemberService>();

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
