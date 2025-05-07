using Contacts.Application;
using Contacts.Infrastructure;
using Contacts.Api.Endpoints.PhoneNumbers;
using Contacts.Api.Endpoints.Hrm;

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
builder.Services.AddOpenApi();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Configuration.AddJsonFile("./appsettings.Production.json", false);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseHttpsRedirection();

var api = app.MapGroup("api");

api.MapPhoneNumbers();
api.MapHrm();

app.Run();
