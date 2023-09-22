using AutoMapper;
using BRB.Attributes;
using BusinessObjects.Helper;
using BusinessObjects.Models;
using BusinessObjects.Services;
using BusinessObjects.Services.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Runtime.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<Wh4lprodContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConn"));
    //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".brb.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MapperConfig());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IResumeService, ResumeService>();
builder.Services.AddScoped<IContactInfoService, ContactInfoService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IDropdownService, DropdownService>();
//builder.Services.AddScoped<IEducationService, EducationService>();
builder.Services.AddScoped<IObjectiveService, ObjectiveService>();
builder.Services.AddScoped<IOverseasStudyService, OverseasStudyService>();
builder.Services.AddScoped<IMilitaryService, MilitaryService>();
builder.Services.AddScoped<IOrginazationService, OrginazationService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<IProfessionalService, ProfessionalService>();
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<ITechnicalSkillService, TechnicalSkillService>();
builder.Services.AddScoped<IWorkExperienceService, WorkExperienceService>();
//builder.Services.AddOutputCache(options =>
//{
//    options.AddBasePolicy(builder => builder.Cache());
//    options.AddPolicy("OutputCacheWithAuthPolicy",p => p.Expire(TimeSpan.FromSeconds(20)));
//});
builder.Services.AddMvc(options =>
{
    options.Filters.Add(new GlobalFilterAttribute());
 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseStatusCodePages(ctx =>
{
    if (ctx.HttpContext.Response.StatusCode == 404)
        ctx.HttpContext.Response.Redirect("/Resume/home");

    return Task.CompletedTask;
});
app.UseAuthorization();
//app.UseOutputCache();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();
