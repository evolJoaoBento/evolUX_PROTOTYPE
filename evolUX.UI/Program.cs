using evolUX.UI.Repositories;
using evolUX.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Core/Auth/LoginPost";
    options.LogoutPath = "/Core/Auth/Logout";
    options.AccessDeniedPath = "/Core/Auth/AccessDenied";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    options.Cookie.SameSite = SameSiteMode.Strict;
    

});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Cookie", policy =>
    {
        policy.AuthenticationSchemes.Add(CookieAuthenticationDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IAuthRepository, AuthRepository>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter("Cookie"));
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

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MyAreas",
    pattern: "{area:exists}/{controller}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Core}/{controller=Auth}/{action=Index}/{id?}");


app.Run();
