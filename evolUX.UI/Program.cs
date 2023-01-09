using evolUX.UI.Areas.Core.Services;
using evolUX.UI.Areas.Core.Services.Interfaces;
using evolUX.UI.Areas.EvolDP.Services;
using evolUX.UI.Areas.EvolDP.Services.Interfaces;
using evolUX.UI.Areas.Finishing.Services;
using evolUX.UI.Areas.Finishing.Services.Interfaces;
using evolUX.UI.Filters;
using evolUX.UI.Repositories;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Core/Auth/Index";
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
builder.Services.AddSingleton<IDocCodeService, DocCodeService>();
builder.Services.AddSingleton<IDocCodeRepository, DocCodeRepository>();
builder.Services.AddSingleton<IExpeditionTypeRepository, ExpeditionTypeRepository>();
builder.Services.AddSingleton<IExpeditionTypeService, ExpeditionTypeService>();
builder.Services.AddSingleton<IProductionReportService, ProductionReportService>();
builder.Services.AddSingleton<IProductionReportRepository, ProductionReportRepository>();
builder.Services.AddSingleton<IPrintRepository, PrintRepository>();
builder.Services.AddSingleton<IPrintService, PrintService>();
builder.Services.AddSingleton<IConcludedPrintService, ConcludedPrintService>();
builder.Services.AddSingleton<IConcludedPrintRepository, ConcludedPrintRepository>();
builder.Services.AddSingleton<IConcludedEnvelopeService, ConcludedEnvelopeService>();
builder.Services.AddSingleton<IConcludedEnvelopeRepository, ConcludedEnvelopeRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AuthorizeFilter("Cookie"));
    options.Filters.Add<BreadcrumbActionFilter>();
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = new List<CultureInfo> {
            new CultureInfo("pt"),
            new CultureInfo("en")
            };
        options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures= supportedCultures;
    }
    );
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
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
app.UseSession();
//var supportedCultures = new[] { "pt", "es" };
//var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures.First())
//    .AddSupportedCultures(supportedCultures)
//    .AddSupportedUICultures(supportedCultures);
//app.UseRequestLocalization(localizationOptions);

app.MapControllerRoute(
    name: "MyAreas",
    pattern: "{area:exists}/{controller}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Core}/{controller=Auth}/{action=Index}/{id?}");



app.Run();
