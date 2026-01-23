using FluentValidation;
using MembersManagement.Application.Services;
using MembersManagement.Application.Validators;
using MembersManagement.Application.ApplicationInterface;
using MembersManagement.Domain.Entities;
using MembersManagement.Domain.Interfaces;
using MembersManagement.Infrastructure.AppDbContext;
using MembersManagement.Infrastructure.RepositoryImplementation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Manual FluentValidation registration
builder.Services.AddTransient<IValidator<Member>, MemberValidation>();

// DB Context
builder.Services.AddDbContext<MemberDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository & Service
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IMemberService, MemberService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
