using Contacts.Application;
using Contacts.Contracts.PhoneNumbers;
using Contacts.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models;
using Contacts.Api.Endpoints.Structures;
using Contacts.Api.Endpoints.PhoneNumbers;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();

var api = app.MapGroup("api");

api.MapOrganizationStructure();
api.MapPhoneNumbers();

app.Run();
