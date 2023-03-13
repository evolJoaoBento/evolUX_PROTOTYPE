using evolUX.API.Areas.Core.Services;
using evolUX.API.Areas.Core.Services.Interfaces;
using evolUX.API.Areas.evolDP.Services;
using evolUX.API.Areas.evolDP.Services.Interfaces;
using evolUX.API.Areas.Finishing.Services;
using evolUX.API.Areas.Finishing.Services.Interfaces;
using evolUX.API.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Session;
using System.Text;
using System.Text.Json.Serialization;
using evolUX.API.Areas.Core.Repositories.Interfaces;
using evolUX.API.Areas.Core.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Core
builder.Services.AddMvc();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<ISessionService, SessionService>();
builder.Services.AddSingleton<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<IWrapperRepository, WrapperRepository>();

//evolDP
builder.Services.AddSingleton<IDocCodeService, DocCodeService>();
builder.Services.AddSingleton<IGenericService, GenericService>();
builder.Services.AddSingleton<IExpeditionService, ExpeditionService>();
builder.Services.AddSingleton<IConsumablesService, ConsumablesService>();
builder.Services.AddSingleton<IServiceProvisionService, ServiceProvisionService>();

//Finishing
builder.Services.AddSingleton<IProductionReportService, ProductionReportService>();
builder.Services.AddSingleton<IPendingRegistService, PendingRegistService>();
builder.Services.AddSingleton<IPrintService, PrintService>();
builder.Services.AddSingleton<IConcludedPrintService, ConcludedPrintService>();
builder.Services.AddSingleton<IConcludedFullfillService, ConcludedFullfillService>();
builder.Services.AddSingleton<IRecoverService, RecoverService>();
builder.Services.AddSingleton<IPostalObjectService, PostalObjectService>();
builder.Services.AddSingleton<IPendingRecoverService, PendingRecoverService>();
builder.Services.AddSingleton<IExpeditionReportService, ExpeditionReportService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //https://localhost:7107/ api dev
        //http://localhost:5100/ api prod
        //https://localhost:7067 ui dev
        //http://localhost:86 ui prod
        ValidIssuer = builder.Configuration.GetValue<string>("APIurl"),
        ValidAudience = builder.Configuration.GetValue<string>("UIurl"),
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("AppSettings:Secret")))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    // Swagger 2.+ support
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    //option.CustomSchemaIds(type => type.ToString());
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MyAreas",
    pattern: "{area:exists}/{controller}/{action=Index}/{id?}");

app.Run();
