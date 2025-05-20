using Contacts.Application;
using Contacts.Infrastructure;
using Contacts.Api.Endpoints.PhoneNumbers;
using Contacts.Api.Endpoints.Hrm;
using Contacts.Api.Endpoints.Users;
using Contacts.Infrastructure.Persistance;
using Contacts.Application.Common.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Contacts.Api.Endpoints.Auth;
using Contacts.Api.Endpoints.Accounts;
using Serilog;
using Api;
using System.Text.Json;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Configuration.AddJsonFile("./appsettings.Production.json", false);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()!;

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtOptions.AccessTokenSecretKey)),
    };
});

builder.Services.AddAuthorization();

var configuration = new LoggerConfiguration()
              .MinimumLevel.Information()
              .WriteTo.Console();
Log.Logger = configuration.CreateLogger();

builder.Services.AddSerilog(Log.Logger);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

await Seeder.SeedData(app.Services);

var json = Path.Combine(app.Environment.ContentRootPath, "Resources", "Errors.json");

var jsonText = File.ReadAllText(json);

var errors = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonText);

Translation.AddTranslations(errors);

var api = app.MapGroup("api");

api.MapPhoneNumbers();
api.MapHrm();
api.MapUsers();
api.MapAuth();
api.MapAccounts();

app.Run();
