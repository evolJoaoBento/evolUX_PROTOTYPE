using evolUX.Context;
using evolUX.Interfaces;
using evolUX.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.IISIntegration;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IExpeditionTypeRepository, ExpeditionTypeRepository>();
builder.Services.AddScoped<IEnvelopeMediaRepository, EnvelopeMediaRepository>();
builder.Services.AddScoped<IFinishingRepository, FinishingRepository>();
builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();