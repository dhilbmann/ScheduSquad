using Microsoft.AspNetCore.Authentication.Cookies;
using ScheduSquad.DataAccess;
using ScheduSquad.Models;
using ScheduSquad.Service;
using ScheduSquad.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Used to get connection string for the IDbConfiguration classes
builder.Configuration.AddJsonFile("appsettings.json");

// Add DI classes/interfaces to container
builder.Services.AddTransient<IDbConfiguration, SqlExpressDbConfiguration>();
builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
builder.Services.AddScoped<IRepository<Availability>, AvailabilityRepository>();
builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
builder.Services.AddScoped<IRepository<Member>, MemberRepository>();
builder.Services.AddScoped<IMembersForSquadRepository, MemberRepository>();
builder.Services.AddScoped<IRepository<Squad>, SquadRepository>();
builder.Services.AddScoped<ISquadMemberRepository, SquadRepository>();
builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ISquadService, SquadService>();
builder.Services.AddScoped<ILoginAuthenticationService, LoginAuthenticationService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
    {
        options.Cookie.Name = "SSAuthorization";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Use(async (context, next) =>
{
    if (context.User?.Identity?.IsAuthenticated != true && !context.Request.Path.StartsWithSegments("/Account"))
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next();
});

app.Run();
