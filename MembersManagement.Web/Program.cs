using FluentValidation;
using MembersManagement.Application.ApplicationInterface;
using MembersManagement.Application.BusinessLogic;
using MembersManagement.Application.Services;
using MembersManagement.Application.Validators;
using MembersManagement.Domain.Entities;
using MembersManagement.Domain.Interfaces;
using MembersManagement.Infrastructure.AppDbContext;
using MembersManagement.Infrastructure.RepositoryImplementation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
